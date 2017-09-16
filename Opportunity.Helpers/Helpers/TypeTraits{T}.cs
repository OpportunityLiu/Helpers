using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Opportunity.Helpers
{
    public static class TypeTraits<T>
    {
        static TypeTraits()
        {
            var type = typeof(T);
            Type = type.GetTypeInfo();
            NullableUnderlyingType = Nullable.GetUnderlyingType(type)?.GetTypeInfo();
        }

        public static TypeInfo Type { get; }
        public static TypeInfo NullableUnderlyingType { get; }

        public static bool IsNullable => NullableUnderlyingType != null;
        public static T Default => default(T);


    }
}
