using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncOperation<T> : AsyncInfoBase, IAsyncOperation<T>
    {
        public static AsyncOperation<T> CreateCompleted(T results)
        {
            var r = new AsyncOperation<T>();
            r.SetResults(results);
            return r;
        }

        public static AsyncOperation<T> CreateFault(Exception ex)
        {
            var r = new AsyncOperation<T>();
            r.SetException(ex);
            return r;
        }

        public static AsyncOperation<T> CreateCancelled()
        {
            var r = new AsyncOperation<T>();
            r.Cancel();
            return r;
        }

        private AsyncOperationCompletedHandler<T> completed;
        public AsyncOperationCompletedHandler<T> Completed
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
    }
}
