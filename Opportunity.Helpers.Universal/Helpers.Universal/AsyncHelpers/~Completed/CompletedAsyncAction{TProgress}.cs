using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    internal sealed class CompletedAsyncAction<TProgress> : CompletedAsyncBase, IAsyncActionWithProgress<TProgress>
    {
        internal CompletedAsyncAction(AsyncStatus status, Exception error)
            : base(status, error) { }

        AsyncActionWithProgressCompletedHandler<TProgress> IAsyncActionWithProgress<TProgress>.Completed { get => null; set => value?.Invoke(this, this.Status); }
        AsyncActionProgressHandler<TProgress> IAsyncActionWithProgress<TProgress>.Progress { get => null; set => value?.Invoke(this, default); }
    }
}
