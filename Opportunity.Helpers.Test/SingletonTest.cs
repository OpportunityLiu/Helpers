using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Opportunity.Helpers.ObjectModel;
using Opportunity.Helpers.Universal.AsyncHelpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        public void Basic()
        {
            Assert.IsNull(Singleton.Get<object>());
            Assert.AreSame(Singleton.GetOrCreate<object>(), Singleton.GetOrCreate(() => new object()));
            var o = Singleton.Get<object>();
            Assert.AreSame(o, Singleton.Reset<object>());
            Assert.IsNull(Singleton.Get<object>());
            Assert.IsNotNull(Singleton.GetOrDefault(new object()));
            Assert.IsNull(Singleton.Set(new object()));
            Assert.AreSame(Singleton.Get<object>(), Singleton.GetOrCreate<object>());
        }

        [TestMethod]
        public void BasicThreadLocal()
        {
            Assert.IsNull(ThreadLocalSingleton.Get<object>());
            Assert.AreEqual(0, ThreadLocalSingleton.Count<object>());
            Assert.AreSame(ThreadLocalSingleton.GetOrCreate<object>(), ThreadLocalSingleton.GetOrCreate(() => new object()));
            Assert.AreEqual(1, ThreadLocalSingleton.Count<object>());
            var o = ThreadLocalSingleton.Get<object>();
            Assert.AreSame(o, ThreadLocalSingleton.Reset<object>());
            Assert.AreEqual(0, ThreadLocalSingleton.Count<object>());
            Assert.IsNull(ThreadLocalSingleton.Get<object>());
            Assert.AreEqual(0, ThreadLocalSingleton.Count<object>());
            Assert.IsNotNull(ThreadLocalSingleton.GetOrDefault(new object()));
            Assert.AreEqual(0, ThreadLocalSingleton.Count<object>());
            Assert.IsNull(ThreadLocalSingleton.Set(new object()));
            Assert.AreEqual(1, ThreadLocalSingleton.Count<object>());
            Assert.AreSame(ThreadLocalSingleton.Get<object>(), ThreadLocalSingleton.GetOrCreate<object>());
            Assert.AreEqual(1, ThreadLocalSingleton.Count<object>());
        }
    }
}
