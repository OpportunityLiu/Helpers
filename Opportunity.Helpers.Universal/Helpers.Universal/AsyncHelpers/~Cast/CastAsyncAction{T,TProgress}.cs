using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CastAsyncAction<T, TProgress> : CastAsyncBase, IAsyncActionWithProgress<TProgress>
    {
        internal readonly IAsyncOperationWithProgress<T, TProgress> Operation;
        protected override IAsyncInfo GetWrapped() => this.Operation;

        public CastAsyncAction(IAsyncOperationWithProgress<T, TProgress> operation)
        {
            this.Operation = operation ?? throw new ArgumentNullException(nameof(operation));
            this.Operation.Progress = operationProgress;
        }

        void IAsyncActionWithProgress<TProgress>.GetResults() => this.Operation.GetResults();

        private void operationCompleted(IAsyncOperationWithProgress<T, TProgress> asyncInfo, AsyncStatus asyncStatus)
            => this.completed?.Invoke(this, asyncStatus);
        private AsyncActionWithProgressCompletedHandler<TProgress> completed;
        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed
        {
            get => this.completed;
            set
            {
                this.completed = value;
                this.Operation.Completed = operationCompleted;
            }
        }

        private void operationProgress(IAsyncOperationWithProgress<T, TProgress> asyncInfo, TProgress progressInfo)
            => this.progressHandler?.Invoke(this, progressInfo);
        private AsyncActionProgressHandler<TProgress> progressHandler;
        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress
        {
            get => this.progressHandler;
            set
            {
                this.progressHandler = value;
                this.Operation.Progress = operationProgress;
            }
        }
    }
}
