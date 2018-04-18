using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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
        internal AsyncInfoBase() { }

        public void Cancel()
        {
            if (this.Status != AsyncStatus.Started)
                return;
            var cb = Interlocked.Exchange(ref this.CanceledCallback, null);
            cb?.Invoke();
        }

        public void RegisterCancellation(Action CanceledCallback)
        {
            this.CanceledCallback += CanceledCallback;
        }

        private Action CanceledCallback;

        internal abstract void OnCompleted();

        internal void GetCompleted()
        {
            switch (this.status)
            {
            case (int)AsyncStatus.Canceled:
                throw new OperationCanceledException();
            case (int)AsyncStatus.Completed:
                return;
            case (int)AsyncStatus.Error:
                ExceptionDispatchInfo.Capture(ErrorCode).Throw();
                return;
            default:
                throw new InvalidOperationException("The async info has not finished yet.");
            }
        }

        internal bool TrySetCompleted()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Completed, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            OnCompleted();
            return true;
        }

        public bool TrySetCanceled()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Canceled, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            OnCompleted();
            return true;
        }

        public bool TrySetException(Exception ex)
        {
            if (ex is null)
                throw new ArgumentNullException(nameof(ex));
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Error, (int)AsyncStatus.Started);
            if (state == AsyncStatus.Started)
            {
                this.error = ex;
                OnCompleted();
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
                if (error is null)
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
            Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Completed, (int)AsyncStatus.Started);
        }
    }
}
