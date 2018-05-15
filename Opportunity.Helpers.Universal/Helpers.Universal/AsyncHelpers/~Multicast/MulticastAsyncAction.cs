using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncAction : MulticastAsyncBase, IAsyncAction
    {
        public MulticastAsyncAction(IAsyncAction action) : base(action)
        {
            action.Completed = this.action_Completed;
        }

        private void action_Completed(IAsyncAction sender, AsyncStatus e)
            => this.completed?.Invoke(this, e);

        private AsyncActionCompletedHandler completed;
        AsyncActionCompletedHandler IAsyncAction.Completed
        {
            get => this.completed;
            set
            {
                this.completed -= value;
                this.completed += value;
            }
        }

        protected override void CloseOverride()
        {
            this.completed = null;
        }

        void IAsyncAction.GetResults()
            => ((IAsyncAction)Wrapped).GetResults();
    }
}
