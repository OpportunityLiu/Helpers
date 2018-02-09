using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncOperation<T> : MulticastAsyncBase, IAsyncOperation<T>
    {
        private IAsyncOperation<T> action;
        protected override IAsyncInfo GetWrapped() => this.action;

        public MulticastAsyncOperation(IAsyncOperation<T> operation)
        {
            this.action = operation ?? throw new ArgumentNullException(nameof(operation));
            this.action.Completed = this.action_Completed;
        }

        private void action_Completed(IAsyncOperation<T> sender, AsyncStatus e)
        {
            if (this.action != null)
                this.completed?.Invoke(this, e);
        }

        private AsyncOperationCompletedHandler<T> completed;
        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed
        {
            get => this.completed;
            set => this.completed += value;
        }

        public override void Close()
        {
            if (this.action == null)
                return;
            this.action.Close();
            this.action = null;
            this.completed = null;
        }

        T IAsyncOperation<T>.GetResults() => this.action.GetResults();
    }
}
