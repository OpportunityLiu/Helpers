using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CompletedAsyncOperation<T> : CompletedAsyncBase, IAsyncOperation<T>
    {
        internal CompletedAsyncOperation(AsyncStatus status, T result, Exception error)
            : base(status, error)
        {
            this.result = result;
        }

        private T result;

        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed { get => null; set => value?.Invoke(this, this.Status); }

        public override void Close()
        {
            (this.result as IDisposable)?.Dispose();
            this.result = default;
            base.Close();
        }

        T IAsyncOperation<T>.GetResults()
        {
            GetResults();
            return this.result;
        }
    }
}
