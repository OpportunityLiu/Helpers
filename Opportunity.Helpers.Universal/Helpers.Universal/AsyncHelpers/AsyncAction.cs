using System;
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
                if (this.completed != null)
                    throw new InvalidOperationException("Completed has been set.");
                this.completed = value;
            }
        }

        public void SetResults()
        {
            PreSetResults();
            this.completed?.Invoke(this, this.Status);
        }

        public void GetResults() => PreGetResults();

        public override void SetException(Exception ex)
        {
            base.SetException(ex);
            this.completed?.Invoke(this, this.Status);
        }
    }
}
