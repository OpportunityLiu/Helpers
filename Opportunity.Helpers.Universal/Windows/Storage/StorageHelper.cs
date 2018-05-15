using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Foundation;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace Windows.Storage
{
    /// <summary>
    /// Extension methods for storage.
    /// </summary>
    public static class StorageExtension
    {
        /// <summary>
        /// Convert <see cref="IBuffer"/> to <see cref="StorageFile"/>.
        /// </summary>
        /// <param name="buffer">Data of file.</param>
        /// <param name="fileName">Display name with extention.</param>
        /// <param name="thumbnail">Thumbnail for created file.</param>
        /// <returns>Streamed file.</returns>
        public static IAsyncOperation<StorageFile> AsStroageFileAsync(this IBuffer buffer, string fileName, IRandomAccessStreamReference thumbnail)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (fileName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(fileName));

            return StorageFile.CreateStreamedFileAsync(fileName, dataRequested, thumbnail);

            async void dataRequested(StreamedFileDataRequest stream)
            {
                try
                {
                    await stream.WriteAsync(buffer);
                    await stream.FlushAsync();
                    stream.Dispose();
                }
                catch
                {
                    stream.FailAndClose(StreamedFileFailureMode.Failed);
                    throw;
                }
            }
        }

        /// <summary>
        /// Convert <see cref="IBuffer"/> to <see cref="StorageFile"/>.
        /// </summary>
        /// <param name="buffer">Data of file.</param>
        /// <param name="fileName">Display name with extention.</param>
        /// <returns>Streamed file.</returns>
        public static IAsyncOperation<StorageFile> AsStroageFileAsync(this IBuffer buffer, string fileName)
            => AsStroageFileAsync(buffer, fileName, null);

        /// <summary>
        /// Try get file with given <paramref name="name"/> in <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">Folder of file.</param>
        /// <param name="name">Name of file.</param>
        /// <returns>The file, or <see langword="null"/>, if not found.</returns>
        public static IAsyncOperation<StorageFile> TryGetFileAsync(this StorageFolder folder, string name)
        {
            return Run(async token => await folder.TryGetItemAsync(name) as StorageFile);
        }

        /// <summary>
        /// Try get folder with given <paramref name="name"/> in <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">Folder of folder.</param>
        /// <param name="name">Name of folder.</param>
        /// <returns>The folder, or <see langword="null"/>, if not found.</returns>
        public static IAsyncOperation<StorageFolder> TryGetFolderAsync(this StorageFolder folder, string name)
        {
            return Run(async token => await folder.TryGetItemAsync(name) as StorageFolder);
        }

        /// <summary>
        /// Store data to file.
        /// </summary>
        /// <param name="folder">Folder of file.</param>
        /// <param name="fileName">Name of file.</param>
        /// <param name="options"><see cref="CreationCollisionOption"/> for the operation.</param>
        /// <param name="buffer">Data to store.</param>
        /// <returns>The stored file.</returns>
        public static IAsyncOperation<StorageFile> SaveFileAsync(this StorageFolder folder, string fileName, CreationCollisionOption options, IBuffer buffer)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            return Run(async token =>
            {
                fileName = StorageHelper.ToValidFileName(fileName);
                var file = await folder.CreateFileAsync(fileName, options);
                await FileIO.WriteBufferAsync(file, buffer);
                return file;
            });
        }

    }

    /// <summary>
    /// Helper class for storage.
    /// </summary>
    public static class StorageHelper
    {
        private readonly static StorageFolder temp = ApplicationData.Current.TemporaryFolder;

        /// <summary>
        /// Get icon of specific file extension.
        /// </summary>
        /// <param name="extension">Extention of file.</param>
        /// <returns>Icon of specific file extension.</returns>
        public static IAsyncOperation<StorageItemThumbnail> GetIconOfExtensionAsync(string extension)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));
            var filename = default(string);
            if (string.IsNullOrWhiteSpace(extension))
                filename = $"Dummy{"".GetHashCode()}";
            else
            {
                var ext = extension.Trim().TrimStart('.');
                filename = $"Dummy{ext.GetHashCode()}.{ext}";
            }
            return Run(async token =>
            {
                var dummy = await temp.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                return await dummy.GetThumbnailAsync(ThumbnailMode.SingleItem);
            });
        }

        /// <summary>
        /// Create a temp folder with name of timestamp in <see cref="ApplicationData.TemporaryFolder"/>.
        /// </summary>
        public static IAsyncOperation<StorageFolder> CreateTempFolderAsync()
        {
            return temp.CreateFolderAsync(ToValidFileName(null));
        }

        private static readonly Dictionary<char, char> alternateFolderChars = new Dictionary<char, char>()
        {
            ['?'] = '？',
            ['\\'] = '＼',
            ['/'] = '／',
            ['"'] = '＂',
            ['|'] = '｜',
            ['*'] = '＊',
            ['<'] = '＜',
            ['>'] = '＞',
            [':'] = '：'
        };

        private static readonly char[] invalidChars = Path.GetInvalidFileNameChars();

        /// <summary>
        /// Build a vaild name for file from <paramref name="raw"/>.
        /// </summary>
        /// <param name="raw">Original file name for reference.</param>
        /// <returns>A vaild name for file.</returns>
        public static string ToValidFileName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return DateTimeOffset.Now.Ticks.ToString();
            if (raw.IndexOfAny(invalidChars) == -1)
                return raw;
            var sb = new StringBuilder(raw);
            foreach (var item in alternateFolderChars)
            {
                sb.Replace(item.Key, item.Value);
            }
            foreach (var item in invalidChars)
            {
                sb.Replace(item.ToString(), "");
            }
            var final = sb.ToString().Trim();
            if (string.IsNullOrEmpty(final))
                return DateTimeOffset.Now.Ticks.ToString();
            return final;
        }
    }
}
