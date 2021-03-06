﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Implemetation of <see cref="IAsyncOperation{TResult}"/>.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncOperationMethodBuilder<>))]
    public sealed class AsyncOperation<T> : AsyncOperationBase<T>, IAsyncOperation<T>
    {
        /// <summary>
        /// Create a complated <see cref="IAsyncOperation{TResult}"/>.
        /// </summary>
        /// <returns>A complated <see cref="IAsyncOperation{TResult}"/>.</returns>
        public static IAsyncOperation<T> CreateCompleted() => CompletedAsyncInfo<T, VoidProgress>.Instanse;
        /// <summary>
        /// Create a complated <see cref="IAsyncOperation{TResult}"/>.
        /// </summary>
        /// <param name="results">Results of the operation.</param>
        /// <returns>A complated <see cref="IAsyncOperation{TResult}"/>.</returns>
        public static IAsyncOperation<T> CreateCompleted(T results)
            => AsyncOperationCache<VoidProgress>.TryGetCacehd(results) ?? CompletedAsyncInfo<T, VoidProgress>.Create(results);
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncOperation{TResult}"/>.
        /// </summary>
        /// <returns>A faulted ended <see cref="IAsyncOperation{TResult}"/>.</returns>
        public static IAsyncOperation<T> CreateFault() => FaultedAsyncInfo<T, VoidProgress>.Instanse;
        /// <summary>
        /// Create a faulted ended <see cref="IAsyncOperation{TResult}"/>.
        /// </summary>
        /// <param name="ex">Fault of the operation.</param>
        /// <returns>A faulted ended <see cref="IAsyncOperation{TResult}"/>.</returns>
        public static IAsyncOperation<T> CreateFault(Exception ex) => FaultedAsyncInfo<T, VoidProgress>.Create(ex);
        /// <summary>
        /// Create a canceled <see cref="IAsyncOperation{TResult}"/>.
        /// </summary>
        /// <returns>A canceled <see cref="IAsyncOperation{TResult}"/>.</returns>
        public static IAsyncOperation<T> CreateCanceled() => CanceledAsyncInfo<T, VoidProgress>.Instanse;

        private AsyncOperationCompletedHandler<T> completed;
        /// <summary>
        /// Notifier for completion.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Completed"/> has been set.</exception>
        public AsyncOperationCompletedHandler<T> Completed
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
        /// End the operation.
        /// </summary>
        public override void Close()
        {
            base.Close();
            this.completed = null;
        }
    }
}
