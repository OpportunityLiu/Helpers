namespace System.Collections
{
    public static class DictionaryExtention
    {
        public static object GetValueOrDefault(this IDictionary dictionary, object key, Func<object> @default)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (@default == null)
                throw new ArgumentNullException(nameof(@default));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return @default();
        }

        public static object GetValueOrDefault(this IDictionary dictionary, object key, object @default)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.Contains(key))
                return dictionary[key];
            else
                return @default;
        }
    }
}
