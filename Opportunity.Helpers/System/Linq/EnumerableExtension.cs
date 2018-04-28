using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Check the <paramref name="collection"/> is <see langword="null"/> or empty.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="collection"/> is <see langword="null"/> or empty.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection is null)
                return true;
            return IsEmpty(collection);
        }

        /// <summary>
        /// Check the <paramref name="collection"/> is empty.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="collection"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (collection is ICollection<T> col)
                return col.Count <= 0;
            if (collection is IReadOnlyCollection<T> roCol)
                return roCol.Count <= 0;
            if (collection is ICollection nCol)
                return nCol.Count <= 0;
            foreach (var item in collection)
            {
                return false;
            }
            return true;
        }
    }
}
