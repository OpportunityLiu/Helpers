using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opportunity.Helpers.Universal.AsyncHelpers;
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
                var action = client.GetAsync(new Uri("https://www.baidu.com")).AsMulticast();
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
