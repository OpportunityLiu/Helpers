using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Base class for async actions and async operations.
    /// </summary>
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

        public void RegisterCancellation(Action CanceledCallback)
        {
            this.CanceledCallback += CanceledCallback;
        }

        private Action CanceledCallback;

        internal bool PreCancel()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Canceled, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            this.CanceledCallback?.Invoke();
            return true;
        }

        internal bool PreSetResults()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Completed, (int)AsyncStatus.Started);
            return state == AsyncStatus.Started;
        }

        internal bool PreSetException(Exception ex)
        {
            if (ex is null)
                throw new ArgumentNullException(nameof(ex));
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Error, (int)AsyncStatus.Started);
            if (state == AsyncStatus.Started)
            {
                this.error = ex;
                return true;
            }
            return false;
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

        /// <summary>
        /// Id of current <see cref="IAsyncInfo"/>.
        /// </summary>
        public uint Id => unchecked((uint)GetHashCode());

        private int status = (int)AsyncStatus.Started;
        /// <summary>
        /// Status of current <see cref="IAsyncInfo"/>.
        /// </summary>
        public AsyncStatus Status => (AsyncStatus)this.status;

        /// <summary>
        /// End this <see cref="IAsyncInfo"/>.
        /// </summary>
        public virtual void Close()
        {
            (this.error as IDisposable)?.Dispose();
            this.error = null;
        }
    }
}
