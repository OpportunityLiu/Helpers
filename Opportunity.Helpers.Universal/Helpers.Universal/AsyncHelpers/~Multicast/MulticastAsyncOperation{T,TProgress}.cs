using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncOperation<T, TProgress> : MulticastAsyncBase, IAsyncOperationWithProgress<T, TProgress>
    {
        private IAsyncOperationWithProgress<T, TProgress> action;
        protected override IAsyncInfo GetWrapped() => this.action;

        public MulticastAsyncOperation(IAsyncOperationWithProgress<T, TProgress> operation)
        {
            this.action = operation ?? throw new ArgumentNullException(nameof(operation));
            this.action.Completed = this.action_Completed;
            this.action.Progress = this.action_Progress;
        }

        private void action_Completed(IAsyncOperationWithProgress<T, TProgress> sender, AsyncStatus e)
        {
            if (this.action != null)
                this.completed?.Invoke(this, e);
        }

        private void action_Progress(IAsyncOperationWithProgress<T, TProgress> asyncInfo, TProgress progressInfo)
        {
            if (this.action != null)
                this.progress?.Invoke(this, progressInfo);
        }

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

        public override void Close()
        {
            if (this.action is null)
                return;
            this.action.Close();
            this.action = null;
            this.progress = null;
            this.completed = null;
        }

        T IAsyncOperationWithProgress<T, TProgress>.GetResults() => this.action.GetResults();
    }
}
