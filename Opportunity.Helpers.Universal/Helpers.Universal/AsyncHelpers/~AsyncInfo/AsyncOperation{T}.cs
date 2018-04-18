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
        public static IAsyncOperation<T> CreateCompleted() => CompletedAsyncInfo<T, VoidProgress>.Instanse;
        public static IAsyncOperation<T> CreateCompleted(T results)
            => AsyncOperationCache<VoidProgress>.TryGetCacehd(results) ?? CompletedAsyncInfo<T, VoidProgress>.Create(results);
        public static IAsyncOperation<T> CreateFault() => FaultedAsyncInfo<T, VoidProgress>.Instanse;
        public static IAsyncOperation<T> CreateFault(Exception ex) => FaultedAsyncInfo<T, VoidProgress>.Create(ex);
        public static IAsyncOperation<T> CreateCanceled() => CanceledAsyncInfo<T, VoidProgress>.Instanse;

        private AsyncOperationCompletedHandler<T> completed;
        public AsyncOperationCompletedHandler<T> Completed
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
        }
    }
}
