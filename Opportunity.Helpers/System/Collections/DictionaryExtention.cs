namespace System.Collections
{
    public static class DictionaryExtention
    {
        public static object GetOrCreateValue(this IDictionary dictionary, object key, Func<object> valueCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (valueCreator == null)
                throw new ArgumentNullException(nameof(valueCreator));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return dictionary[key] = valueCreator();
        }

        public static object GetOrCreateValue(this IDictionary dictionary, object key, Func<object, object> valueCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (valueCreator == null)
                throw new ArgumentNullException(nameof(valueCreator));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return dictionary[key] = valueCreator(key);
        }

        public static object GetValueOrDefault(this IDictionary dictionary, object key, Func<object> defaultCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (defaultCreator == null)
                throw new ArgumentNullException(nameof(defaultCreator));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return defaultCreator();
        }

        public static object GetValueOrDefault(this IDictionary dictionary, object key, Func<object, object> defaultCreator)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (defaultCreator == null)
                throw new ArgumentNullException(nameof(defaultCreator));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return defaultCreator(key);
        }

        public static object GetValueOrDefault(this IDictionary dictionary, object key, object defaultValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return defaultValue;
        }
    }
}
