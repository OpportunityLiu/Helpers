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
        /// Check the <paramref name="collection"/> is <see langword="null"/> or empty.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="collection"/> is <see langword="null"/> or empty.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this IEnumerable collection)
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
            if (collection is ICollection nCol)
                return nCol.Count <= 0;
            if (collection is IReadOnlyCollection<T> roCol)
                return roCol.Count <= 0;
            foreach (var item in collection)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check the <paramref name="collection"/> is empty.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="collection"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public static bool IsEmpty(this IEnumerable collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (collection is ICollection col)
                return col.Count <= 0;
            foreach (var item in collection)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get index of <paramref name="item"/> in <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Collection to search.</param>
        /// <param name="item">Item to search.</param>
        /// <returns>Index of <paramref name="item"/>, if find, otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (collection is IList<T> list)
                return list.IndexOf(item);
            var i = 0;
            foreach (var item2 in collection)
            {
                if (EqualityComparer<T>.Default.Equals(item, item2))
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Copy data from <paramref name="collection"/> to <paramref name="array"/> start from <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="collection">Collection of data to copy from.</param>
        /// <param name="array">Array to store data.</param>
        /// <param name="arrayIndex">Start index of array to store data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Not enough space for copying.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        public static void CopyTo<T>(this IEnumerable<T> collection, T[] array, int arrayIndex)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (collection is ICollection<T> col)
            {
                col.CopyTo(array, arrayIndex);
                return;
            }
            if (collection is IReadOnlyCollection<T> rcol && arrayIndex + rcol.Count > array.Length)
                throw new ArgumentException("Not enough space for copying.");
            try
            {
                foreach (var item in collection)
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ArgumentException("Not enough space for copying.", ex);
            }
        }
    }
}
