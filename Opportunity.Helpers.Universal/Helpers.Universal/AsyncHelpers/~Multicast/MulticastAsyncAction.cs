using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncAction : MulticastAsyncBase, IAsyncAction
    {
        private IAsyncAction action;
        protected override IAsyncInfo GetWrapped() => this.action;

        public MulticastAsyncAction(IAsyncAction action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.action.Completed = this.action_Completed;
        }

        private void action_Completed(IAsyncAction sender, AsyncStatus e)
        {
            if (this.action != null)
                this.completed?.Invoke(this, e);
        }

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

        public override void Close()
        {
            if (this.action == null)
                return;
            this.action.Close();
            this.action = null;
            this.completed = null;
        }

        void IAsyncAction.GetResults() => this.action.GetResults();
    }
}
