namespace DataUri
{
    using System;
    using System.Threading;

    internal sealed class DataUriAsyncResult : IAsyncResult
    {
        public DataUriAsyncResult(object asyncState)
        {
            this.AsyncState = asyncState;
        }

        public object AsyncState { get; }

        public WaitHandle AsyncWaitHandle => new AutoResetEvent(true);

        public bool CompletedSynchronously => true;

        public bool IsCompleted => true;
    }
}
