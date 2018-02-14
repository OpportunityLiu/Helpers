using System;
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
                if (this.completed != null)
                    throw new InvalidOperationException("Completed has been set.");
                this.completed = value;
            }
        }
        private T results;
        public void SetResults(T results)
        {
            PreSetResults();
            this.results = results;
            this.completed?.Invoke(this, this.Status);
        }

        public T GetResults()
        {
            PreGetResults();
            return this.results;
        }

        public override void SetException(Exception ex)
        {
            base.SetException(ex);
            this.completed?.Invoke(this, this.Status);
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
                if (this.progress != null)
                    throw new InvalidOperationException("Completed has been set.");
                this.progress = value;
            }
        }
    }
}
