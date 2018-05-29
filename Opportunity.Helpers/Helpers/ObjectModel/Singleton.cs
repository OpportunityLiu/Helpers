using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple singleton.
    /// </summary>
    public static class Singleton
    {
        private static class Storage<T>
            where T : class
        {
            public static T Value;
        }

        /// <summary>
        /// Set singleton value of <typeparamref name="T"/> to <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Original value of singleton.</returns>
        public static T Reset<T>()
            where T : class
            => Set<T>(null);

        /// <summary>
        /// Set singleton value of <typeparamref name="T"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">New value of singleton.</param>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Original value of singleton.</returns>
        public static T Set<T>(T value)
            where T : class
            => Interlocked.Exchange(ref Storage<T>.Value, value);

        private static T getOrCreate<T>(Func<T> activator)
            where T : class
        {
            var v = Storage<T>.Value;
            if (v != null)
                return v;
            v = activator() ?? throw new ArgumentException($"Call of activator({activator}) returns null.");
            var r = Interlocked.CompareExchange(ref Storage<T>.Value, v, null);
            if (r != null)
                return r;
            return v;
        }

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>,
        /// if <see langword="null"/>, a new instance of <typeparamref name="T"/> will be created.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Current value of singleton.</returns>
        public static T GetOrCreate<T>()
            where T : class, new()
            => getOrCreate(() => new T());

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>,
        /// if <see langword="null"/>, a new instance of <typeparamref name="T"/> will be created.
        /// </summary>
        /// <param name="activator">Delegate to create new instance of <typeparamref name="T"/>.</param>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Current value of singleton.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="activator"/> is <see langword="null"/>.</exception>
        /// <exception cref=" ArgumentException"><paramref name="activator"/> returns <see langword="null"/>.</exception>
        public static T GetOrCreate<T>(Func<T> activator)
            where T : class
        {
            if (activator is null)
                throw new ArgumentNullException(nameof(activator));
            return getOrCreate(activator);
        }

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>,
        /// if <see langword="null"/>, <paramref name="createdValue"/> will be set and returned.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <param name="createdValue">Created value of singleton.</param>
        /// <returns>Current value of singleton.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="createdValue"/> is <see langword="null"/>.</exception>
        public static T GetOrCreate<T>(T createdValue)
            where T : class
        {
            if (createdValue is null)
                throw new ArgumentNullException(nameof(createdValue));
            return getOrCreate(() => createdValue);
        }

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>。
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Current value of singleton.</returns>
        public static T Get<T>()
            where T : class
            => Storage<T>.Value;

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>,
        /// if <see langword="null"/>, <paramref name="defaultValue"/> will be returned.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <param name="defaultValue">Default value of singleton.</param>
        /// <returns>Current value of singleton.</returns>
        public static T GetOrDefault<T>(T defaultValue)
            where T : class
            => Storage<T>.Value ?? defaultValue;
    }
}
