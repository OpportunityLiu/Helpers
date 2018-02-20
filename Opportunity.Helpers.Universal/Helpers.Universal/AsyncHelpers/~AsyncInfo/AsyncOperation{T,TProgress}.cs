using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncOperation<T, TProgress> : AsyncInfoBase, IAsyncOperationWithProgress<T, TProgress>, IProgress<TProgress>
    {
        public static AsyncOperation<T, TProgress> CreateCompleted(T results)
        {
            var r = new AsyncOperation<T, TProgress>();
            r.SetResults(results);
            return r;
        }

        public static AsyncOperation<T, TProgress> CreateFault(Exception ex)
        {
            var r = new AsyncOperation<T, TProgress>();
            r.SetException(ex);
            return r;
        }

        public static AsyncOperation<T, TProgress> CreateCancelled()
        {
            var r = new AsyncOperation<T, TProgress>();
            r.Cancel();
            return r;
        }

        private AsyncOperationWithProgressCompletedHandler<T, TProgress> completed;
        public AsyncOperationWithProgressCompletedHandler<T, TProgress> Completed
        {
            get => this.completed;
            set
            {
                if (Interlocked.CompareExchange(ref this.completed, value, null) != null)
                    throw new InvalidOperationException("Completed has been set.");
                if (this.Status != AsyncStatus.Started)
                    value?.Invoke(this, this.Status);
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
