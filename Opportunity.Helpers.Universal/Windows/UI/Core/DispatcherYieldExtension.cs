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
            if (dispatcher == null)
                this.awaiter = new EmptyDispatcherAwaiter();
            else if (priority == CoreDispatcherPriority.Idle)
                this.awaiter = new IdleDispatcherAwaiter(dispatcher);
            else
                this.awaiter = new NormalDispatcherAwaiter(dispatcher, priority);
        }

        private readonly IDispatcherAwaiter awaiter;

        /// <summary>
        /// Get awaiter of this <see cref="DispatcherAwaiterSource"/>.
        /// </summary>
        /// <returns>Awaiter of this <see cref="DispatcherAwaiterSource"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IDispatcherAwaiter GetAwaiter() => this.awaiter;
    }

    /// <summary>
    /// Awaiter of <see cref="DispatcherAwaiterSource"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDispatcherAwaiter : INotifyCompletion
    {
        /// <summary>
        /// The awaiter is completed or not, for yield awaiter whose <see cref="CoreDispatcher"/> is not <c>null</c>, will always returns <c>false</c>.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Get result of the awaiter, for yield awaiter, will do nothing.
        /// </summary> 
        void GetResult();
    }

    internal sealed class EmptyDispatcherAwaiter : IDispatcherAwaiter
    {
        public EmptyDispatcherAwaiter() { }

        public bool IsCompleted => true;

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            continuation();
        }
    }

    internal sealed class IdleDispatcherAwaiter : IDispatcherAwaiter
    {
        private readonly CoreDispatcher dispatcher;

        public IdleDispatcherAwaiter(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool IsCompleted => false;

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            var ignore = this.dispatcher.RunIdleAsync(a => continuation());
        }
    }

    internal sealed class NormalDispatcherAwaiter : IDispatcherAwaiter
    {
        private readonly CoreDispatcher dispatcher;
        private readonly CoreDispatcherPriority priority;

        internal NormalDispatcherAwaiter(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            this.dispatcher = dispatcher;
            this.priority = priority;
        }

        public bool IsCompleted => false;

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            var ignore = this.dispatcher.RunAsync(this.priority, () => continuation());
        }
    }
}
