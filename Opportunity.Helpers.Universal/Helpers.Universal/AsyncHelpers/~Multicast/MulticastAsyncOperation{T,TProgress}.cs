using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncOperation<T, TProgress> : MulticastAsyncBase, IAsyncOperationWithProgress<T, TProgress>
    {
        public MulticastAsyncOperation(IAsyncOperationWithProgress<T, TProgress> operation) : base(operation)
        {
            operation.Completed = this.operation_Completed;
            operation.Progress = this.operation_Progress;
        }

        private void operation_Completed(IAsyncOperationWithProgress<T, TProgress> sender, AsyncStatus e)
            => this.completed?.Invoke(this, e);

        private void operation_Progress(IAsyncOperationWithProgress<T, TProgress> asyncInfo, TProgress progressInfo)
            => this.progress?.Invoke(this, progressInfo);

        private AsyncOperationWithProgressCompletedHandler<T, TProgress> completed;
        AsyncOperationWithProgressCompletedHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Completed
        {
            get => this.completed;
            set
            {
                this.completed -= value;
                this.completed += value;
            }
        }

        private AsyncOperationProgressHandler<T, TProgress> progress;
        AsyncOperationProgressHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Progress
        {
            get => this.progress;
            set
            {
                this.progress -= value;
                this.progress += value;
            }
        }

        protected override void CloseOverride()
        {
            this.completed = null;
            this.progress = null;
        }

        T IAsyncOperationWithProgress<T, TProgress>.GetResults()
            => ((IAsyncOperationWithProgress<T, TProgress>)Wrapped).GetResults();
    }
}
