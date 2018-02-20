using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public abstract class AsyncInfoBase : IAsyncInfo
    {
        internal void PreGetResults()
        {
            switch (this.status)
            {
            case (int)AsyncStatus.Canceled:
                throw new OperationCanceledException();
            case (int)AsyncStatus.Completed:
                return;
            case (int)AsyncStatus.Error:
                throw ErrorCode;
            default:
                throw new InvalidOperationException("The async info has not finished yet.");
            }
        }

        public abstract void Cancel();

        public void RegisterCancellation(Action cancelledCallback)
        {
            this.cancelledCallback += cancelledCallback;
        }

        private Action cancelledCallback;

        internal bool PreCancel()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Canceled, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            this.cancelledCallback?.Invoke();
            return true;
        }

        internal void PreSetResults()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Completed, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                throw new InvalidOperationException("Cancel, SetResults or SetException has been called.");
        }

        internal void PreSetException(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Error, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                throw new InvalidOperationException("Cancel, SetResults or SetException has been called.");
            if (Interlocked.CompareExchange(ref this.error, ex, null) != null)
                throw new InvalidOperationException("Cancel, SetResults or SetException has been called.");
        }

        private Exception error;
        public Exception ErrorCode
        {
            get
            {
                if (this.status != (int)AsyncStatus.Error)
                    return null;
                var error = this.error;
                if (error == null)
                    return new Exception();
                return error;
            }
        }

        public uint Id => unchecked((uint)GetHashCode());

        private int status = (int)AsyncStatus.Started;
        public AsyncStatus Status => (AsyncStatus)this.status;

        public virtual void Close()
        {
            (this.error as IDisposable)?.Dispose();
            this.error = null;
        }
    }
}
