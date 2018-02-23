using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal abstract class AsyncOperationCache
    {
        public const int CACHE_START = -1;
        public const int CACHE_END = 10;

        public static AsyncOperation<bool> True = CreateCache(true);

        public static AsyncOperation<bool> False = CreateCache(false);

        public static AsyncOperation<int>[] Int32 = CreateRange<int>();

        public static AsyncOperation<long>[] Int64 = CreateRange<long>();

        private static AsyncOperation<T> CreateCache<T>(T result)
        {
            var r = new AsyncOperation<T>();
            r.SetResults(result);
            return r;
        }

        private static AsyncOperation<T>[] CreateRange<T>()
        {
            var resultType = typeof(T);
            var r = new AsyncOperation<T>[CACHE_END - CACHE_START];
            for (var i = 0; i < r.Length; i++)
            {
                r[i] = CreateCache((T)Convert.ChangeType(CACHE_START + i, resultType));
            }
            return r;
        }
    }

    public sealed class AsyncOperation<T> : AsyncInfoBase, IAsyncOperation<T>
    {
        private static class Cache
        {
            public static readonly AsyncOperation<T> Completed = createCompleted();
            private static AsyncOperation<T> createCompleted()
            {
                var r = new AsyncOperation<T>();
                r.SetResults(default);
                return r;
            }

            public static readonly AsyncOperation<T> Canceled = createCanceled();
            private static AsyncOperation<T> createCanceled()
            {
                var r = new AsyncOperation<T>();
                r.Cancel();
                return r;
            }
        }

        public static AsyncOperation<T> CreateCompleted() => Cache.Completed;

        public static AsyncOperation<T> CreateCompleted(T results)
        {
            if (results == null)
                return CreateCompleted();
            if (results is bool b)
            {
                if (b)
                    return (AsyncOperation<T>)(object)AsyncOperationCache.True;
                else
                    return (AsyncOperation<T>)(object)AsyncOperationCache.False;
            }
            else if (results is int i32)
            {
                if (i32 >= AsyncOperationCache.CACHE_START && i32 < AsyncOperationCache.CACHE_END)
                    return (AsyncOperation<T>)(object)AsyncOperationCache.Int32[i32 - AsyncOperationCache.CACHE_START];
            }
            else if (results is long i64)
            {
                if (i64 >= AsyncOperationCache.CACHE_START && i64 < AsyncOperationCache.CACHE_END)
                    return (AsyncOperation<T>)(object)AsyncOperationCache.Int64[i64 - AsyncOperationCache.CACHE_START];
            }
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

        public static AsyncOperation<T> CreateCanceled() => Cache.Canceled;

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
