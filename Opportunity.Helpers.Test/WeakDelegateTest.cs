using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opportunity.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class WeakDelegateTest
    {
        private static readonly object testSender = new object();
        private static readonly UnhandledExceptionEventArgs testArgs = new UnhandledExceptionEventArgs(new Exception(), false);
        private static int testConter;

        private static void testCore(UnhandledExceptionEventHandler dele, int expectedTestCounter)
        {
            var w = WeakDelegate.Create(dele);
            var otc = testConter;
            w(testSender, testArgs);
            Assert.AreEqual(expectedTestCounter, testConter - otc);
        }

        [TestMethod]
        public void LocalMethod()
        {
            testCore(localMethod, 1);

            void localMethod(object s, UnhandledExceptionEventArgs e)
            {
                Assert.AreSame(testSender, s);
                Assert.AreSame(testArgs, e);
                testConter += 1;
            }
        }

        [TestMethod]
        public void LocalMethodWithCapture()
        {
            testCore(localMethodWithCapture, 123);

            void localMethodWithCapture(object s, UnhandledExceptionEventArgs e)
            {
                Assert.AreSame(testSender, s);
                Assert.AreSame(testArgs, e);
                if (this is object)
                    testConter += 123;
            }
        }

        [TestMethod]
        public void InstanceMethod()
        {
            testCore(insMethod, 10);
        }
        private void insMethod(object s, UnhandledExceptionEventArgs e)
        {
            Assert.AreSame(testSender, s);
            Assert.AreSame(testArgs, e);
            testConter += 10;
        }

        [TestMethod]
        public void InstanceVirtualMethod()
        {
            testCore(InsVirMethod, 100);
        }
        public virtual void InsVirMethod(object s, UnhandledExceptionEventArgs e)
        {
            Assert.AreSame(testSender, s);
            Assert.AreSame(testArgs, e);
            testConter += 100;
        }

        [TestMethod]
        public void StaticMethod()
        {
            testCore(staticMethod, 1000);
        }
        private static void staticMethod(object s, UnhandledExceptionEventArgs e)
        {
            Assert.AreSame(testSender, s);
            Assert.AreSame(testArgs, e);
            testConter += 1000;
        }

        [TestMethod]
        public void Combined()
        {
            UnhandledExceptionEventHandler combined = localMethod;
            combined += insMethod;
            combined += InsVirMethod;
            combined += staticMethod;
            testCore(combined, 1111);

            void localMethod(object s, UnhandledExceptionEventArgs e)
            {
                Assert.AreSame(testSender, s);
                Assert.AreSame(testArgs, e);
                testConter += 1;
            }
        }
    }
}
