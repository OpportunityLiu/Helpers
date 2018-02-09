using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CompletedAsyncOperation<T, TProgress> : CompletedAsyncBase, IAsyncOperationWithProgress<T, TProgress>
    {
        internal CompletedAsyncOperation(AsyncStatus status, T result, Exception error)
            : base(status, error)
        {
            this.result = result;
        }

        private T result;

        public AsyncOperationWithProgressCompletedHandler<T, TProgress> Completed { get => null; set => value?.Invoke(this, this.Status); }
        public AsyncOperationProgressHandler<T, TProgress> Progress { get => null; set => value?.Invoke(this, default); }

        public override void Close()
        {
            (this.result as IDisposable)?.Dispose();
            this.result = default;
            base.Close();
        }

        T IAsyncOperationWithProgress<T, TProgress>.GetResults()
        {
            GetResults();
            return this.result;
        }
    }
}
