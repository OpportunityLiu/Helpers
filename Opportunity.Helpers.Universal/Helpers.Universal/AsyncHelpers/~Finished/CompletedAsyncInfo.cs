using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CompletedAsyncInfo<T, TProgress>
        : IAsyncAction, IAsyncActionWithProgress<TProgress>, IAsyncOperation<T>, IAsyncOperationWithProgress<T, TProgress>
    {
        public static CompletedAsyncInfo<T, TProgress> Instanse { get; } = new CompletedAsyncInfo<T, TProgress>(default);
        public static CompletedAsyncInfo<T, TProgress> Create(T results)
            => results == null ? Instanse : new CompletedAsyncInfo<T, TProgress>(results);

        private readonly T results;

        private CompletedAsyncInfo(T results) => this.results = results;

        uint IAsyncInfo.Id => unchecked((uint)GetHashCode());
        AsyncStatus IAsyncInfo.Status => AsyncStatus.Completed;
        void IAsyncInfo.Cancel() { }
        void IAsyncInfo.Close() { }
        Exception IAsyncInfo.ErrorCode => null;

        void IAsyncAction.GetResults() { }
        void IAsyncActionWithProgress<TProgress>.GetResults() { }
        T IAsyncOperation<T>.GetResults() => this.results;
        T IAsyncOperationWithProgress<T, TProgress>.GetResults() => this.results;

        AsyncActionCompletedHandler IAsyncAction.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Completed); }
        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Completed); }
        AsyncOperationCompletedHandler<T> IAsyncOperation<T>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Completed); }
        AsyncOperationWithProgressCompletedHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Completed { get => null; set => value?.Invoke(this, AsyncStatus.Completed); }

        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress { get => null; set { } }
        AsyncOperationProgressHandler<T, TProgress> IAsyncOperationWithProgress<T, TProgress>.Progress { get => null; set { } }
    }
}
