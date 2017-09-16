﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System
{
    public static class EnumExtension
    {
        private static class EnumExtentionCache<T>
            where T : struct, IComparable, IFormattable, IConvertible
        {
            static EnumExtentionCache()
            {
                TType = typeof(T);
                TTypeCode = Convert.GetTypeCode(default(T));
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
                return Array.BinarySearch(Values, that);
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

        public static ulong ToUInt64(this Enum that)
        {
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

        public static T ToEnum<T>(this ulong that)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), that);
        }

        public static string ToFriendlyNameString<T>(this T that, Func<T, string> nameProvider)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.ToFriendlyNameString(that, nameProvider);
        }

        public static string ToFriendlyNameString<T>(this T that, Func<string, string> nameProvider)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.ToFriendlyNameString(that, nameProvider);
        }

        public static bool IsDefined<T>(this T that)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumExtentionCache<T>.GetIndex(that) >= 0;
        }

        public static IEnumerable<KeyValuePair<string, T>> GetDefinedValues<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            var names = EnumExtentionCache<T>.Names;
            var values = EnumExtentionCache<T>.Values;
            for (var i = 0; i < names.Length; i++)
            {
                yield return new KeyValuePair<string, T>(names[i], values[i]);
            }
        }
    }
}