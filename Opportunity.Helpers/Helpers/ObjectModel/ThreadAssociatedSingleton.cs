﻿using System;
using System.Threading;

namespace Opportunity.Helpers.ObjectModel
{
    /// <summary>
    /// Simple singleton with <see cref="ThreadStaticAttribute"/> backing field.
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

        /// <summary>
        /// Count of created singleton of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Count of created singleton of <typeparamref name="T"/>.</returns>
        public static int Count<T>()
            where T : class
            => Storage<T>.Count;

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
        {
            var r = Interlocked.Exchange(ref Storage<T>.Value, value);

            if (value is null && r != null)
                Interlocked.Decrement(ref Storage<T>.Count);
            else if (value != null && r is null)
                Interlocked.Increment(ref Storage<T>.Count);

            return r;
        }

        /// <summary>
        /// Get singleton value of <typeparamref name="T"/>,
        /// if <see langword="null"/>, a new instance of <typeparamref name="T"/> will be created.
        /// </summary>
        /// <typeparam name="T">Type of singleton.</typeparam>
        /// <returns>Current value of singleton.</returns>
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
            if (Storage<T>.Value is null)
            {
                Storage<T>.Value = activator() ?? throw new ArgumentException($"Call of activator({activator}) returns null.");
                Interlocked.Increment(ref Storage<T>.Count);
            }
            return Storage<T>.Value;
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
            if (Storage<T>.Value is null)
            {
                Storage<T>.Value = createdValue;
                Interlocked.Increment(ref Storage<T>.Count);
            }
            return Storage<T>.Value;
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
