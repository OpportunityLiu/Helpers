using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CastAsyncAction<T> : CastAsyncBase, IAsyncAction
    {
        internal readonly IAsyncOperation<T> Operation;
        protected override IAsyncInfo GetWrapped() => this.Operation;

        public CastAsyncAction(IAsyncOperation<T> operation)
        {
            this.Operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        void IAsyncAction.GetResults() => this.Operation.GetResults();

        private void operationCompleted(IAsyncOperation<T> asyncInfo, AsyncStatus asyncStatus)
            => this.completed?.Invoke(this, asyncStatus);
        private AsyncActionCompletedHandler completed;
        AsyncActionCompletedHandler IAsyncAction.Completed
        {
            get => this.completed;
            set
            {
                this.completed = value;
                this.Operation.Completed = this.operationCompleted;
            }
        }
    }
}
