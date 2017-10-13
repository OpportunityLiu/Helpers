using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Opportunity.Helpers
{
    public struct TypeTraitsInfo : IEquatable<TypeTraitsInfo>
    {
        internal TypeTraitsInfo(Type type)
        {
            Type = type.GetTypeInfo();
            NullableUnderlyingType = Nullable.GetUnderlyingType(type)?.GetTypeInfo();
            if (Type.IsValueType && NullableUnderlyingType == null && !Type.ContainsGenericParameters)
                Default = Activator.CreateInstance(type);
            else
                Default = null;
        }

        public bool Equals(TypeTraitsInfo other) => this == other;

        public override bool Equals(object obj)
        {
            if (obj is TypeTraitsInfo ti)
                return this == ti;
            return false;
        }

        public override int GetHashCode()
        {
            if (this.Type == null)
                return -1;
            return this.Type.GetHashCode();
        }

        public static bool operator ==(TypeTraitsInfo typeTraitsInfo1, TypeTraitsInfo typeTraitsInfo2)
        {
            if (typeTraitsInfo1.Type == null)
                return typeTraitsInfo2.Type == null;
            return typeTraitsInfo1.Type.Equals(typeTraitsInfo2.Type);
        }

        public static bool operator !=(TypeTraitsInfo typeTraitsInfo1, TypeTraitsInfo typeTraitsInfo2) => !(typeTraitsInfo1 == typeTraitsInfo2);

        public TypeInfo Type { get; }
        public TypeInfo NullableUnderlyingType { get; }
        public object Default { get; }

        public bool IsNullable => NullableUnderlyingType != null;
        public bool CanBeNull => Default == null;
    }

    public static class TypeTraits
    {
        private static Dictionary<Type, TypeTraitsInfo> cache = new Dictionary<Type, TypeTraitsInfo>();

        private static class Cache<T>
        {
            public static readonly TypeTraitsInfo Value = new TypeTraitsInfo(typeof(T));
        }

        public static TypeTraitsInfo Of<T>() => Cache<T>.Value;

        public static TypeTraitsInfo Of(Type type)
            => cache.GetOrCreateValue(type ?? throw new ArgumentNullException(nameof(type)), key => new TypeTraitsInfo(key));

        public static TypeTraitsInfo GetInfo(this Type type) => Of(type);
    }
}
