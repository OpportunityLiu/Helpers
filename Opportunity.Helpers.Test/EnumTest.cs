using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class EnumTest
    {
        public enum Tbyte : byte
        {
            D = 0,
            P = 1,
        }
        public enum Tsbyte : sbyte
        {
            N = -1,
            D = 0,
            P = 1,
        }
        public enum Tint : int
        {
            N = -1,
            D = 0,
            P = 1,
        }
        public enum Tuint : uint
        {
            D = 0,
            P = 1,
        }
        public enum Tlong : long
        {
            N = -1,
            D = 0,
            P = 1,
        }
        public enum Tulong : ulong
        {
            D = 0,
            P = 1,
        }

        [TestMethod]
        public void EnumToUlong()
        {
            Assert.AreEqual(0ul, Tbyte.D.ToUInt64());
            Assert.AreEqual(0ul, Tsbyte.D.ToUInt64());
            Assert.AreEqual(0ul, Tint.D.ToUInt64());
            Assert.AreEqual(0ul, Tuint.D.ToUInt64());
            Assert.AreEqual(0ul, Tlong.D.ToUInt64());
            Assert.AreEqual(0ul, Tulong.D.ToUInt64());

            Assert.AreEqual(1ul, Tbyte.P.ToUInt64());
            Assert.AreEqual(1ul, Tsbyte.P.ToUInt64());
            Assert.AreEqual(1ul, Tint.P.ToUInt64());
            Assert.AreEqual(1ul, Tuint.P.ToUInt64());
            Assert.AreEqual(1ul, Tlong.P.ToUInt64());
            Assert.AreEqual(1ul, Tulong.P.ToUInt64());

            Assert.AreEqual(ulong.MaxValue, Tsbyte.N.ToUInt64());
            Assert.AreEqual(ulong.MaxValue, Tint.N.ToUInt64());
            Assert.AreEqual(ulong.MaxValue, Tlong.N.ToUInt64());
        }

        [TestMethod]
        public void EnumToUlongNonGeneric()
        {
            Assert.AreEqual(0ul, ((Enum)Tbyte.D).ToUInt64());
            Assert.AreEqual(0ul, ((Enum)Tsbyte.D).ToUInt64());
            Assert.AreEqual(0ul, ((Enum)Tint.D).ToUInt64());
            Assert.AreEqual(0ul, ((Enum)Tuint.D).ToUInt64());
            Assert.AreEqual(0ul, ((Enum)Tlong.D).ToUInt64());
            Assert.AreEqual(0ul, ((Enum)Tulong.D).ToUInt64());

            Assert.AreEqual(1ul, ((Enum)Tbyte.P).ToUInt64());
            Assert.AreEqual(1ul, ((Enum)Tsbyte.P).ToUInt64());
            Assert.AreEqual(1ul, ((Enum)Tint.P).ToUInt64());
            Assert.AreEqual(1ul, ((Enum)Tuint.P).ToUInt64());
            Assert.AreEqual(1ul, ((Enum)Tlong.P).ToUInt64());
            Assert.AreEqual(1ul, ((Enum)Tulong.P).ToUInt64());

            Assert.AreEqual(ulong.MaxValue, ((Enum)Tsbyte.N).ToUInt64());
            Assert.AreEqual(ulong.MaxValue, ((Enum)Tint.N).ToUInt64());
            Assert.AreEqual(ulong.MaxValue, ((Enum)Tlong.N).ToUInt64());
        }

        [TestMethod]
        public void UlongToEnum()
        {
            Assert.AreEqual(Tbyte.D, 0ul.ToEnum<Tbyte>());
            Assert.AreEqual(Tsbyte.D, 0ul.ToEnum<Tsbyte>());
            Assert.AreEqual(Tint.D, 0ul.ToEnum<Tint>());
            Assert.AreEqual(Tuint.D, 0ul.ToEnum<Tuint>());
            Assert.AreEqual(Tlong.D, 0ul.ToEnum<Tlong>());
            Assert.AreEqual(Tulong.D, 0ul.ToEnum<Tulong>());

            Assert.AreEqual(Tbyte.P, 1ul.ToEnum<Tbyte>());
            Assert.AreEqual(Tsbyte.P, 1ul.ToEnum<Tsbyte>());
            Assert.AreEqual(Tint.P, 1ul.ToEnum<Tint>());
            Assert.AreEqual(Tuint.P, 1ul.ToEnum<Tuint>());
            Assert.AreEqual(Tlong.P, 1ul.ToEnum<Tlong>());
            Assert.AreEqual(Tulong.P, 1ul.ToEnum<Tulong>());

            Assert.AreEqual(Tsbyte.N, ulong.MaxValue.ToEnum<Tsbyte>());
            Assert.AreEqual(Tint.N, ulong.MaxValue.ToEnum<Tint>());
            Assert.AreEqual(Tlong.N, ulong.MaxValue.ToEnum<Tlong>());
        }

        [TestMethod]
        public void IsDefined()
        {
            Assert.IsTrue(Tbyte.D.IsDefined());
            Assert.IsTrue(Tsbyte.D.IsDefined());
            Assert.IsTrue(Tint.D.IsDefined());
            Assert.IsTrue(Tuint.D.IsDefined());
            Assert.IsTrue(Tlong.D.IsDefined());
            Assert.IsTrue(Tulong.D.IsDefined());

            Assert.IsTrue(Tbyte.P.IsDefined());
            Assert.IsTrue(Tsbyte.P.IsDefined());
            Assert.IsTrue(Tint.P.IsDefined());
            Assert.IsTrue(Tuint.P.IsDefined());
            Assert.IsTrue(Tlong.P.IsDefined());
            Assert.IsTrue(Tulong.P.IsDefined());

            Assert.IsTrue(Tsbyte.N.IsDefined());
            Assert.IsTrue(Tint.N.IsDefined());
            Assert.IsTrue(Tlong.N.IsDefined());
        }

        [TestMethod]
        public void IsDefinedNonGeneric()
        {
            Assert.IsTrue(((Enum)Tbyte.D).IsDefined());
            Assert.IsTrue(((Enum)Tsbyte.D).IsDefined());
            Assert.IsTrue(((Enum)Tint.D).IsDefined());
            Assert.IsTrue(((Enum)Tuint.D).IsDefined());
            Assert.IsTrue(((Enum)Tlong.D).IsDefined());
            Assert.IsTrue(((Enum)Tulong.D).IsDefined());

            Assert.IsTrue(((Enum)Tbyte.P).IsDefined());
            Assert.IsTrue(((Enum)Tsbyte.P).IsDefined());
            Assert.IsTrue(((Enum)Tint.P).IsDefined());
            Assert.IsTrue(((Enum)Tuint.P).IsDefined());
            Assert.IsTrue(((Enum)Tlong.P).IsDefined());
            Assert.IsTrue(((Enum)Tulong.P).IsDefined());

            Assert.IsTrue(((Enum)Tsbyte.N).IsDefined());
            Assert.IsTrue(((Enum)Tint.N).IsDefined());
            Assert.IsTrue(((Enum)Tlong.N).IsDefined());
        }
    }
}
