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

        internal override void OnCompleted() => this.completed?.Invoke(this, this.Status);

        private AsyncOperationProgressHandler<T, TProgress> progress;
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

        public void Report(TProgress progress)
        {
            if (Status != AsyncStatus.Started)
                return;
            this.progress?.Invoke(this, progress);
        }

        private T results;
        public bool TrySetResults(T results)
        {
            if (Status != AsyncStatus.Started)
                return false;

            var oldv = this.results;
            this.results = results;
            if (TrySetCompleted())
            {
                return true;
            }
            this.results = oldv;
            return false;
        }

        public T GetResults()
        {
            GetCompleted();
            return this.results;
        }

        public override void Close()
        {
            base.Close();
            (this.results as IDisposable)?.Dispose();
            this.results = default;
            this.completed = null;
            this.progress = null;
        }
    }
}
