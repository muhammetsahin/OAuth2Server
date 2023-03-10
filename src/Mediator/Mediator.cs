using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Mediator
{
    public class Mediator : MediatR.Mediator, IMediator
    {
        private PublishStrategy _strategy = PublishStrategy.ParallelNoWait;


        public Mediator(IServiceProvider serviceFactory)
            : base(serviceFactory)
        {
        }


        public IMediator SetPublishStrategy(PublishStrategy strategy)
        {
            _strategy = strategy;
            return this;
        }

        private static Task ParallelWhenAll(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>(handlers.Count());
            tasks.AddRange(handlers.Select(handler => Task.Run(() => handler.HandlerCallback(notification, cancellationToken))));

            return Task.WhenAll(tasks);
        }

        private static Task ParallelWhenAny(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>(handlers.Count());
            tasks.AddRange(handlers.Select(handler => Task.Run(() => handler.HandlerCallback(notification, cancellationToken))));
            return Task.WhenAny(tasks);
        }

        private static Task ParallelNoWait(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            Parallel.ForEach(handlers, (handler) => { handler.HandlerCallback(notification, cancellationToken); });

            return Task.CompletedTask;
        }

        private static async Task AsyncContinueOnException(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>(handlers.Count());
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    tasks.Add(handler.HandlerCallback(notification, cancellationToken));
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        private static async Task SyncStopOnException(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            foreach (var handler in handlers)
            {
                await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task SyncContinueOnException(IEnumerable<NotificationHandlerExecutor> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
                }
                catch (AggregateException ex)
                {
                    exceptions.AddRange(ex.Flatten().InnerExceptions);
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override Task PublishCore(IEnumerable<NotificationHandlerExecutor> allHandlers, INotification notification, CancellationToken cancellationToken) => _strategy switch
        {
            PublishStrategy.SyncContinueOnException => SyncContinueOnException(allHandlers, notification,
                cancellationToken),
            PublishStrategy.SyncStopOnException =>
                SyncStopOnException(allHandlers, notification, cancellationToken),
            PublishStrategy.Async => AsyncContinueOnException(allHandlers, notification, cancellationToken),
            PublishStrategy.ParallelNoWait => ParallelNoWait(allHandlers, notification, cancellationToken),
            PublishStrategy.ParallelWhenAll => ParallelWhenAll(allHandlers, notification, cancellationToken),
            PublishStrategy.ParallelWhenAny => ParallelWhenAny(allHandlers, notification, cancellationToken),
            _ => throw new ArgumentException("Invalid strategy")
        };
        //
        // protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers,
        //     INotification notification, CancellationToken cancellationToken) => _strategy switch
        // {
        //     PublishStrategy.SyncContinueOnException => SyncContinueOnException(allHandlers, notification,
        //         cancellationToken),
        //     PublishStrategy.SyncStopOnException =>
        //     SyncStopOnException(allHandlers, notification, cancellationToken),
        //     PublishStrategy.Async => AsyncContinueOnException(allHandlers, notification, cancellationToken),
        //     PublishStrategy.ParallelNoWait => ParallelNoWait(allHandlers, notification, cancellationToken),
        //     PublishStrategy.ParallelWhenAll => ParallelWhenAll(allHandlers, notification, cancellationToken),
        //     PublishStrategy.ParallelWhenAny => ParallelWhenAny(allHandlers, notification, cancellationToken),
        //     _ => throw new ArgumentException("Invalid strategy")
        // };
    }
}