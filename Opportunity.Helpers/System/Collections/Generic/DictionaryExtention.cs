namespace System.Collections.Generic
{
    public static class DictionaryExtention
    {
        public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (valueCreator == null)
                throw new ArgumentNullException(nameof(valueCreator));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return dictionary[key] = valueCreator();
        }

        public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (valueCreator == null)
                throw new ArgumentNullException(nameof(valueCreator));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return dictionary[key] = valueCreator(key);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (defaultCreator == null)
                throw new ArgumentNullException(nameof(defaultCreator));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return defaultCreator();
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defaultCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (defaultCreator == null)
                throw new ArgumentNullException(nameof(defaultCreator));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return defaultCreator(key);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out var va))
                return va;
            else
                return defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValueOrDefault(key, default(TValue));
        }
    }
}
