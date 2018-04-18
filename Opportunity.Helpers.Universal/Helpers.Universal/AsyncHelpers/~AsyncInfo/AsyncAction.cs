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

        internal override void OnCompleted() => this.completed?.Invoke(this, this.Status);

        public bool TrySetResults() => TrySetCompleted();

        public void GetResults() => GetCompleted();

        public override void Close()
        {
            base.Close();
            this.completed = null;
        }
    }
}
