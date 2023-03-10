namespace Mediator
{
    public enum PublishStrategy
    {
        SyncContinueOnException,
        SyncStopOnException,
        Async,
        ParallelNoWait,
        ParallelWhenAll,
        ParallelWhenAny,
    }
}