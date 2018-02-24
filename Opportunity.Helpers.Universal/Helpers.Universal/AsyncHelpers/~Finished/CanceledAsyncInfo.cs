using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CanceledAsyncInfo<T, TProgress>
        : IAsyncAction, IAsyncActionWithProgress<TProgress>, IAsyncOperation<T>, IAsyncOperationWithProgress<T, TProgress>
    {
        public static CanceledAsyncInfo<T, TProgress> Instanse { get; } = new CanceledAsyncInfo<T, TProgress>();

        private CanceledAsyncInfo() { }

        uint IAsyncInfo.Id => unchecked((uint)GetHashCode());
        AsyncStatus IAsyncInfo.Status => AsyncStatus.Canceled;
        void IAsyncInfo.Cancel() { }
        void IAsyncInfo.Close() { }
        Exception IAsyncInfo.ErrorCode => null;

        void IAsyncAction.GetResults() => throw new OperationCanceledException();
        void IAsyncActionWithProgress<TProgress>.GetResults() => throw new OperationCanceledException();
        T IAsyncOperation<T>.GetResults() => throw new OperationCanceledException();
        T IAsyncOperationWithProgress<T, TProgress>.GetResults() => throw new OperationCanceledException();

        AsyncActionCompletedHandler IAsyncAction.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Canceled); }
        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Canceled); }
        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Canceled); }
        AsyncOperationWithProgressCompletedHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Canceled); }

        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress { get => null; set { } }
        AsyncOperationProgressHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Progress { get => null; set { } }
    }
}
