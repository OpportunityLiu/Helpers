﻿using System;
using System.Threading;
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

        public void Report(TProgress value) => this.progress?.Invoke(this, value);

        private AsyncActionProgressHandler<TProgress> progress;
        public AsyncActionProgressHandler<TProgress> Progress
        {
            get => this.progress;
            set
            {
                if (Interlocked.CompareExchange(ref this.progress, value, null) != null)
                    throw new InvalidOperationException("Progress has been set.");
            }
        }
    }
}