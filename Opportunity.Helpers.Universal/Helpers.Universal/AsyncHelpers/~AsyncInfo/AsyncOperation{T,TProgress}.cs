using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Implemetation of <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
    /// </summary>
    public sealed class AsyncOperation<T, TProgress> : AsyncOperationBase<T>, IAsyncOperationWithProgress<T, TProgress>, IProgress<TProgress>
    {
        /// <summary>
        /// Create a complated <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
        /// </summary>
        /// <returns>A complated <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.</returns>
        public static IAsyncOperationWithProgress<T, TProgress> CreateCompleted() => CompletedAsyncInfo<T, TProgress>.Instanse;
        /// <summary>
        /// Create a complated <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
        /// </summary>
        /// <param name="results">Results of the operation.</param>
        /// <returns>A complated <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.</returns>
        public static IAsyncOperationWithProgress<T, TProgress> CreateCompleted(T results)
            => AsyncOperationCache<TProgress>.TryGetCacehd(results) ?? CompletedAsyncInfo<T, TProgress>.Create(results);
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
        /// </summary>
        /// <returns>A faulted ended <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.</returns>
        public static IAsyncOperationWithProgress<T, TProgress> CreateFault() => FaultedAsyncInfo<T, TProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
        /// </summary>
        /// <param name="ex">Fault of the operation.</param>
        /// <returns>A faulted ended <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.</returns>
        public static IAsyncOperationWithProgress<T, TProgress> CreateFault(Exception ex) => FaultedAsyncInfo<T, TProgress>.Create(ex);
        /// <summary>
        /// Create a canceled <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.
        /// </summary>
        /// <returns>A canceled <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/>.</returns>
        public static IAsyncOperationWithProgress<T, TProgress> CreateCanceled() => CanceledAsyncInfo<T, TProgress>.Instanse;

        private AsyncOperationWithProgressCompletedHandler<T, TProgress> completed;
        /// <summary>
        /// Notifier for completion.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Completed"/> has been set.</exception>
        public AsyncOperationWithProgressCompletedHandler<T, TProgress> Completed
        {
            get => this.completed;
            set
            {
                if (this.Status != AsyncStatus.Started)
                {
                    value?.Invoke(this, this.Status);
                    Interlocked.Exchange(ref this.completed, value);
                }
                else if (Interlocked.CompareExchange(ref this.completed, value, null) != null)
                    throw new InvalidOperationException("Completed has been set.");
            }
        }

        internal override void OnCompleted() => this.completed?.Invoke(this, this.Status);

        private AsyncOperationProgressHandler<T, TProgress> progress;
        /// <summary>
        /// Notifier for progress.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Progress"/> has been set.</exception>
        public AsyncOperationProgressHandler<T, TProgress> Progress
        {
            get => this.progress;
            set
            {
                if (Status != AsyncStatus.Started)
                    return;
                if (Interlocked.CompareExchange(ref this.progress, value, null) != null)
                    throw new InvalidOperationException("Progress has been set.");
            }
        }

        /// <summary>
        /// Report progress.
        /// </summary>
        /// <param name="progress">Progress of operation.</param>
        public void Report(TProgress progress)
        {
            if (Status != AsyncStatus.Started)
                return;
            this.progress?.Invoke(this, progress);
        }

        /// <summary>
        /// End the operation.
        /// </summary>
        public override void Close()
        {
            base.Close();
            this.completed = null;
            this.progress = null;
        }
    }
}
