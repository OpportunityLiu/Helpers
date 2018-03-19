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
        public static DataPackage ToDataPackage(this string str)
        {
            if (str is null)
                throw new ArgumentNullException(nameof(str));
            var dp = create();
            dp.SetText(str);
            return dp;
        }
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
        public static DataPackage ToDataPackage(this IStorageItem storageItem)
        {
            if (storageItem is null)
                throw new ArgumentNullException(nameof(storageItem));
            return ToDataPackage(new[] { storageItem });
        }
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
