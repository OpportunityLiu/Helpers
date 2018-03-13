using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Helper methods for casting.
    /// </summary>
    public static class CastExtension
    {
        /// <summary>
        /// Cast value to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="obj">Value to cast.</param>
        /// <returns>Casted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cast<T>(this object obj) => (T)obj;

        /// <summary>
        /// Try cast value to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="obj">Value to cast.</param>
        /// <param name="defaultValue">Value for failed casting.</param>
        /// <returns>Casted value, or <paramref name="defaultValue"/>, if casting failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryCast<T>(this object obj, T defaultValue)
        {
            if (obj is T v)
                return v;
            return defaultValue;
        }

        /// <summary>
        /// Try cast value to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="obj">Value to cast.</param>
        /// <returns>Casted value, or default value of <typeparamref name="T"/>, if casting failed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryCast<T>(this object obj) => TryCast(obj, default(T));
    }
}
