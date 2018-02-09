using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal abstract class MulticastAsyncBase : IAsyncInfo
    {
        protected abstract IAsyncInfo GetWrapped();

        void IAsyncInfo.Cancel() => GetWrapped().Cancel();
        public abstract void Close();
        Exception IAsyncInfo.ErrorCode => GetWrapped().ErrorCode;
        uint IAsyncInfo.Id => GetWrapped().Id;
        AsyncStatus IAsyncInfo.Status => GetWrapped().Status;
    }
}