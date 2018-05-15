using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Implemetation of <see cref="IAsyncAction"/>.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncActionMethodBuilder))]
    public sealed class AsyncAction : AsyncActionBase, IAsyncAction
    {
        /// <summary>
        /// Create a complated <see cref="IAsyncAction"/>.
        /// </summary>
        /// <returns>A complated <see cref="IAsyncAction"/>.</returns>
        public static IAsyncAction CreateCompleted() => CompletedAsyncInfo<VoidResult, VoidProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncAction"/>.
        /// </summary>
        /// <returns>A faulted ended <see cref="IAsyncAction"/>.</returns>
        public static IAsyncAction CreateFault() => FaultedAsyncInfo<VoidResult, VoidProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncAction"/>.
        /// </summary>
        /// <param name="ex">Fault of the action.</param>
        /// <returns>A faulted ended <see cref="IAsyncAction"/>.</returns>
        public static IAsyncAction CreateFault(Exception ex) => FaultedAsyncInfo<VoidResult, VoidProgress>.Create(ex);
        /// <summary>
        /// Create a canceled <see cref="IAsyncAction"/>.
        /// </summary>
        /// <returns>A canceled <see cref="IAsyncAction"/>.</returns>
        public static IAsyncAction CreateCanceled() => CanceledAsyncInfo<VoidResult, VoidProgress>.Instanse;

        private AsyncActionCompletedHandler completed;
        /// <summary>
        /// Notifier for completion.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Completed"/> has been set.</exception>
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

        /// <summary>
        /// End the action.
        /// </summary>
        public override void Close()
        {
            base.Close();
            this.completed = null;
        }
    }
}
