using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal abstract class AsyncOperationCache<TProgress> : AsyncOperationCache
    {
        public static new AsyncOperation<bool, TProgress> True = CreateCache(true);

        public static new AsyncOperation<bool, TProgress> False = CreateCache(false);

        public static new AsyncOperation<int, TProgress>[] Int32 = CreateRange<int>();

        public static new AsyncOperation<long, TProgress>[] Int64 = CreateRange<long>();

        private static AsyncOperation<T, TProgress> CreateCache<T>(T result)
        {
            var r = new AsyncOperation<T, TProgress>();
            r.SetResults(result);
            return r;
        }

        private static AsyncOperation<T, TProgress>[] CreateRange<T>()
        {
            var resultType = typeof(T);
            var r = new AsyncOperation<T, TProgress>[CACHE_END - CACHE_START];
            for (var i = 0; i < r.Length; i++)
            {
                r[i] = CreateCache((T)Convert.ChangeType(CACHE_START + i, resultType));
            }
            return r;
        }
    }
    public sealed class AsyncOperation<T, TProgress> : AsyncInfoBase, IAsyncOperationWithProgress<T, TProgress>, IProgress<TProgress>
    {
        private static class Cache
        {
            public static readonly AsyncOperation<T, TProgress> Completed = createCompleted();
            private static AsyncOperation<T, TProgress> createCompleted()
            {
                var r = new AsyncOperation<T, TProgress>();
                r.SetResults(default);
                return r;
            }

            public static readonly AsyncOperation<T, TProgress> Canceled = createCanceled();
            private static AsyncOperation<T, TProgress> createCanceled()
            {
                var r = new AsyncOperation<T, TProgress>();
                r.Cancel();
                return r;
            }
        }

        public static AsyncOperation<T, TProgress> CreateCompleted() => Cache.Completed;

        public static AsyncOperation<T, TProgress> CreateCompleted(T results)
        {
            if (results == null)
                return CreateCompleted();
            if (results is bool b)
            {
                if (b)
                    return (AsyncOperation<T, TProgress>)(object)AsyncOperationCache<TProgress>.True;
                else
                    return (AsyncOperation<T, TProgress>)(object)AsyncOperationCache<TProgress>.False;
            }
            else if (results is int i32)
            {
                if (i32 >= AsyncOperationCache.CACHE_START && i32 < AsyncOperationCache.CACHE_END)
                    return (AsyncOperation<T, TProgress>)(object)AsyncOperationCache<TProgress>.Int32[i32 - AsyncOperationCache.CACHE_START];
            }
            else if (results is long i64)
            {
                if (i64 >= AsyncOperationCache.CACHE_START && i64 < AsyncOperationCache.CACHE_END)
                    return (AsyncOperation<T, TProgress>)(object)AsyncOperationCache<TProgress>.Int64[i64 - AsyncOperationCache.CACHE_START];
            }
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

        public static AsyncOperation<T, TProgress> CreateCanceled() => Cache.Canceled;

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
