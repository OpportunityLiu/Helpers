using Opportunity.Helpers.Universal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Extension methods for enum.
    /// </summary>
    public static class EnumDisplayExtension
    {
        private static readonly Dictionary<Type, IDictionary> dispNameCache = new Dictionary<Type, IDictionary>();

        private static class EnumExtentionCache<T>
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            static EnumExtentionCache()
            {
                var type = typeof(T);
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    var name = field.Name;
                    var value = field.GetValue(null).Cast<T>();
                    var da = field.GetCustomAttribute<EnumDisplayNameAttribute>();
                    if (da == null || da.Value == null)
                        DisplayNames[value] = name;
                    else if (!da.Value.StartsWith("ms-resource:"))
                        DisplayNames[value] = da.Value;
                    else
                        DisplayNames[value] = LocalizedStrings.GetValue(da.Value);

                }
                dispNameCache.Add(type, DisplayNames);
            }
            public static readonly Dictionary<T, string> DisplayNames = new Dictionary<T, string>();
        }

        /// <summary>
        /// Get display text of an enum value.
        /// </summary>
        /// <typeparam name="T">The type of enum value, must be subtype of <see cref="Enum"/>.</typeparam>
        /// <param name="that">The enum value.</param>
        /// <returns>The display text.</returns>
        /// <seealso cref="EnumDisplayNameAttribute"/>
        public static string ToDisplayNameString<T>(this T that)
            where T : struct, Enum, IComparable, IFormattable, IConvertible
        {
            return EnumExtension.ToFriendlyNameString(that, v => EnumExtentionCache<T>.DisplayNames[v]);
        }

        /// <summary>
        /// Get display text of an enum value.
        /// </summary>
        /// <param name="that">The enum value.</param>
        /// <returns>The display text.</returns>
        /// <seealso cref="EnumDisplayNameAttribute"/>
        public static string ToDisplayNameString(this Enum that)
        {
            if (that is null)
                throw new ArgumentNullException(nameof(that));
            var t = that.GetType();
            if (!dispNameCache.TryGetValue(t, out var dic))
            {
                var cachet = typeof(EnumExtentionCache<>).MakeGenericType(t);
                RuntimeHelpers.RunClassConstructor(cachet.TypeHandle);
                dic = dispNameCache[t];
            }
            return EnumExtension.ToFriendlyNameString(that, (Enum v) => (string)dic[v]);
        }
    }
}
