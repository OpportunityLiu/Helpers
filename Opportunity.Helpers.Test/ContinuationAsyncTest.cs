using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opportunity.Helpers.Universal.AsyncHelpers;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Opportunity.Helpers.Test
{
    [TestClass]
    public class ContinuationAsyncTest
    {
        [TestMethod]
        public async Task Successful()
        {
            var begin1 = AsyncInfo.Run(async token => { await Task.Delay(100); return 123; });
            await begin1.ContinueWith(t => Assert.AreEqual(123, t.GetResults()));

            var begin2 = AsyncInfo.Run(async token => { await Task.Delay(100); return 123; });
            Assert.AreEqual("", await begin2.ContinueWith(t => { Assert.AreEqual(123, t.GetResults()); return ""; }));

            begin1 = AsyncInfo.Run(async token => { await Task.Delay(100); return 123; });
            await begin1;
            await begin1.ContinueWith(t => Assert.AreEqual(123, t.GetResults()));

            begin2 = AsyncInfo.Run(async token => { await Task.Delay(100); return 123; });
            await begin2;
            Assert.AreEqual("", await begin2.ContinueWith(t => { Assert.AreEqual(123, t.GetResults()); return ""; }));
        }

        [TestMethod]
        public async Task Cancel()
        {
            var begin1 = AsyncInfo.Run(async token => { await Task.Delay(100); token.ThrowIfCancellationRequested(); return 123; });
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () => await begin1.ContinueWith(t => throw new OperationCanceledException()));

            var begin2 = AsyncInfo.Run(async token => { await Task.Delay(100); token.ThrowIfCancellationRequested(); return 123; });
            begin2.Cancel();
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await begin2.ContinueWith(t => 0));

            begin1 = AsyncInfo.Run(async token => { await Task.Delay(100); token.ThrowIfCancellationRequested(); return 123; });
            await begin1;
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () => await begin1.ContinueWith(t => throw new OperationCanceledException()));

            begin2 = AsyncInfo.Run(async token => { await Task.Delay(100); token.ThrowIfCancellationRequested(); return 123; });
            begin2.Cancel();
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () => await begin2);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await begin2.ContinueWith(t =>
            {
                return 0;
            }));
        }
    }
}
