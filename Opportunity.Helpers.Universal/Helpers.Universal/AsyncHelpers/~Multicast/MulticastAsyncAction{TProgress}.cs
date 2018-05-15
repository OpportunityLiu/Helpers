using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncAction<TProgress> : MulticastAsyncBase, IAsyncActionWithProgress<TProgress>
    {
        public MulticastAsyncAction(IAsyncActionWithProgress<TProgress> action) : base(action)
        {
            action.Completed = this.action_Completed;
            action.Progress = this.action_Progress;
        }

        private void action_Completed(IAsyncActionWithProgress<TProgress> sender, AsyncStatus e)
            => this.completed?.Invoke(this, e);

        private void action_Progress(IAsyncActionWithProgress<TProgress> asyncInfo, TProgress progressInfo)
            => this.progress?.Invoke(this, progressInfo);

        private AsyncActionWithProgressCompletedHandler<TProgress> completed;
        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed
        {
            get => this.completed;
            set
            {
                this.completed -= value;
                this.completed += value;
            }
        }

        private AsyncActionProgressHandler<TProgress> progress;
        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress
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

        void IAsyncActionWithProgress<TProgress>.GetResults()
            => ((IAsyncActionWithProgress<TProgress>)Wrapped).GetResults();
    }
}
