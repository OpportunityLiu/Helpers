using Opportunity.Helpers.Universal.AsyncHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.UI.Core
{
    /// <summary>
    /// Yield methods for <see cref="CoreDispatcher"/>.
    /// </summary>
    public static class DispatcherYieldExtension
    {
        /// <summary>
        /// Create an awaiter source that asynchronously yields back to the UI context when awaited.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to execute callback on</param>
        /// <param name="priority">priority of callback</param>
        /// <returns>awaiter source for yield</returns>
        public static DispatcherYieldAwaiterSource Yield(this CoreDispatcher dispatcher, CoreDispatcherPriority priority)
            => new DispatcherYieldAwaiterSource(dispatcher, priority);

        /// <summary>
        /// Create an awaiter source that asynchronously yields back to the UI context when awaited with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to execute callback on</param>
        /// <returns>awaiter source for yield</returns>
        public static DispatcherYieldAwaiterSource Yield(this CoreDispatcher dispatcher)
            => Yield(dispatcher, CoreDispatcherPriority.Normal);

        /// <summary>
        /// Create an awaiter source that asynchronously yields back to the UI context when awaited with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to execute callback on</param>
        /// <returns>awaiter source for yield</returns>
        public static DispatcherYieldAwaiterSource YieldIdle(this CoreDispatcher dispatcher)
            => Yield(dispatcher, CoreDispatcherPriority.Idle);
    }

    /// <summary>
    /// Awaiter source for methods in <see cref="DispatcherExtension"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly struct DispatcherYieldAwaiterSource
    {
        internal DispatcherYieldAwaiterSource(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (priority > CoreDispatcherPriority.High)
                priority = CoreDispatcherPriority.High;
            else if (priority < CoreDispatcherPriority.Idle)
                priority = CoreDispatcherPriority.Idle;
            this.awaiter = new DispatcherYieldAwaiter(dispatcher, priority);
        }

        private readonly DispatcherYieldAwaiter awaiter;

        /// <summary>
        /// Get awaiter of this <see cref="DispatcherYieldAwaiterSource"/>.
        /// </summary>
        /// <returns>Awaiter of this <see cref="DispatcherYieldAwaiterSource"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatcherYieldAwaiter GetAwaiter() => this.awaiter;
    }

    /// <summary>
    /// Awaiter of <see cref="DispatcherYieldAwaiterSource"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly struct DispatcherYieldAwaiter : INotifyCompletion
    {
        private readonly CoreDispatcher dispatcher;
        private readonly CoreDispatcherPriority priority;

        internal DispatcherYieldAwaiter(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            this.dispatcher = dispatcher;
            this.priority = priority;
        }

        /// <summary>
        /// The awaiter is completed or not, for yield awaiter whose <see cref="CoreDispatcher"/> is not <see langword="null"/>,
        /// will always returns <see langword="false"/>.
        /// </summary>
        public bool IsCompleted => this.dispatcher is null;

        /// <summary>
        /// Get result of the awaiter, for yield awaiter, will do nothing.
        /// </summary> 
        public void GetResult() { }

        /// <summary>
        /// Schedules the continuation action that's invoked when the instance completes.
        /// </summary>
        /// <param name="continuation">the action to invoke when the operation completes</param>
        public void OnCompleted(Action continuation)
        {
            if (this.dispatcher is null)
            {
                continuation();
                return;
            }
            if (this.priority == CoreDispatcherPriority.Idle)
            {
                var ignore = this.dispatcher.RunIdleAsync(a => continuation());
            }
            else
            {
                var ignore = this.dispatcher.RunAsync(this.priority, () => continuation());
            }
        }
    }
}
