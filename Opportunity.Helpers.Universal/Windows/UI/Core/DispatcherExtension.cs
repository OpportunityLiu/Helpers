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
    /// Extension methods for <see cref="CoreDispatcher"/>.
    /// </summary>
    public static class DispatcherExtension
    {
        /// <summary>
        /// Run <paramref name="agileCallback"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>An <see cref="IAsyncAction"/> indicates the completion of <paramref name="agileCallback"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> or <paramref name="agileCallback"/>
        /// is <see langword="null"/>.</exception>
        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, DispatchedHandler agileCallback)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (agileCallback == null)
                throw new ArgumentNullException(nameof(agileCallback));
            return dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
        }

        /// <summary>
        /// Run <paramref name="agileCallback"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>An <see cref="IAsyncAction"/> indicates the completion of <paramref name="agileCallback"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> or <paramref name="agileCallback"/>
        /// is <see langword="null"/>.</exception>
        public static IAsyncAction RunIdleAsync(this CoreDispatcher dispatcher, DispatchedHandler agileCallback)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (agileCallback == null)
                throw new ArgumentNullException(nameof(agileCallback));
            return dispatcher.RunIdleAsync(i => agileCallback());
        }

        /// <summary>
        /// Begin <paramref name="agileCallback"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> or <paramref name="agileCallback"/>
        /// is <see langword="null"/></exception>
        public static void Begin(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, DispatchedHandler agileCallback)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (agileCallback == null)
                throw new ArgumentNullException(nameof(agileCallback));
            if (priority > CoreDispatcherPriority.High)
                priority = CoreDispatcherPriority.High;
            else if (priority < CoreDispatcherPriority.Idle)
                priority = CoreDispatcherPriority.Idle;
            if (priority == CoreDispatcherPriority.Idle)
                beginIdleCore(dispatcher, a => agileCallback());
            else
                beginCore(dispatcher, priority, agileCallback);
        }

        /// <summary>
        /// Begin <paramref name="agileCallback"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> or <paramref name="agileCallback"/>
        /// is <see langword="null"/></exception>
        public static void Begin(this CoreDispatcher dispatcher, DispatchedHandler agileCallback)
            => Begin(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        /// <summary>
        /// Begin <paramref name="agileCallback"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> or <paramref name="agileCallback"/>
        /// is <see langword="null"/></exception>
        public static void BeginIdle(this CoreDispatcher dispatcher, IdleDispatchedHandler agileCallback)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            beginIdleCore(dispatcher, agileCallback);
        }

        private static async void beginCore(CoreDispatcher dispatcher, CoreDispatcherPriority priority, DispatchedHandler agileCallback)
        {
            await dispatcher.RunAsync(priority, agileCallback);
        }

        private static async void beginIdleCore(CoreDispatcher dispatcher, IdleDispatchedHandler agileCallback)
        {
            await dispatcher.RunIdleAsync(agileCallback);
        }
    }
}
