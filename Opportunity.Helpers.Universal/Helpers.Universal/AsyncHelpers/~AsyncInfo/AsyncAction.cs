using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncAction : AsyncInfoBase, IAsyncAction
    {
        public static AsyncAction CreateCompleted()
        {
            var r = new AsyncAction();
            r.SetResults();
            return r;
        }

        public static AsyncAction CreateFault(Exception ex)
        {
            var r = new AsyncAction();
            r.SetException(ex);
            return r;
        }

        public static AsyncAction CreateCancelled()
        {
            var r = new AsyncAction();
            r.Cancel();
            return r;
        }

        private AsyncActionCompletedHandler completed;
        public AsyncActionCompletedHandler Completed
        {
            get => this.completed;
            set
            {
                if (Interlocked.CompareExchange(ref this.completed, value, null) != null)
                    throw new InvalidOperationException("Completed has been set.");
                if (this.Status != AsyncStatus.Started)
                    value?.Invoke(this, this.Status);
            }
        }

        public void SetResults()
        {
            var c = this.completed;
            PreSetResults();
            c?.Invoke(this, this.Status);
        }

        public void GetResults() => PreGetResults();

        public void SetException(Exception ex)
        {
            var c = this.completed;
            PreSetException(ex);
            c?.Invoke(this, this.Status);
        }

        public override void Cancel()
        {
            var c = this.completed;
            if (PreCancel())
                c?.Invoke(this, this.Status);
        }
    }
}
