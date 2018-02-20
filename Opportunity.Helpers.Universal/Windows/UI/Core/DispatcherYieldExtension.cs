﻿using Opportunity.Helpers.Universal.AsyncHelpers;
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
    public static class DispatcherYieldExtension
    {
        public static DispatcherAwaiterSource Yield(this CoreDispatcher dispatcher, CoreDispatcherPriority priority)
            => new DispatcherAwaiterSource(dispatcher, priority);

        public static DispatcherAwaiterSource Yield(this CoreDispatcher dispatcher)
            => Yield(dispatcher, CoreDispatcherPriority.Normal);

        public static DispatcherAwaiterSource YieldIdle(this CoreDispatcher dispatcher)
            => Yield(dispatcher, CoreDispatcherPriority.Idle);
    }

    /// <summary>
    /// Awaiter source for methods in <see cref="DispatcherExtension"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct DispatcherAwaiterSource
    {
        internal DispatcherAwaiterSource(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (priority > CoreDispatcherPriority.High)
                priority = CoreDispatcherPriority.High;
            else if (priority < CoreDispatcherPriority.Idle)
                priority = CoreDispatcherPriority.Idle;
            this.awaiter = new DispatcherAwaiter(dispatcher, priority);
        }

        private readonly DispatcherAwaiter awaiter;

        /// <summary>
        /// Get awaiter of this <see cref="DispatcherAwaiterSource"/>.
        /// </summary>
        /// <returns>Awaiter of this <see cref="DispatcherAwaiterSource"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatcherAwaiter GetAwaiter() => this.awaiter;
    }

    /// <summary>
    /// Awaiter of <see cref="DispatcherAwaiterSource"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct DispatcherAwaiter : INotifyCompletion
    {
        private readonly CoreDispatcher dispatcher;
        private readonly CoreDispatcherPriority priority;

        internal DispatcherAwaiter(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            this.dispatcher = dispatcher;
            this.priority = priority;
        }

        /// <summary>
        /// The awaiter is completed or not, for yield awaiter whose <see cref="CoreDispatcher"/> is not <see langword="null"/>,
        /// will always returns <see langword="false"/>.
        /// </summary>
        public bool IsCompleted => this.dispatcher == null;

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
            if (this.dispatcher == null)
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
