using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Opportunity.Helpers.Universal.AsyncHelpers;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class AsyncCacheTest
    {
        [TestMethod]
        public void Action()
        {
            var a1 = AsyncAction.CreateCompleted();
            var a2 = AsyncAction.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncAction.CreateCanceled();
            a2 = AsyncAction.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
        }

        [TestMethod]
        public void ActionProgress()
        {
            var a1 = AsyncAction<int>.CreateCompleted();
            var a2 = AsyncAction<int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncAction<int>.CreateCanceled();
            a2 = AsyncAction<int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
        }

        [TestMethod]
        public void Bool()
        {
            var a1 = AsyncOperation<bool>.CreateCompleted();
            var a2 = AsyncOperation<bool>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCanceled();
            a2 = AsyncOperation<bool>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCompleted(true);
            a2 = AsyncOperation<bool>.CreateCompleted(true);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCompleted(false);
            a2 = AsyncOperation<bool>.CreateCompleted(false);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
        }

        [TestMethod]
        public void BoolProgress()
        {
            var a1 = AsyncOperation<bool, int>.CreateCompleted();
            var a2 = AsyncOperation<bool, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCanceled();
            a2 = AsyncOperation<bool, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCompleted(true);
            a2 = AsyncOperation<bool, int>.CreateCompleted(true);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCompleted(false);
            a2 = AsyncOperation<bool, int>.CreateCompleted(false);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
        }

        [TestMethod]
        public void Int32()
        {
            var a1 = AsyncOperation<int>.CreateCompleted();
            var a2 = AsyncOperation<int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int>.CreateCanceled();
            a2 = AsyncOperation<int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<int>.CreateCompleted(i);
                a2 = AsyncOperation<int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache.CACHE_START && i < AsyncOperationCache.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
            }
        }

        [TestMethod]
        public void Int32Progress()
        {
            var a1 = AsyncOperation<int, int>.CreateCompleted();
            var a2 = AsyncOperation<int, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int, int>.CreateCanceled();
            a2 = AsyncOperation<int, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<int, int>.CreateCompleted(i);
                a2 = AsyncOperation<int, int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache.CACHE_START && i < AsyncOperationCache.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
            }
        }

        [TestMethod]
        public void Int64()
        {
            var a1 = AsyncOperation<long>.CreateCompleted();
            var a2 = AsyncOperation<long>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long>.CreateCanceled();
            a2 = AsyncOperation<long>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<long>.CreateCompleted(i);
                a2 = AsyncOperation<long>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache.CACHE_START && i < AsyncOperationCache.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
            }
        }

        [TestMethod]
        public void Int64Progress()
        {
            var a1 = AsyncOperation<long, int>.CreateCompleted();
            var a2 = AsyncOperation<long, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long, int>.CreateCanceled();
            a2 = AsyncOperation<long, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<long, int>.CreateCompleted(i);
                a2 = AsyncOperation<long, int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache.CACHE_START && i < AsyncOperationCache.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
            }
        }
    }
}
