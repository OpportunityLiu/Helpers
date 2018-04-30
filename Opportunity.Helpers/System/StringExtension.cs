using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Compute levenstein distance of two string, of <see cref="StringComparison.CurrentCulture"/>.
        /// </summary>
        /// <param name="source">1st string.</param>
        /// <param name="target">2nd string.</param>
        /// <returns>Levenstein distance of two string.</returns>
        public static int Distance(this string source, string target)
            => Distance(source, target, StringComparison.CurrentCulture);

        /// <summary>
        /// Compute levenstein distance of two string, of <paramref name="comparisonType"/>.
        /// </summary>
        /// <param name="source">1st string.</param>
        /// <param name="target">2nd string.</param>
        /// <param name="comparisonType">Comparsion type of char comparing.</param>
        /// <returns>Levenstein distance of two string.</returns>
        public static int Distance(this string source, string target, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target))
                    return 0;
                return target.Length;
            }
            else if (string.IsNullOrEmpty(target))
                return source.Length;

            if (source.Length > target.Length)
                return levenshteinCore(target, source, comparisonType);
            else
                return levenshteinCore(source, target, comparisonType);

        }

        private static int levenshteinCore(string sstr, string lstr, StringComparison comparisonType)
        {
            Debug.Assert(sstr.Length <= lstr.Length);
            var m = sstr.Length;
            var n = lstr.Length;
            var col1 = new int[m + 1];
            var col2 = new int[m + 1];

            // initialize col1
            for (var i = 0; i <= m; i++)
                col1[i] = i;
            for (var j = 1; j <= n; j++)
            {
                col2[0] = j;
                for (var i = 1; i <= m; i++)
                {
                    var equals = string.Compare(sstr, i - 1, lstr, j - 1, 1, comparisonType) == 0;
                    var min1 = col2[i - 1] + 1;
                    var min2 = col1[i] + 1;
                    var min3 = col1[i - 1] + (equals ? 0 : 1);
                    col2[i] = Math.Min(Math.Min(min1, min2), min3);
                }
                // swap cache
                var temp = col2;
                col2 = col1;
                col1 = temp;
            }
            return col1[m];
        }

        /// <summary>
        /// Call <see cref="string.IsNullOrEmpty(string)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        /// <summary>
        /// Call <see cref="string.IsNullOrWhiteSpace(string)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}
