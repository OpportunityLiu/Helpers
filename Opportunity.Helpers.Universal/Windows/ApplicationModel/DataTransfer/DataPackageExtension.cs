using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Windows.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Extension methods for <see cref="DataPackage"/>.
    /// </summary>
    public static class DataPackageExtension
    {
        /// <summary>
        /// Create a <see cref="DataPackage"/> with given <paramref name="str"/>.
        /// </summary>
        /// <param name="str"><see cref="string"/> in the <see cref="DataPackage"/>.</param>
        /// <returns>A <see cref="DataPackage"/> with given <paramref name="str"/>.</returns>
        public static DataPackage ToDataPackage(this string str)
        {
            str = str ?? "";
            var dp = create();
            dp.SetText(str);
            return dp;
        }
        /// <summary>
        /// Create a <see cref="DataPackage"/> with given <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> in the <see cref="DataPackage"/>.</param>
        /// <returns>A <see cref="DataPackage"/> with given <paramref name="uri"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <see langword="null"/>.</exception>
        public static DataPackage ToDataPackage(this Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));
            var dp = create();
            dp.SetText(uri.ToString());
            if (uri.Scheme == "http" || uri.Scheme == "https")
                dp.SetWebLink(uri);
            else
                dp.SetApplicationLink(uri);
            return dp;
        }
        /// <summary>
        /// Create a <see cref="DataPackage"/> with given <paramref name="storageItem"/>.
        /// </summary>
        /// <param name="storageItem"><see cref="IStorageItem"/> in the <see cref="DataPackage"/>.</param>
        /// <returns>A <see cref="DataPackage"/> with given <paramref name="storageItem"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageItem"/> is <see langword="null"/>.</exception>
        public static DataPackage ToDataPackage(this IStorageItem storageItem)
        {
            if (storageItem is null)
                throw new ArgumentNullException(nameof(storageItem));
            return ToDataPackage(new[] { storageItem });
        }
        /// <summary>
        /// Create a <see cref="DataPackage"/> with given <paramref name="storageItems"/>.
        /// </summary>
        /// <param name="storageItems"><see cref="IStorageItem"/> in the <see cref="DataPackage"/>.</param>
        /// <returns>A <see cref="DataPackage"/> with given <paramref name="storageItems"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageItems"/> is <see langword="null"/>.</exception>
        public static DataPackage ToDataPackage(this IEnumerable<IStorageItem> storageItems)
        {
            if (storageItems is null)
                throw new ArgumentNullException(nameof(storageItems));
            var dp = create();
            foreach (var item in storageItems.OfType<IStorageFile>().Select(f => f.FileType).Distinct())
            {
                if (item != null)
                    dp.Properties.FileTypes.Add(item);
            }
            dp.SetStorageItems(storageItems);
            return dp;
        }
        /// <summary>
        /// Create a <see cref="DataPackage"/> with given <paramref name="bitmap"/>.
        /// </summary>
        /// <param name="bitmap"><see cref="RandomAccessStreamReference"/> representation of bitmap in the <see cref="DataPackage"/>.</param>
        /// <returns>A <see cref="DataPackage"/> with given <paramref name="bitmap"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bitmap"/> is <see langword="null"/>.</exception>
        public static DataPackage ToDataPackage(this RandomAccessStreamReference bitmap)
        {
            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));
            var dp = create();
            dp.SetBitmap(bitmap);
            return dp;
        }

        private static DataPackage create()
        {
            return new DataPackage
            {
                Properties =
                {
                    Title = Package.Current.DisplayName,
                    ApplicationName = Package.Current.DisplayName,
                    PackageFamilyName = Package.Current.Id.FamilyName,
                },
                RequestedOperation = DataPackageOperation.Copy,
            };
        }
    }
}
