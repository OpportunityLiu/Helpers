using System.Collections.Generic;

namespace System.Linq
{
    partial class EnumerableExtension
    {
        /// <summary>
        /// Get index of <paramref name="item"/> in <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Collection to search.</param>
        /// <param name="item">Item to search.</param>
        /// <returns>Index of <paramref name="item"/>, if find, otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <remarks><see cref="IList{T}.IndexOf(T)"/> is used by the method.</remarks>
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
        /// 
        /// <param name="arrayIndex">Start index of array to store data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Not enough space for copying.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <remarks><see cref="ICollection{T}.CopyTo(T[], int)"/> is used by the method.</remarks>
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
