namespace Mediator
{
    public interface IMediator : MediatR.IMediator
    {
        public IMediator SetPublishStrategy(PublishStrategy strategy);
    }
}