using System;
using System.Threading;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple singlelon with <see cref="ThreadStaticAttribute"/> backing field.
    /// </summary>
    public static class ThreadLocalSingleton
    {
        private static class Storage<T>
            where T : class
        {
            [ThreadStatic]
            public static T Value;
            public static int Count;
        }

        public static int Count<T>()
            where T : class
            => Storage<T>.Count;

        public static T Reset<T>()
            where T : class
            => Set<T>(null);

        public static T Set<T>(T value)
            where T : class
        {
            var r = Interlocked.Exchange(ref Storage<T>.Value, value);

            if (value is null && r != null)
                Interlocked.Decrement(ref Storage<T>.Count);
            else if (value != null && r is null)
                Interlocked.Increment(ref Storage<T>.Count);

            return r;
        }

        public static T GetOrCreate<T>()
            where T : class, new()
        {
            if (Storage<T>.Value is null)
            {
                Storage<T>.Value = new T();
                Interlocked.Increment(ref Storage<T>.Count);
            }
            return Storage<T>.Value;
        }

        public static T GetOrCreate<T>(Func<T> activator)
            where T : class
        {
            if (activator is null)
                throw new ArgumentNullException(nameof(activator));
            if (Storage<T>.Value is null)
            {
                var value = activator();
                if (value is null)
                    return null;
                Storage<T>.Value = value;
                Interlocked.Increment(ref Storage<T>.Count);
            }
            return Storage<T>.Value;
        }

        public static T GetOrCreate<T>(T createdValue)
            where T : class
        {
            if (createdValue is null)
                throw new ArgumentNullException(nameof(createdValue));
            if (Storage<T>.Value is null)
            {
                Storage<T>.Value = createdValue;
                Interlocked.Increment(ref Storage<T>.Count);
            }
            return Storage<T>.Value;
        }

        public static T Get<T>()
            where T : class
            => Storage<T>.Value;

        public static T GetOrDefault<T>(T defaultValue)
            where T : class
            => Storage<T>.Value ?? defaultValue;
    }
}
