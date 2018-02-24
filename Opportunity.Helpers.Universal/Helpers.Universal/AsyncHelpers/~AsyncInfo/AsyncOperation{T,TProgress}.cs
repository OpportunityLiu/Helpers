using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{

    public sealed class AsyncOperation<T, TProgress> : AsyncInfoBase, IAsyncOperationWithProgress<T, TProgress>, IProgress<TProgress>
    {
        public static IAsyncOperationWithProgress<T, TProgress> CreateCompleted() => CompletedAsyncInfo<T, TProgress>.Instanse;
        public static IAsyncOperationWithProgress<T, TProgress> CreateCompleted(T results)
            => AsyncOperationCache<TProgress>.TryGetCacehd(results) ?? CompletedAsyncInfo<T, TProgress>.Create(results);
        public static IAsyncOperationWithProgress<T, TProgress> CreateFault() => FaultedAsyncInfo<T, TProgress>.Instanse;
        public static IAsyncOperationWithProgress<T, TProgress> CreateFault(Exception ex) => FaultedAsyncInfo<T, TProgress>.Create(ex);
        public static IAsyncOperationWithProgress<T, TProgress> CreateCanceled() => CanceledAsyncInfo<T, TProgress>.Instanse;

        private AsyncOperationWithProgressCompletedHandler<T, TProgress> completed;
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
        private T results;
        public void SetResults(T results)
        {
            var c = this.completed;
            PreSetResults();
            this.results = results;
            c?.Invoke(this, this.Status);
        }

        public T GetResults()
        {
            PreGetResults();
            return this.results;
        }

        public void SetException(Exception ex)
        {
            var c = this.completed;
            PreSetException(ex);
            c?.Invoke(this, this.Status);
        }

        public override void Cancel()
        {
            var c = this.completed;
            if (PreCancel())
                c?.Invoke(this, this.Status);
        }

        public override void Close()
        {
            base.Close();
            (this.results as IDisposable)?.Dispose();
            this.results = default;
        }

        public void Report(TProgress value) => this.progress?.Invoke(this, value);

        private AsyncOperationProgressHandler<T, TProgress> progress;
        public AsyncOperationProgressHandler<T, TProgress> Progress
        {
            get => this.progress;
            set
            {
                if (Interlocked.CompareExchange(ref this.progress, value, null) != null)
                    throw new InvalidOperationException("Progress has been set.");
            }
        }
    }
}
