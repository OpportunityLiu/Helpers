﻿namespace System.Collections.Generic
{
    public static class DictionaryExtention
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> @default)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (@default == null)
                throw new ArgumentNullException(nameof(@default));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return @default();
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue @default)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return @default;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValueOrDefault(key, default(TValue));
        }
    }
}
