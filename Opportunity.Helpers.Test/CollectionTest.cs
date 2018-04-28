using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Opportunity.Helpers.Universal.AsyncHelpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using System.Linq;
using System.Collections.ObjectModel;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class CollectionTest
    {
        [TestMethod]
        public void NullCollection()
        {
            var c = default(IEnumerable<string>);
            Assert.IsTrue(c.IsNullOrEmpty());
            Assert.ThrowsException<ArgumentNullException>(() => c.IsEmpty());
        }

        [TestMethod]
        public void EmptyCollection()
        {
            var c = new IEnumerable<string>[]
            {
                Array.Empty<string>(),
                g(),
                new List<string>(),
                new ReadOnlyCollection<string>(Array.Empty<string>()),
                new System.Collections.Concurrent.ConcurrentBag<string>(),
                new ObservableCollection<string>(),
            };

            foreach (var item in c)
            {
                Assert.IsTrue(item.IsNullOrEmpty());
                Assert.IsTrue(item.IsEmpty());
            }

            IEnumerable<string> g()
            {
                yield break;
            }
        }

        [TestMethod]
        public void NonEmptyCollection()
        {
            var c = new IEnumerable<string>[]
            {
                new string[]{ null },
                g(),
                new List<string>{ null },
                new ReadOnlyCollection<string>(new string[]{ null }),
                new System.Collections.Concurrent.ConcurrentBag<string>{ null },
                new ObservableCollection<string>{ null },
            };

            foreach (var item in c)
            {
                Assert.IsFalse(item.IsNullOrEmpty());
                Assert.IsFalse(item.IsEmpty());
            }

            IEnumerable<string> g()
            {
                yield return null;
                Assert.Fail("Empty test should never reach here.");
                yield return null;
                yield return null;
                yield return null;
            }
        }
    }
}
