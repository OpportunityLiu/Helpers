using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncAction<TProgress> : AsyncInfoBase, IAsyncActionWithProgress<TProgress>, IProgress<TProgress>
    {
        public static AsyncAction<TProgress> CreateCompleted()
        {
            var r = new AsyncAction<TProgress>();
            r.SetResults();
            return r;
        }

        public static AsyncAction<TProgress> CreateFault(Exception ex)
        {
            var r = new AsyncAction<TProgress>();
            r.SetException(ex);
            return r;
        }

        public static AsyncAction<TProgress> CreateCancelled()
        {
            var r = new AsyncAction<TProgress>();
            r.Cancel();
            return r;
        }

        private AsyncActionWithProgressCompletedHandler<TProgress> completed;
        public AsyncActionWithProgressCompletedHandler<TProgress> Completed
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

        public void Report(TProgress value) => this.progress?.Invoke(this, value);

        private AsyncActionProgressHandler<TProgress> progress;
        public AsyncActionProgressHandler<TProgress> Progress
        {
            get => this.progress;
            set
            {
                if (this.progress != null)
                    throw new InvalidOperationException("Completed has been set.");
                this.progress = value;
            }
        }
    }
}
