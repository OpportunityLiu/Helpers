using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class MulticastAsyncOperation<T> : MulticastAsyncBase, IAsyncOperation<T>
    {
        public MulticastAsyncOperation(IAsyncOperation<T> operation) : base(operation)
        {
            operation.Completed = this.operation_Completed;
        }

        private void operation_Completed(IAsyncOperation<T> sender, AsyncStatus e)
            => this.completed?.Invoke(this, e);

        private AsyncOperationCompletedHandler<T> completed;
        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed
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

        T IAsyncOperation<T>.GetResults()
            => ((IAsyncOperation<T>)Wrapped).GetResults();
    }
}
