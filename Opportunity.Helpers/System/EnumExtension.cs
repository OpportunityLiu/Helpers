using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/> types.
    /// </summary>
    public static class EnumExtension
    {
        internal static class EnumExtentionCache<T>
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            static EnumExtentionCache()
            {
                TType = typeof(T);
                TTypeCode = default(T).GetTypeCode();
                TUnderlyingType = Enum.GetUnderlyingType(TType);
                var info = TType.GetTypeInfo();
                IsFlag = info.GetCustomAttribute<FlagsAttribute>() != null;

                var names = Enum.GetNames(TType);
                var values = (T[])Enum.GetValues(TType);
                var count = names.Length;
                var query = from index in Enumerable.Range(0, count)
                            let r = new { Name = names[index], Value = values[index], UInt64Value = ToUInt64(values[index]) }
                            orderby r.UInt64Value
                            select r;
                Names = new string[count];
                Values = new T[count];
                UInt64Values = new ulong[count];
                var i = 0;
                foreach (var item in query)
                {
                    Names[i] = item.Name;
                    Values[i] = item.Value;
                    UInt64Values[i] = item.UInt64Value;
                    i++;
                }
            }

            public static readonly Type TType;
            public static readonly TypeCode TTypeCode;
            public static readonly Type TUnderlyingType;
            public static readonly bool IsFlag;

            public static readonly string[] Names;
            public static readonly T[] Values;
            public static readonly ulong[] UInt64Values;

            private const string enumSeperator = ", ";

            public static ulong ToUInt64(T value)
            {
                ulong result = 0;
                switch (TTypeCode)
                {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    result = (ulong)Convert.ToInt64(value, Globalization.CultureInfo.InvariantCulture);
                    break;

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Boolean:
                case TypeCode.Char:
                    result = Convert.ToUInt64(value, Globalization.CultureInfo.InvariantCulture);
                    break;
                }
                return result;
            }

            public static string ToFriendlyNameString(T that, Func<string, string> nameProvider)
            {
                return ToFriendlyNameString(that, i => nameProvider(Names[i]));
            }

            public static string ToFriendlyNameString(T that, Func<T, string> nameProvider)
            {
                return ToFriendlyNameString(that, i => nameProvider(Values[i]));
            }

            private static string ToFriendlyNameString(T that, Func<int, string> nameProvider)
            {
                var idx = GetIndex(that);
                if (idx >= 0)
                    return nameProvider(idx);
                else if (!IsFlag)
                    return that.ToString();
                else
                    return ToFriendlyNameStringForFlagsFormat(that, nameProvider);
            }

            public static int GetIndex(T that)
            {
                return Array.IndexOf(Values, that);
            }

            private static string ToFriendlyNameStringForFlagsFormat(T that, Func<int, string> nameProvider)
            {
                var result = ToUInt64(that);

                var index = Values.Length - 1;
                var retval = new StringBuilder();
                var firstTime = true;
                var saveResult = result;

                // We will not optimize this code further to keep it maintainable. There are some boundary checks that can be applied
                // to minimize the comparsions required. This code works the same for the best/worst case. In general the number of
                // items in an enum are sufficiently small and not worth the optimization.
                while (index >= 0)
                {
                    if ((index == 0) && (UInt64Values[index] == 0))
                        break;

                    if ((result & UInt64Values[index]) == UInt64Values[index])
                    {
                        result -= UInt64Values[index];
                        if (!firstTime)
                            retval.Insert(0, enumSeperator);

                        retval.Insert(0, nameProvider(index));
                        firstTime = false;
                    }

                    index--;
                }

                // We were unable to represent this number as a bitwise or of valid flags
                if (result != 0)
                    return that.ToString();

                // For the case when we have zero
                if (saveResult == 0)
                {
                    if (Values.Length > 0 && UInt64Values[0] == 0)
                        return nameProvider(0); // Zero was one of the enum values.
                    else
                        return "0";
                }
                else
                    return retval.ToString(); // Return the string representation
            }
        }

        /// <summary>
        /// Get defined values of the enum.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>Defined values of the enum.</returns>
        public static IEnumerable<KeyValuePair<string, T>> GetDefinedValues<T>()
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            var names = EnumExtentionCache<T>.Names;
            var values = EnumExtentionCache<T>.Values;
            for (var i = 0; i < names.Length; i++)
            {
                yield return new KeyValuePair<string, T>(names[i], values[i]);
            }
        }

        /// <summary>
        /// Get underlying type of the enum.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>Underlying type of the enum.</returns>
        public static Type GetUnderlyingType<T>()
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            return EnumExtentionCache<T>.TUnderlyingType;
        }

        /// <summary>
        /// Check whether the enum is flag.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns><see langword="true"/> if is flag.</returns>
        public static bool IsFlag<T>()
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            return EnumExtentionCache<T>.IsFlag;
        }

        /// <summary>
        /// Convert an enum value to its <see cref="ulong"/> equivalent.
        /// </summary>
        /// <param name="that">Value to convert.</param>
        /// <returns><see cref="ulong"/> equivalent of <paramref name="that"/>.</returns>
        public static ulong ToUInt64<T>(this T that)
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            switch (that.GetTypeCode())
            {
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return unchecked((ulong)that.ToInt64(Globalization.CultureInfo.InvariantCulture));

            case TypeCode.Byte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Boolean:
            case TypeCode.Char:
                return that.ToUInt64(Globalization.CultureInfo.InvariantCulture);
            }
            throw new ArgumentException("Can't convert.");
        }

        /// <summary>
        /// Convert an enum value to its <see cref="ulong"/> equivalent.
        /// </summary>
        /// <param name="that">Value to convert.</param>
        /// <returns><see cref="ulong"/> equivalent of <paramref name="that"/>.</returns>
        public static ulong ToUInt64(this Enum that)
        {
            if (that == null)
                throw new ArgumentNullException(nameof(that));
            var c = (IConvertible)that;
            switch (c.GetTypeCode())
            {
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return unchecked((ulong)c.ToInt64(Globalization.CultureInfo.InvariantCulture));

            case TypeCode.Byte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Boolean:
            case TypeCode.Char:
                return c.ToUInt64(Globalization.CultureInfo.InvariantCulture);
            }
            throw new ArgumentException("Can't convert.");
        }

        /// <summary>
        /// Convert an <see cref="ulong"/> value to its <typeparamref name="T"/> equivalent.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="that">Value to convert.</param>
        /// <returns><typeparamref name="T"/> equivalent of <paramref name="that"/>.</returns>
        public static T ToEnum<T>(this ulong that)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), that);
        }

        /// <summary>
        /// Get string representaion of enum value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="that">Enum value.</param>
        /// <param name="nameProvider">Name provider provides names of defined enum values.</param>
        /// <returns>String representaion of enum value.</returns>
        public static string ToFriendlyNameString<T>(this T that, Func<T, string> nameProvider)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.ToFriendlyNameString(that, nameProvider);
        }

        /// <summary>
        /// Get string representaion of enum value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="that">Enum value.</param>
        /// <param name="nameProvider">Name provider provides names of defined enum values.</param>
        /// <returns>String representaion of enum value.</returns>
        public static string ToFriendlyNameString<T>(this T that, Func<string, string> nameProvider)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.ToFriendlyNameString(that, nameProvider);
        }

        /// <summary>
        /// Check whether the enum is defined.
        /// </summary>
        /// <param name="that">Enum value to check.</param>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns><see langword="true"/> if is defined.</returns>
        public static bool IsDefined<T>(this T that)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.GetIndex(that) >= 0;
        }
    }
}
