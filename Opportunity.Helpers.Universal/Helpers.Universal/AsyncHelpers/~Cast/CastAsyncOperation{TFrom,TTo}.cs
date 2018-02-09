using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CastAsyncOperation<TFrom, TTo> : CastAsyncBase, IAsyncOperation<TTo>
    {
        internal readonly IAsyncOperation<TFrom> Operation;
        protected override IAsyncInfo GetWrapped() => this.Operation;

        public CastAsyncOperation(IAsyncOperation<TFrom> operation)
        {
            this.Operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        TTo IAsyncOperation<TTo>.GetResults() => (TTo)(object)this.Operation.GetResults();

        private void operationCompletedHandler(IAsyncOperation<TFrom> asyncInfo, AsyncStatus asyncStatus)
            => this.completed?.Invoke(this, asyncStatus);
        private AsyncOperationCompletedHandler<TTo> completed;
        AsyncOperationCompletedHandler<TTo> IAsyncOperation<TTo>.Completed
        {
            get => this.completed;
            set
            {
                this.completed = value;
                this.Operation.Completed = this.operationCompletedHandler;
            }
        }
    }
}
