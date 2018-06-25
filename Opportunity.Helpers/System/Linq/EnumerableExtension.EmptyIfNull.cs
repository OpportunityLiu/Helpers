using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq
{
    partial class EnumerableExtension
    {
        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> collection)
            => collection ?? Array.Empty<T>();

        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICollection<T> EmptyIfNull<T>(this ICollection<T> collection)
            => collection ?? Array.Empty<T>();

        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<T> EmptyIfNull<T>(this IList<T> collection)
            => collection ?? Array.Empty<T>();

        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable EmptyIfNull(this IEnumerable collection)
            => collection ?? Array.Empty<object>();

        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICollection EmptyIfNull(this ICollection collection)
            => collection ?? Array.Empty<object>();

        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or not.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns>An empty collcetion if the <paramref name="collection"/> is <see langword="null"/>, otherwise, the <paramref name="collection"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList EmptyIfNull(this IList collection)
            => collection ?? Array.Empty<object>();
    }
}
