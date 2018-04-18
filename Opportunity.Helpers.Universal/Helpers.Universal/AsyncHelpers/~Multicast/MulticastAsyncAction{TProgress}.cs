using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncAction<TProgress> : MulticastAsyncBase, IAsyncActionWithProgress<TProgress>
    {
        private IAsyncActionWithProgress<TProgress> action;
        protected override IAsyncInfo GetWrapped() => this.action;

        public MulticastAsyncAction(IAsyncActionWithProgress<TProgress> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.action.Completed = this.action_Completed;
            this.action.Progress = this.action_Progress;
        }

        private void action_Completed(IAsyncActionWithProgress<TProgress> sender, AsyncStatus e)
        {
            if (this.action != null)
                this.completed?.Invoke(this, e);
        }

        private void action_Progress(IAsyncActionWithProgress<TProgress> asyncInfo, TProgress progressInfo)
        {
            if (this.action != null)
                this.progress?.Invoke(this, progressInfo);
        }

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

        public override void Close()
        {
            if (this.action is null)
                return;
            this.action.Close();
            this.action = null;
            this.completed = null;
            this.progress = null;
        }

        void IAsyncActionWithProgress<TProgress>.GetResults() => this.action.GetResults();
    }
}
