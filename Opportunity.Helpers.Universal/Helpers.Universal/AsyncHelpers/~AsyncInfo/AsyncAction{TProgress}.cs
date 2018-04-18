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

        internal override void OnCompleted() => this.completed?.Invoke(this, this.Status);

        private AsyncActionProgressHandler<TProgress> progress;
        public AsyncActionProgressHandler<TProgress> Progress
        {
            get => this.progress;
            set
            {
                if (Status != AsyncStatus.Started)
                    return;
                if (Interlocked.CompareExchange(ref this.progress, value, null) != null)
                    throw new InvalidOperationException("Progress has been set.");
            }
        }

        public void Report(TProgress progress)
        {
            if (Status != AsyncStatus.Started)
                return;
            this.progress?.Invoke(this, progress);
        }

        public bool TrySetResults() => TrySetCompleted();

        public void GetResults() => GetCompleted();

        public override void Close()
        {
            base.Close();
            this.progress = null;
            this.completed = null;
        }
    }
}
