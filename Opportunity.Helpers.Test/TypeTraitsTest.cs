using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using Opportunity.Helpers;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class TypeTraitsTest
    {
        [TestMethod]
        public void TestClass()
        {
            var t = typeof(object);
            Assert.AreEqual(t, TypeTraits<object>.Type);
            Assert.AreEqual(null, TypeTraits<object>.NullableUnderlyingType);
            Assert.AreEqual(false, TypeTraits<object>.IsNullable);
            Assert.AreEqual(true, TypeTraits<object>.Type.IsClass);
        }

        [TestMethod]
        public void TestNullable()
        {
            {
                var t = typeof(int?);
                Assert.AreEqual(t, TypeTraits<int?>.Type);
                Assert.AreEqual(typeof(int), TypeTraits<int?>.NullableUnderlyingType);
                Assert.AreEqual(true, TypeTraits<int?>.IsNullable);
                Assert.AreEqual(false, TypeTraits<int?>.Type.IsClass);
            }
            {
                var t = typeof(StringComparison?);
                Assert.AreEqual(t, TypeTraits<StringComparison?>.Type);
                Assert.AreEqual(typeof(StringComparison), TypeTraits<StringComparison?>.NullableUnderlyingType);
                Assert.AreEqual(true, TypeTraits<StringComparison?>.IsNullable);
                Assert.AreEqual(false, TypeTraits<StringComparison?>.Type.IsClass);
            }
            {
                var t = typeof(DateTime?);
                Assert.AreEqual(t, TypeTraits<DateTime?>.Type);
                Assert.AreEqual(typeof(DateTime), TypeTraits<DateTime?>.NullableUnderlyingType);
                Assert.AreEqual(true, TypeTraits<DateTime?>.IsNullable);
                Assert.AreEqual(false, TypeTraits<DateTime?>.Type.IsClass);
            }
        }

        [TestMethod]
        public void TestPrimitive()
        {
            {
                var t = typeof(int);
                Assert.AreEqual(t, TypeTraits<int>.Type);
                Assert.AreEqual(null, TypeTraits<int>.NullableUnderlyingType);
                Assert.AreEqual(false, TypeTraits<int>.IsNullable);
                Assert.AreEqual(false, TypeTraits<int>.Type.IsClass);
            }
            {
                var t = typeof(byte);
                Assert.AreEqual(t, TypeTraits<byte>.Type);
                Assert.AreEqual(null, TypeTraits<byte>.NullableUnderlyingType);
                Assert.AreEqual(false, TypeTraits<byte>.IsNullable);
                Assert.AreEqual(false, TypeTraits<byte>.Type.IsClass);
            }
            {
                var t = typeof(double);
                Assert.AreEqual(t, TypeTraits<double>.Type);
                Assert.AreEqual(null, TypeTraits<double>.NullableUnderlyingType);
                Assert.AreEqual(false, TypeTraits<double>.IsNullable);
                Assert.AreEqual(false, TypeTraits<double>.Type.IsClass);
            }
            {
                var t = typeof(decimal);
                Assert.AreEqual(t, TypeTraits<decimal>.Type);
                Assert.AreEqual(null, TypeTraits<decimal>.NullableUnderlyingType);
                Assert.AreEqual(false, TypeTraits<decimal>.IsNullable);
                Assert.AreEqual(false, TypeTraits<decimal>.Type.IsClass);
            }
            {
                var t = typeof(char);
                Assert.AreEqual(t, TypeTraits<char>.Type);
                Assert.AreEqual(null, TypeTraits<char>.NullableUnderlyingType);
                Assert.AreEqual(false, TypeTraits<char>.IsNullable);
                Assert.AreEqual(false, TypeTraits<char>.Type.IsClass);
            }
        }

        [TestMethod]
        public void TestEnum()
        {
            var t = typeof(StringComparison);
            Assert.AreEqual(t, TypeTraits<StringComparison>.Type);
            Assert.AreEqual(null, TypeTraits<StringComparison>.NullableUnderlyingType);
            Assert.AreEqual(false, TypeTraits<StringComparison>.IsNullable);
            Assert.AreEqual(false, TypeTraits<StringComparison>.Type.IsClass);
        }

        [TestMethod]
        public void TestStruct()
        {
            var t = typeof(DateTime);
            Assert.AreEqual(t, TypeTraits<DateTime>.Type);
            Assert.AreEqual(null, TypeTraits<DateTime>.NullableUnderlyingType);
            Assert.AreEqual(false, TypeTraits<DateTime>.IsNullable);
            Assert.AreEqual(false, TypeTraits<DateTime>.Type.IsClass);
        }
    }
}
