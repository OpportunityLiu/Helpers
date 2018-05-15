using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal abstract class MulticastAsyncBase : IAsyncInfo
    {
        protected MulticastAsyncBase(IAsyncInfo wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }

        private IAsyncInfo wrapped;
        protected IAsyncInfo Wrapped => this.wrapped ?? throw new ObjectDisposedException(this.ToString());

        void IAsyncInfo.Cancel() => this.wrapped?.Cancel();
        void IAsyncInfo.Close()
        {
            var wrapped = Interlocked.Exchange(ref this.wrapped, null);
            if (wrapped is null)
                return;
            wrapped.Close();
            CloseOverride();
        }
        Exception IAsyncInfo.ErrorCode => this.Wrapped.ErrorCode;
        uint IAsyncInfo.Id => this.Wrapped.Id;
        AsyncStatus IAsyncInfo.Status => this.Wrapped.Status;

        protected abstract void CloseOverride();
    }
}