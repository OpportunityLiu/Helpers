using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Implemetation of <see cref="IAsyncActionWithProgress{TProgress}"/>.
    /// </summary>
    public sealed class AsyncAction<TProgress> : AsyncActionBase, IAsyncActionWithProgress<TProgress>, IProgress<TProgress>
    {
        /// <summary>
        /// Create a complated <see cref="IAsyncActionWithProgress{TProgress}"/>.
        /// </summary>
        /// <returns>A complated <see cref="IAsyncActionWithProgress{TProgress}"/>.</returns>
        public static IAsyncActionWithProgress<TProgress> CreateCompleted() => CompletedAsyncInfo<VoidResult, TProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncActionWithProgress{TProgress}"/>.
        /// </summary>
        /// <returns>A faulted ended <see cref="IAsyncActionWithProgress{TProgress}"/>.</returns>
        public static IAsyncActionWithProgress<TProgress> CreateFault() => FaultedAsyncInfo<VoidResult, TProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncActionWithProgress{TProgress}"/>.
        /// </summary>
        /// <param name="ex">Fault of the action.</param>
        /// <returns>A faulted ended <see cref="IAsyncActionWithProgress{TProgress}"/>.</returns>
        public static IAsyncActionWithProgress<TProgress> CreateFault(Exception ex) => FaultedAsyncInfo<VoidResult, TProgress>.Create(ex);
        /// <summary>
        /// Create a canceled <see cref="IAsyncActionWithProgress{TProgress}"/>.
        /// </summary>
        /// <returns>A canceled <see cref="IAsyncActionWithProgress{TProgress}"/>.</returns>
        public static IAsyncActionWithProgress<TProgress> CreateCanceled() => CanceledAsyncInfo<VoidResult, TProgress>.Instanse;

        private AsyncActionWithProgressCompletedHandler<TProgress> completed;
        /// <summary>
        /// Notifier for completion.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Completed"/> has been set.</exception>
        public AsyncActionWithProgressCompletedHandler<TProgress> Completed
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

        private AsyncActionProgressHandler<TProgress> progress;
        /// <summary>
        /// Notifier for progress.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Progress"/> has been set.</exception>
        public AsyncActionProgressHandler<TProgress> Progress
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
        /// <param name="progress">Progress of action.</param>
        public void Report(TProgress progress)
        {
            if (Status != AsyncStatus.Started)
                return;
            this.progress?.Invoke(this, progress);
        }

        /// <summary>
        /// End the action.
        /// </summary>
        public override void Close()
        {
            base.Close();
            this.progress = null;
            this.completed = null;
        }
    }
}
