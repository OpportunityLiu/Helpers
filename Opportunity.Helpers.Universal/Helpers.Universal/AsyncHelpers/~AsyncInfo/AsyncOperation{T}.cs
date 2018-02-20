using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
    }
}
