using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class AsyncInfoBase : IAsyncInfo
    {
        internal AsyncInfoBase() { }

        private string DebuggerDisplay
        {
            get
            {
                try
                {
                    switch (Status)
                    {
                    case AsyncStatus.Canceled:
                        return "Status = Canceled";
                    case AsyncStatus.Completed:
                        return "Status = Completed";
                    case AsyncStatus.Error:
                        return $"Status = Error, ErrorCode = {this.error}";
                    case AsyncStatus.Started:
                        return $"Status = Started, CanceledCallback = {this.CanceledCallback?.ToString() ?? "null"}";
                    default:
                        return "Status = Unknown";
                    }
                }
                catch (InvalidOperationException)
                {
                    return "Status = Closed";
                }
            }
        }

        /// <summary>
        /// Cancel the <see cref="IAsyncInfo"/>.
        /// </summary>
        public void Cancel()
        {
            if (Volatile.Read(ref this.status) != (int)AsyncStatus.Started)
                return;
            var cb = Interlocked.Exchange(ref this.CanceledCallback, null);
            cb?.Invoke();
        }

        /// <summary>
        /// Register a callback for <see cref="Cancel()"/>.
        /// </summary>
        /// <param name="canceledCallback">Callback to register.</param>
        /// <remarks>
        /// <paramref name="canceledCallback"/> will only be invoked when <see cref="Status"/> is <see cref="AsyncStatus.Started"/>,
        /// you can call <see cref="TrySetCanceled()"/> in the callback.</remarks>
        public void RegisterCancellation(Action canceledCallback)
        {
            if (this.Status != AsyncStatus.Started)
                return;
            this.CanceledCallback += canceledCallback;
        }

        private Action CanceledCallback;

        internal abstract void OnCompleted();

        internal void GetCompleted()
        {
            switch (Status)
            {
            case AsyncStatus.Canceled:
                throw new OperationCanceledException();
            case AsyncStatus.Completed:
                return;
            case AsyncStatus.Error:
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

        /// <summary>
        /// Set <see cref="Status"/> to <see cref="AsyncStatus.Canceled"/>.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="Status"/> is previously <see cref="AsyncStatus.Started"/> and be set to <see cref="AsyncStatus.Canceled"/> successfully, otherwise, <see langword="false"/>.</returns>
        public bool TrySetCanceled()
        {
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Canceled, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            OnCompleted();
            return true;
        }

        /// <summary>
        /// Set <see cref="Status"/> to <see cref="AsyncStatus.Error"/>.
        /// </summary>
        /// <param name="ex"><see cref="ErrorCode"/> for the async info.</param>
        /// <returns><see langword="true"/> if <see cref="Status"/> is previously <see cref="AsyncStatus.Started"/> and be set to <see cref="AsyncStatus.Error"/> successfully, otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is <see langword="null"/>.</exception>
        public bool TrySetException(Exception ex)
        {
            if (ex is null)
                throw new ArgumentNullException(nameof(ex));
            var state = (AsyncStatus)Interlocked.CompareExchange(ref this.status, (int)AsyncStatus.Error, (int)AsyncStatus.Started);
            if (state != AsyncStatus.Started)
                return false;
            this.error = ex;
            OnCompleted();
            return true;
        }

        private Exception error;
        /// <summary>
        /// Error of current <see cref="IAsyncInfo"/>., valid only if <see cref="Status"/> is <see cref="AsyncStatus.Error"/>,
        /// otherwise, <see langword="null"/>.
        /// </summary>
        public Exception ErrorCode
        {
            get
            {
                if (this.Status != AsyncStatus.Error)
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int status = (int)AsyncStatus.Started;
        /// <summary>
        /// Status of current <see cref="IAsyncInfo"/>.
        /// </summary>
        public AsyncStatus Status
        {
            get
            {
                var s = Volatile.Read(ref this.status);
                if (s == CLOSED_STATUS)
                    throw new InvalidOperationException("The async info has been closed.");
                return (AsyncStatus)s;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const int CLOSED_STATUS = 2137643278;

        /// <summary>
        /// End the <see cref="IAsyncInfo"/>.
        /// </summary>
        public virtual void Close()
        {
            Volatile.Write(ref this.status, CLOSED_STATUS);
            (Interlocked.Exchange(ref this.error, null) as IDisposable)?.Dispose();
            this.CanceledCallback = null;
        }
    }
}
