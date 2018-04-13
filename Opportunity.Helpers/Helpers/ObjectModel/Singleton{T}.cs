using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple singlelon.
    /// </summary>
    public static class Singleton
    {
        private static class Storage<T>
            where T : class
        {
            public static T Value;
        }

        public static T Reset<T>()
            where T : class
            => Set<T>(null);

        public static T Set<T>(T value)
            where T : class
            => Interlocked.Exchange(ref Storage<T>.Value, value);

        public static T GetOrCreate<T>()
            where T : class, new()
        {
            if (Storage<T>.Value is null)
            {
                lock (typeof(Storage<T>))
                {
                    if (Storage<T>.Value is null)
                    {
                        Storage<T>.Value = new T();
                    }
                }
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
                lock (typeof(Storage<T>))
                {
                    if (Storage<T>.Value is null)
                    {
                        Storage<T>.Value = activator();
                    }
                }
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
                lock (typeof(Storage<T>))
                {
                    if (Storage<T>.Value is null)
                    {
                        Storage<T>.Value = createdValue;
                    }
                }
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
