using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class FaultedAsyncInfo<T, TProgress>
        : IAsyncAction, IAsyncActionWithProgress<TProgress>, IAsyncOperation<T>, IAsyncOperationWithProgress<T, TProgress>
    {
        public static FaultedAsyncInfo<T, TProgress> Instanse { get; } = new FaultedAsyncInfo<T, TProgress>(null);
        public static FaultedAsyncInfo<T, TProgress> Create(Exception ex)
            => new FaultedAsyncInfo<T, TProgress>(ex ?? throw new ArgumentNullException(nameof(ex)));

        private object error;

        private FaultedAsyncInfo(Exception ex) => this.error = ex;

        uint IAsyncInfo.Id => unchecked((uint)GetHashCode());
        AsyncStatus IAsyncInfo.Status => AsyncStatus.Error;
        void IAsyncInfo.Cancel() { }
        void IAsyncInfo.Close()
        {
            if (this.error is Exception ex)
            {
                this.error = ex.Message;
                (ex as IDisposable)?.Dispose();
            }
        }
        public Exception ErrorCode
        {
            get
            {
                if (this.error is null)
                    return new Exception();
                else if (this.error is Exception ex)
                    return ex;
                else
                    return new Exception(this.error.ToString());
            }
        }

        void IAsyncAction.GetResults() => ExceptionDispatchInfo.Capture(ErrorCode).Throw();
        void IAsyncActionWithProgress<TProgress>.GetResults() => ExceptionDispatchInfo.Capture(ErrorCode).Throw();
        T IAsyncOperation<T>.GetResults() { ExceptionDispatchInfo.Capture(ErrorCode).Throw(); return default; }
        T IAsyncOperationWithProgress<T, TProgress>.GetResults() { ExceptionDispatchInfo.Capture(ErrorCode).Throw(); return default; }

        AsyncActionCompletedHandler IAsyncAction.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Error); }
        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Error); }
        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Error); }
        AsyncOperationWithProgressCompletedHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Error); }

        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress { get => null; set { } }
        AsyncOperationProgressHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Progress { get => null; set { } }
    }
}
