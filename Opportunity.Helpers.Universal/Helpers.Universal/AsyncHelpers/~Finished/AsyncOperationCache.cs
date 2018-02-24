using System;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal static class AsyncOperationCache<TProgress>
    {
        internal const int CACHE_START = -1;
        internal const int CACHE_END = 10;

        private static class Bool
        {
            public static readonly CompletedAsyncInfo<bool, TProgress> True = CreateCache(true);

            public static readonly CompletedAsyncInfo<bool, TProgress> False = CreateCache(false);
        }

        private static class Int32
        {
            public static readonly CompletedAsyncInfo<int, TProgress>[] Values = CreateRange<int>();
        }

        private static class Int64
        {
            public static readonly CompletedAsyncInfo<long, TProgress>[] Values = CreateRange<long>();
        }

        private static class String
        {
            public static readonly CompletedAsyncInfo<string, TProgress> Empty = CreateCache("");
        }

        private static CompletedAsyncInfo<T, TProgress> CreateCache<T>(T result)
        {
            if (Equals(result, default(T)))
                return CompletedAsyncInfo<T, TProgress>.Instanse;
            return CompletedAsyncInfo<T, TProgress>.Create(result);
        }

        private static CompletedAsyncInfo<T, TProgress>[] CreateRange<T>()
        {
            var resultType = typeof(T);
            var r = new CompletedAsyncInfo<T, TProgress>[CACHE_END - CACHE_START];
            for (var i = 0; i < r.Length; i++)
            {
                r[i] = CreateCache((T)Convert.ChangeType(CACHE_START + i, resultType));
            }
            return r;
        }

        public static CompletedAsyncInfo<T, TProgress> TryGetCacehd<T>(T results)
        {
            switch (results)
            {
            case bool b:
                if (b)
                    return (CompletedAsyncInfo<T, TProgress>)(object)Bool.True;
                else
                    return (CompletedAsyncInfo<T, TProgress>)(object)Bool.False;
            case int i32:
                if (i32 >= CACHE_START && i32 < CACHE_END)
                    return (CompletedAsyncInfo<T, TProgress>)(object)Int32.Values[i32 - CACHE_START];
                else
                    return null;
            case long i64:
                if (i64 >= CACHE_START && i64 < CACHE_END)
                    return (CompletedAsyncInfo<T, TProgress>)(object)Int64.Values[i64 - CACHE_START];
                else
                    return null;
            case string s:
                if (s.Length == 0)
                    return (CompletedAsyncInfo<T, TProgress>)(object)String.Empty;
                else
                    return null;
            default:
                return null;
            }
        }
    }
}
