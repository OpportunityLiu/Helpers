using System;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public sealed class AsyncAction<TProgress> : AsyncInfoBase, IAsyncActionWithProgress<TProgress>, IProgress<TProgress>
    {
        public static IAsyncActionWithProgress<TProgress> CreateCompleted() => CompletedAsyncInfo<VoidResult, TProgress>.Instanse;
        public static IAsyncActionWithProgress<TProgress> CreateFault() => FaultedAsyncInfo<VoidResult, TProgress>.Instanse;
        public static IAsyncActionWithProgress<TProgress> CreateFault(Exception ex) => FaultedAsyncInfo<VoidResult, TProgress>.Create(ex);
        public static IAsyncActionWithProgress<TProgress> CreateCanceled() => CanceledAsyncInfo<VoidResult, TProgress>.Instanse;

        private AsyncActionWithProgressCompletedHandler<TProgress> completed;
        public AsyncActionWithProgressCompletedHandler<TProgress> Completed
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
