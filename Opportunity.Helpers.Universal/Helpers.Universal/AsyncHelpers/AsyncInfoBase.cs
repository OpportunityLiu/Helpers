using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public abstract class AsyncInfoBase : IAsyncInfo
    {
        internal void PreGetResults()
        {
            switch (Status)
            {
            case AsyncStatus.Canceled:
                throw new OperationCanceledException();
            case AsyncStatus.Completed:
                return;
            case AsyncStatus.Error:
                throw ErrorCode;
            default:
                throw new InvalidOperationException();
            }
        }

        public void RegisterCancellation(Action cancelledCallback)
        {
            this.cancelledCallback += cancelledCallback;
        }
        private Action cancelledCallback;
        public void Cancel()
        {
            if (Status != AsyncStatus.Started)
                return;
            this.cancelledCallback?.Invoke();
            this.Status = AsyncStatus.Canceled;
        }

        public virtual void Close()
        {
            (this.error as IDisposable)?.Dispose();
            this.error = null;
        }

        internal void PreSetResults()
        {
            if (Status != AsyncStatus.Started)
                throw new InvalidOperationException("Cancel, SetResults or SetException has been called.");
            this.Status = AsyncStatus.Completed;
        }

        public virtual void SetException(Exception ex)
        {
            if (Status != AsyncStatus.Started)
                throw new InvalidOperationException("Cancel, SetResults or SetException has been called.");
            this.error = ex ?? throw new ArgumentNullException(nameof(ex));
            Status = AsyncStatus.Error;
        }

        private Exception error;
        public Exception ErrorCode
        {
            get
            {
                if (Status != AsyncStatus.Error)
                    return null;
                var error = this.error;
                if (error == null)
                    return new Exception();
                return error;
            }
        }

        public uint Id => unchecked((uint)GetHashCode());

        public AsyncStatus Status { get; private set; }
    }
}
