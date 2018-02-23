using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class CastHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cast<T>(this object obj) => (T)obj;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryCast<T>(this object obj, T defaultValue)
        {
            if (obj is T v)
                return v;
            return defaultValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryCast<T>(this object obj) => TryCast(obj, default(T));
    }
}
