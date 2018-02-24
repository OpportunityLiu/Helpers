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
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncAction.CreateFault();
            a2 = AsyncAction.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncAction.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
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
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.AreSame(a2, a1);
            a1 = AsyncAction<int>.CreateFault();
            a2 = AsyncAction<int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncAction<int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void String()
        {
            var a1 = AsyncOperation<string>.CreateCompleted();
            var a2 = AsyncOperation<string>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string>.CreateCanceled();
            a2 = AsyncOperation<string>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string>.CreateCompleted("");
            a2 = AsyncOperation<string>.CreateCompleted("");
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual("", a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string>.CreateFault();
            a2 = AsyncOperation<string>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void StringProgress()
        {
            var a1 = AsyncOperation<string, int>.CreateCompleted();
            var a2 = AsyncOperation<string, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string, int>.CreateCanceled();
            a2 = AsyncOperation<string, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string, int>.CreateCompleted("");
            a2 = AsyncOperation<string, int>.CreateCompleted("");
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual("", a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string, int>.CreateFault();
            a2 = AsyncOperation<string, int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<string, int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void Bool()
        {
            var a1 = AsyncOperation<bool>.CreateCompleted();
            var a2 = AsyncOperation<bool>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCanceled();
            a2 = AsyncOperation<bool>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCompleted(true);
            a2 = AsyncOperation<bool>.CreateCompleted(true);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(true, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateCompleted(false);
            a2 = AsyncOperation<bool>.CreateCompleted(false);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(false, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateFault();
            a2 = AsyncOperation<bool>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void BoolProgress()
        {
            var a1 = AsyncOperation<bool, int>.CreateCompleted();
            var a2 = AsyncOperation<bool, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCanceled();
            a2 = AsyncOperation<bool, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCompleted(true);
            a2 = AsyncOperation<bool, int>.CreateCompleted(true);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(true, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateCompleted(false);
            a2 = AsyncOperation<bool, int>.CreateCompleted(false);
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(false, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateFault();
            a2 = AsyncOperation<bool, int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<bool, int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void Int32()
        {
            var a1 = AsyncOperation<int>.CreateCompleted();
            var a2 = AsyncOperation<int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int>.CreateCanceled();
            a2 = AsyncOperation<int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<int>.CreateCompleted(i);
                a2 = AsyncOperation<int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache<VoidProgress>.CACHE_START && i < AsyncOperationCache<VoidProgress>.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
                Assert.AreEqual(i, a1.GetResults());
                Assert.AreEqual(i, a2.GetResults());
            }
            a1 = AsyncOperation<int>.CreateFault();
            a2 = AsyncOperation<int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void Int32Progress()
        {
            var a1 = AsyncOperation<int, int>.CreateCompleted();
            var a2 = AsyncOperation<int, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int, int>.CreateCanceled();
            a2 = AsyncOperation<int, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<int, int>.CreateCompleted(i);
                a2 = AsyncOperation<int, int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache<int>.CACHE_START && i < AsyncOperationCache<int>.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
                Assert.AreEqual(i, a1.GetResults());
                Assert.AreEqual(i, a2.GetResults());
            }
            a1 = AsyncOperation<int, int>.CreateFault();
            a2 = AsyncOperation<int, int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<int, int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void Int64()
        {
            var a1 = AsyncOperation<long>.CreateCompleted();
            var a2 = AsyncOperation<long>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long>.CreateCanceled();
            a2 = AsyncOperation<long>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<long>.CreateCompleted(i);
                a2 = AsyncOperation<long>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache<VoidProgress>.CACHE_START && i < AsyncOperationCache<VoidProgress>.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
                Assert.AreEqual(i, a1.GetResults());
                Assert.AreEqual(i, a2.GetResults());
            }
            a1 = AsyncOperation<long>.CreateFault();
            a2 = AsyncOperation<long>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }

        [TestMethod]
        public void Int64Progress()
        {
            var a1 = AsyncOperation<long, int>.CreateCompleted();
            var a2 = AsyncOperation<long, int>.CreateCompleted();
            Assert.AreEqual(AsyncStatus.Completed, a1.Status);
            Assert.AreEqual(default, a1.GetResults());
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long, int>.CreateCanceled();
            a2 = AsyncOperation<long, int>.CreateCanceled();
            Assert.AreEqual(AsyncStatus.Canceled, a1.Status);
            Assert.ThrowsException<OperationCanceledException>(() => a1.GetResults());
            Assert.AreSame(a2, a1);
            for (var i = -10; i < 100; i++)
            {
                a1 = AsyncOperation<long, int>.CreateCompleted(i);
                a2 = AsyncOperation<long, int>.CreateCompleted(i);
                Assert.AreEqual(AsyncStatus.Completed, a1.Status);
                if (i >= AsyncOperationCache<int>.CACHE_START && i < AsyncOperationCache<int>.CACHE_END)
                    Assert.AreSame(a2, a1);
                else
                    Assert.AreNotSame(a2, a1);
                Assert.AreEqual(i, a1.GetResults());
                Assert.AreEqual(i, a2.GetResults());
            }
            a1 = AsyncOperation<long, int>.CreateFault();
            a2 = AsyncOperation<long, int>.CreateFault();
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<Exception>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreSame(a2, a1);
            a1 = AsyncOperation<long, int>.CreateFault(new InvalidOperationException("text"));
            Assert.AreEqual(AsyncStatus.Error, a1.Status);
            Assert.ThrowsException<InvalidOperationException>(() => a1.GetResults());
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
            a1.Close();
            Assert.IsNotNull(a1.ErrorCode);
            Assert.AreEqual("text", a1.ErrorCode.Message);
        }
    }
}
