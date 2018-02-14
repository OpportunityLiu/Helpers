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
    public static class DispatcherExtension
    {
        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, DispatchedHandler agileCallback)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            return dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
        }

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

        public static void Begin(this CoreDispatcher dispatcher, DispatchedHandler agileCallback)
            => Begin(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

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
