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
            var tt = TypeTraits.Of(t);
            var tt2 = TypeTraits.Of<object>();
            Assert.AreEqual(tt, tt2);
            Assert.AreEqual(t, tt.Type);
            Assert.AreEqual(null, tt.NullableUnderlyingType);
            Assert.AreEqual(default(object), tt.Default);
        }

        [TestMethod]
        public void TestNullable()
        {
            {
                var t = typeof(int?);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<int?>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(typeof(int), tt.NullableUnderlyingType);
                Assert.AreEqual(default(int?), tt.Default);
            }
            {
                var t = typeof(StringComparison?);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<StringComparison?>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(typeof(StringComparison), tt.NullableUnderlyingType);
                Assert.AreEqual(default(StringComparison?), tt.Default);
            }
            {
                var t = typeof(DateTime?);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<DateTime?>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(typeof(DateTime), tt.NullableUnderlyingType);
                Assert.AreEqual(default(DateTime?), tt.Default);
            }
        }

        [TestMethod]
        public void TestPrimitive()
        {
            {
                var t = typeof(int);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<int>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(default(int), tt.Default);
            }
            {
                var t = typeof(byte);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<byte>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(default(byte), tt.Default);
            }
            {
                var t = typeof(double);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<double>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(default(double), tt.Default);
            }
            {
                var t = typeof(decimal);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<decimal>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(default(decimal), tt.Default);
            }
            {
                var t = typeof(char);
                var tt = TypeTraits.Of(t);
                var tt2 = TypeTraits.Of<char>();
                Assert.AreEqual(tt, tt2);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(default(char), tt.Default);
            }
        }

        [TestMethod]
        public void TestEnum()
        {
            var t = typeof(StringComparison);
            var tt = TypeTraits.Of(t);
            var tt2 = TypeTraits.Of<StringComparison>();
            Assert.AreEqual(tt, tt2);
            Assert.AreEqual(t, tt.Type);
            Assert.AreEqual(null, tt.NullableUnderlyingType);
            Assert.AreEqual(default(StringComparison), tt.Default);
        }

        [TestMethod]
        public void TestStruct()
        {
            var t = typeof(DateTime);
            var tt = TypeTraits.Of(t);
            var tt2 = TypeTraits.Of<DateTime>();
            Assert.AreEqual(tt, tt2);
            Assert.AreEqual(t, tt.Type);
            Assert.AreEqual(null, tt.NullableUnderlyingType);
            Assert.AreEqual(default(DateTime), tt.Default);
        }

        [TestMethod]
        public void TestInterface()
        {
            var t = typeof(IDictionary);
            var tt = TypeTraits.Of(t);
            var tt2 = TypeTraits.Of<IDictionary>();
            Assert.AreEqual(tt, tt2);
            Assert.AreEqual(t, tt.Type);
            Assert.AreEqual(null, tt.NullableUnderlyingType);
            Assert.AreEqual(default(IDictionary), tt.Default);
        }

        [TestMethod]
        public void TestUnfinishedGeneric()
        {
            {
                var t = typeof(IDictionary<,>);
                var tt = TypeTraits.Of(t);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(null, tt.Default);
            }
            {
                var t = typeof(Dictionary<,>);
                var tt = TypeTraits.Of(t);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(null, tt.Default);
            }
            {
                var t = typeof(KeyValuePair<,>);
                var tt = TypeTraits.Of(t);
                Assert.AreEqual(t, tt.Type);
                Assert.AreEqual(null, tt.NullableUnderlyingType);
                Assert.AreEqual(null, tt.Default);
            }
        }
    }
}
