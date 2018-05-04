using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Base class for async operations.
    /// </summary>
    public abstract class AsyncOperationBase<T> : AsyncInfoBase
    {
        internal AsyncOperationBase() { }

        private T results;
        /// <summary>
        /// Set <see cref="AsyncInfoBase.Status"/> to <see cref="AsyncStatus.Completed"/>.
        /// </summary>
        /// <param name="results">Results of async operation.</param>
        /// <returns><see langword="true"/> if <see cref="AsyncInfoBase.Status"/> is previously <see cref="AsyncStatus.Started"/> and be set to <see cref="AsyncStatus.Completed"/> successfully, otherwise, <see langword="false"/>.</returns>
        public bool TrySetResults(T results)
        {
            if (Status != AsyncStatus.Started)
                return false;

            var oldv = this.results;
            this.results = results;
            if (TrySetCompleted())
            {
                return true;
            }
            this.results = oldv;
            return false;
        }

        /// <summary>
        /// Get results for async operation.
        /// </summary>
        /// <returns>Results for async operation.</returns>
        public T GetResults()
        {
            GetCompleted();
            return this.results;
        }

        /// <summary>
        /// End the operation.
        /// </summary>
        public override void Close()
        {
            base.Close();
            (this.results as IDisposable)?.Dispose();
            this.results = default;
        }
    }
}
