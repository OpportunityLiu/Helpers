using System;
using System.Collections.Generic;

namespace Opportunity.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<KeyValuePair<string, T>> GetDefinedValues<T>()
        where T : struct, IComparable, IFormattable, IConvertible
        {
            var names = EnumExtension.EnumExtentionCache<T>.Names;
            var values = EnumExtension.EnumExtentionCache<T>.Values;
            for (var i = 0; i < names.Length; i++)
            {
                yield return new KeyValuePair<string, T>(names[i], values[i]);
            }
        }

        public static Type GetUnderlyingType<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumExtension.EnumExtentionCache<T>.TUnderlyingType;
        }

        public static bool IsFlag<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumExtension.EnumExtentionCache<T>.IsFlag;
        }
    }
}
