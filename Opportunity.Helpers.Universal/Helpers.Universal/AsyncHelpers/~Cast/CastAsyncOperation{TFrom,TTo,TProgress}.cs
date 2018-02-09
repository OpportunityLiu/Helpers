using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CastAsyncOperation<TFrom, TTo, TProgress> : CastAsyncBase, IAsyncOperationWithProgress<TTo, TProgress>
    {
        internal readonly IAsyncOperationWithProgress<TFrom, TProgress> Operation;
        protected override IAsyncInfo GetWrapped() => this.Operation;

        public CastAsyncOperation(IAsyncOperationWithProgress<TFrom, TProgress> operation)
        {
            this.Operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        TTo IAsyncOperationWithProgress<TTo, TProgress>.GetResults() => (TTo)(object)this.Operation.GetResults();

        private void operationCompletedHandler(IAsyncOperationWithProgress<TFrom, TProgress> asyncInfo, AsyncStatus asyncStatus)
            => this.completed?.Invoke(this, asyncStatus);
        private AsyncOperationWithProgressCompletedHandler<TTo, TProgress> completed;
        AsyncOperationWithProgressCompletedHandler<TTo, TProgress> IAsyncOperationWithProgress<TTo, TProgress>.Completed
        {
            get => this.completed;
            set
            {
                this.completed = value;
                this.Operation.Completed = this.operationCompletedHandler;
            }
        }


        private void operationProgressHandler(IAsyncOperationWithProgress<TFrom, TProgress> asyncInfo, TProgress progressInfo)
            => this.progress?.Invoke(this, progressInfo);
        private AsyncOperationProgressHandler<TTo, TProgress> progress;
        AsyncOperationProgressHandler<TTo, TProgress> IAsyncOperationWithProgress<TTo, TProgress>.Progress
        {
            get => this.progress;
            set
            {
                this.progress = value;
                this.Operation.Progress = this.operationProgressHandler;
            }
        }
    }
}
