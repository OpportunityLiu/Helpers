using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/> types.
    /// </summary>
    public static class EnumExtension
    {
        internal interface IEnumExtentionCache
        {
            Type TType { get; }
            TypeCode TTypeCode { get; }
            Type TUnderlyingType { get; }
            bool IsFlag { get; }

            string[] Names { get; }
            Enum[] Values { get; }
            ulong[] UInt64Values { get; }

            int GetIndex(Enum that);
            string ToFriendlyNameString(Enum that, Func<string, string> nameProvider);
            string ToFriendlyNameString(Enum that, Func<Enum, string> nameProvider);
        }

        internal sealed class EnumExtentionCache<T> : IEnumExtentionCache
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            public static readonly EnumExtentionCache<T> Instance;
            static EnumExtentionCache()
            {
                Instance = new EnumExtentionCache<T>();
                EnumExtentionDic[typeof(T)] = Instance;
            }

            private EnumExtentionCache()
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

            public Type TType { get; }
            public TypeCode TTypeCode { get; }
            public Type TUnderlyingType { get; }
            public bool IsFlag { get; }

            public string[] Names { get; }
            public T[] Values { get; }
            public ulong[] UInt64Values { get; }

            private Enum[] boxedValues;
            Enum[] IEnumExtentionCache.Values => LazyInitializer.EnsureInitialized(ref this.boxedValues, () =>
            {
                var v = new Enum[Values.Length];
                for (var i = 0; i < Values.Length; i++)
                {
                    v[i] = Values[i];
                }
                return v;
            });

            private const string enumSeperator = ", ";

            public string ToFriendlyNameString(T that, Func<string, string> nameProvider)
            {
                return ToFriendlyNameString(that, i => nameProvider(Names[i]));
            }

            public string ToFriendlyNameString(T that, Func<T, string> nameProvider)
            {
                return ToFriendlyNameString(that, i => nameProvider(Values[i]));
            }

            public int GetIndex(T that)
            {
                return Array.IndexOf(Values, that);
            }

            private string ToFriendlyNameString(T that, Func<int, string> nameProvider)
            {
                var idx = GetIndex(that);
                if (idx >= 0)
                    return nameProvider(idx);
                else if (!IsFlag)
                    return that.ToString();
                else
                    return ToFriendlyNameStringForFlagsFormat(that, nameProvider);
            }

            private string ToFriendlyNameStringForFlagsFormat(T that, Func<int, string> nameProvider)
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

            int IEnumExtentionCache.GetIndex(Enum that)
                => GetIndex((T)that);
            string IEnumExtentionCache.ToFriendlyNameString(Enum that, Func<string, string> nameProvider)
                => ToFriendlyNameString((T)that, nameProvider);
            string IEnumExtentionCache.ToFriendlyNameString(Enum that, Func<Enum, string> nameProvider)
                => ToFriendlyNameString((T)that, v => nameProvider(v));
        }

        internal static readonly Dictionary<Type, IEnumExtentionCache> EnumExtentionDic = new Dictionary<Type, IEnumExtentionCache>();

        private static IEnumExtentionCache getForType(Type t)
        {
            if (!EnumExtentionDic.TryGetValue(t, out var value))
            {
                var cachet = typeof(EnumExtentionCache<>).MakeGenericType(t);
                RuntimeHelpers.RunClassConstructor(cachet.TypeHandle);
                value = EnumExtentionDic[t];
            }
            return value;
        }

        /// <summary>
        /// Get defined values of the enum.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>Defined values of the enum.</returns>
        public static IEnumerable<KeyValuePair<string, T>> GetDefinedValues<T>()
            where T : struct, Enum, IComparable, IConvertible, IFormattable
        {
            var names = EnumExtentionCache<T>.Instance.Names;
            var values = EnumExtentionCache<T>.Instance.Values;
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
            => EnumExtentionCache<T>.Instance.TUnderlyingType;

        /// <summary>
        /// Check whether the enum is flag.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns><see langword="true"/> if is flag.</returns>
        public static bool IsFlag<T>()
            where T : struct, Enum, IComparable, IConvertible, IFormattable
            => EnumExtentionCache<T>.Instance.IsFlag;

        /// <summary>
        /// Check whether the enum is flag.
        /// </summary>
        /// <param name="enumType">Type of enum.</param>
        /// <returns><see langword="true"/> if is flag.</returns>
        public static bool IsFlag(Type enumType)
        {
            if (enumType is null)
                throw new ArgumentNullException(nameof(enumType));
            try
            {
                var i = getForType(enumType);
                return i.IsFlag;
            }
            catch (Exception ex)
            {

                throw new ArgumentException("enumType is not a valid enum type.", ex);
            }
        }

        private static ulong toUInt64(Enum value)
        {
            var v = (IConvertible)value;
            switch (v.GetTypeCode())
            {
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return unchecked((ulong)v.ToInt64(Globalization.CultureInfo.InvariantCulture));

            case TypeCode.Byte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Boolean:
            case TypeCode.Char:
                return v.ToUInt64(Globalization.CultureInfo.InvariantCulture);
            }
            throw new ArgumentException("Can't convert.");
        }

        /// <summary>
        /// Convert an enum value to its <see cref="ulong"/> equivalent.
        /// </summary>
        /// <param name="that">Value to convert.</param>
        /// <returns><see cref="ulong"/> equivalent of <paramref name="that"/>.</returns>
        public static ulong ToUInt64<T>(this T that)
            where T : struct, Enum, IComparable, IConvertible, IFormattable
            => toUInt64(that);

        /// <summary>
        /// Convert an enum value to its <see cref="ulong"/> equivalent.
        /// </summary>
        /// <param name="that">Value to convert.</param>
        /// <returns><see cref="ulong"/> equivalent of <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="that"/> is <see langword="null"/>.</exception>
        public static ulong ToUInt64(this Enum that)
        {
            if (that is null)
                throw new ArgumentNullException(nameof(that));
            return toUInt64(that);
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
        /// <exception cref="ArgumentNullException"><paramref name="nameProvider"/> is <see langword="null"/>.</exception>
        public static string ToFriendlyNameString<T>(this T that, Func<T, string> nameProvider)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            if (nameProvider is null)
                throw new ArgumentNullException(nameof(nameProvider));
            return EnumExtentionCache<T>.Instance.ToFriendlyNameString(that, nameProvider);
        }

        /// <summary>
        /// Get string representaion of enum value.
        /// </summary>
        /// <param name="that">Enum value.</param>
        /// <param name="nameProvider">Name provider provides names of defined enum values.</param>
        /// <returns>String representaion of enum value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="that"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nameProvider"/> is <see langword="null"/>.</exception>
        public static string ToFriendlyNameString(this Enum that, Func<Enum, string> nameProvider)
        {
            if (nameProvider is null)
                throw new ArgumentNullException(nameof(nameProvider));
            if (that is null)
                throw new ArgumentNullException(nameof(that));
            return getForType(that.GetType()).ToFriendlyNameString(that, nameProvider);
        }

        /// <summary>
        /// Get string representaion of enum value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="that">Enum value.</param>
        /// <param name="nameProvider">Name provider provides names of defined enum values.</param>
        /// <returns>String representaion of enum value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameProvider"/> is <see langword="null"/>.</exception>
        public static string ToFriendlyNameString<T>(this T that, Func<string, string> nameProvider)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            if (nameProvider is null)
                throw new ArgumentNullException(nameof(nameProvider));
            return EnumExtentionCache<T>.Instance.ToFriendlyNameString(that, nameProvider);
        }

        /// <summary>
        /// Get string representaion of enum value.
        /// </summary>
        /// <param name="that">Enum value.</param>
        /// <param name="nameProvider">Name provider provides names of defined enum values.</param>
        /// <returns>String representaion of enum value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="that"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nameProvider"/> is <see langword="null"/>.</exception>
        public static string ToFriendlyNameString(this Enum that, Func<string, string> nameProvider)
        {
            if (nameProvider is null)
                throw new ArgumentNullException(nameof(nameProvider));
            if (that is null)
                throw new ArgumentNullException(nameof(that));
            return getForType(that.GetType()).ToFriendlyNameString(that, nameProvider);
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
            return EnumExtentionCache<T>.Instance.GetIndex(that) >= 0;
        }

        /// <summary>
        /// Check whether the enum is defined.
        /// </summary>
        /// <param name="that">Enum value to check.</param>
        /// <returns><see langword="true"/> if is defined.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="that"/> is <see langword="null"/>.</exception>
        public static bool IsDefined(this Enum that)
        {
            if (that is null)
                throw new ArgumentNullException(nameof(that));
            return Enum.IsDefined(that.GetType(), that);
        }
    }
}
