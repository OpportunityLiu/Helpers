using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncAction : AsyncInfoBase, IAsyncAction
    {
        public static IAsyncAction CreateCompleted() => CompletedAsyncInfo<VoidResult, VoidProgress>.Instanse;
        public static IAsyncAction CreateFault() => FaultedAsyncInfo<VoidResult, VoidProgress>.Instanse;
        public static IAsyncAction CreateFault(Exception ex) => FaultedAsyncInfo<VoidResult, VoidProgress>.Create(ex);
        public static IAsyncAction CreateCanceled() => CanceledAsyncInfo<VoidResult, VoidProgress>.Instanse;

        private AsyncActionCompletedHandler completed;
        public AsyncActionCompletedHandler Completed
        {
            get => this.completed;
            set
            {
                if (this.Status != AsyncStatus.Started)
                {
                    value?.Invoke(this, this.Status);
                    Interlocked.Exchange(ref this.completed, value);
                }
                else if (Interlocked.CompareExchange(ref this.completed, value, null) != null)
                    throw new InvalidOperationException("Completed has been set.");
            }
        }

        public bool TrySetResults()
        {
            var c = this.completed;
            if (PreSetResults())
            {
                c?.Invoke(this, this.Status);
                return true;
            }
            return false;
        }

        public void GetResults() => PreGetResults();

        public bool TrySetException(Exception ex)
        {
            var c = this.completed;
            if (PreSetException(ex))
            {
                c?.Invoke(this, this.Status);
                return true;
            }
            return false;
        }

        public override void Cancel()
        {
            var c = this.completed;
            if (PreCancel())
                c?.Invoke(this, this.Status);
        }
    }
}
