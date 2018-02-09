using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal abstract class CompletedAsyncBase : IAsyncInfo
    {
        protected CompletedAsyncBase(AsyncStatus status, Exception error)
        {
            Status = status;
            this.error = error;
        }

        private Exception error;
        Exception IAsyncInfo.ErrorCode => this.error;

        uint IAsyncInfo.Id => unchecked((uint)GetHashCode());

        public AsyncStatus Status { get; }

        void IAsyncInfo.Cancel() { }

        public virtual void Close()
        {
            (this.error as IDisposable)?.Dispose();
            this.error = null;
        }

        public void GetResults()
        {
            var error = this.error;
            if (error != null)
                throw new AggregateException(error);
        }
    }
}
