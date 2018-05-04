using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Base class for async actions.
    /// </summary>
    public abstract class AsyncActionBase : AsyncInfoBase
    {
        internal AsyncActionBase() { }

        /// <summary>
        /// Set <see cref="AsyncInfoBase.Status"/> to <see cref="AsyncStatus.Completed"/>.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="AsyncInfoBase.Status"/> is previously <see cref="AsyncStatus.Started"/> and be set to <see cref="AsyncStatus.Completed"/> successfully, otherwise, <see langword="false"/>.</returns>
        public bool TrySetResults() => TrySetCompleted();

        /// <summary>
        /// Get results for async action.
        /// </summary>
        public void GetResults() => GetCompleted();
    }
}
