using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Opportunity.Helpers.Universal.AsyncHelpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class MulticastTest
    {
        [TestMethod]
        public void MulticastOP()
        {
            using (var client = new HttpClient())
            {
                var g = client.GetAsync(new Uri("https://www.baidu.com"));
                var action = g.AsMulticast();
                action.Completed += (s, e) =>
                {

                };
                action.Completed += (s, e) =>
                {

                };
                action.Completed += (s, e) =>
                {

                };
                action.Progress = (s, e) =>
                {

                };
                action.Progress = (s, e) =>
                {

                };
                action.Progress = (s, e) =>
                {

                };
            }
        }
    }
}
