﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncAction : AsyncInfoBase, IAsyncAction
    {
        private static class Cache
        {
            public static readonly AsyncAction Completed = createCompleted();
            private static AsyncAction createCompleted()
            {
                var r = new AsyncAction();
                r.SetResults();
                return r;
            }

            public static readonly AsyncAction Canceled = createCanceled();
            private static AsyncAction createCanceled()
            {
                var r = new AsyncAction();
                r.Cancel();
                return r;
            }
        }

        public static AsyncAction CreateCompleted() => Cache.Completed;

        public static AsyncAction CreateFault(Exception ex)
        {
            var r = new AsyncAction();
            r.SetException(ex);
            return r;
        }

        public static AsyncAction CreateCanceled() => Cache.Canceled;

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
