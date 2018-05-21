using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Add data to <see cref="Exception.Data"/>.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="ex"/>.</typeparam>
        /// <param name="ex"><see cref="Exception"/> to add data.</param>
        /// <param name="key">Key of adding data.</param>
        /// <param name="value">Value of adding data.</param>
        /// <returns><paramref name="ex"/> itself.</returns>
        public static T AddData<T>(this T ex, string key, object value)
            where T : Exception
        {
            if (ex is null)
                throw new ArgumentNullException(nameof(ex));
            ex.Data.Add(key, value);
            return ex;
        }
    }
}
