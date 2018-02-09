using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CompletedAsyncAction : CompletedAsyncBase, IAsyncAction
    {
        internal CompletedAsyncAction(AsyncStatus status, Exception error)
            : base(status, error) { }

        AsyncActionCompletedHandler IAsyncAction.Completed { get => null; set => value?.Invoke(this, this.Status); }
    }
}
