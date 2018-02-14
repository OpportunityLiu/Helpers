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
    public static class DispatcherAwaitableExtension
    {
        #region RunAsyncAction

        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncAction> agileCallback)
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
                return runIdleAsyncCore(dispatcher, agileCallback);
            return runAsyncCore(dispatcher, priority, agileCallback);
        }

        public static IAsyncAction RunIdleAsync(this CoreDispatcher dispatcher, Func<IAsyncAction> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, Func<IAsyncAction> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        private static IAsyncAction runAsyncCore(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncAction> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunAsync(priority, () =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                var task = agileCallback();
                r.RegisterCancellation(task.Cancel);
                task.Completed = (s, e) =>
                {
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        s.GetResults();
                        r.SetResults();
                        break;
                    case AsyncStatus.Error:
                        r.SetException(s.ErrorCode);
                        break;
                    }
                };
            });
            return r;
        }

        private static IAsyncAction runIdleAsyncCore(this CoreDispatcher dispatcher, Func<IAsyncAction> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunIdleAsync(args =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                var task = agileCallback();
                r.RegisterCancellation(task.Cancel);
                task.Completed = (s, e) =>
                {
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        s.GetResults();
                        r.SetResults();
                        break;
                    case AsyncStatus.Error:
                        r.SetException(s.ErrorCode);
                        break;
                    }
                };
            });
            return r;
        }

        #endregion RunAsyncAction

        #region RunAsyncOperation

        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncOperation<T>> agileCallback)
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
                return runIdleAsyncCore(dispatcher, agileCallback);
            return runAsyncCore(dispatcher, priority, agileCallback);
        }

        public static IAsyncOperation<T> RunIdleAsync<T>(this CoreDispatcher dispatcher, Func<IAsyncOperation<T>> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, Func<IAsyncOperation<T>> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        private static IAsyncOperation<T> runAsyncCore<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncOperation<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunAsync(priority, () =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                var task = agileCallback();
                r.RegisterCancellation(task.Cancel);
                task.Completed = (s, e) =>
                {
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        r.SetResults(s.GetResults());
                        break;
                    case AsyncStatus.Error:
                        r.SetException(s.ErrorCode);
                        break;
                    }
                };
            });
            return r;
        }

        private static IAsyncOperation<T> runIdleAsyncCore<T>(this CoreDispatcher dispatcher, Func<IAsyncOperation<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunIdleAsync(args =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                var task = agileCallback();
                r.RegisterCancellation(task.Cancel);
                task.Completed = (s, e) =>
                {
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        r.SetResults(s.GetResults());
                        break;
                    case AsyncStatus.Error:
                        r.SetException(s.ErrorCode);
                        break;
                    }
                };
            });
            return r;
        }

        #endregion RunAsyncOperation

        #region RunAsyncResult

        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<T> agileCallback)
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
                return runIdleAsyncCore(dispatcher, agileCallback);
            return runAsyncCore(dispatcher, priority, agileCallback);
        }

        public static IAsyncOperation<T> RunIdleAsync<T>(this CoreDispatcher dispatcher, Func<T> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, Func<T> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        private static IAsyncOperation<T> runAsyncCore<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<T> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunAsync(priority, () =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                try
                {
                    var result = agileCallback();
                    r.SetResults(result);
                }
                catch (Exception ex)
                {
                    r.SetException(ex);
                }
            });
            return r;
        }

        private static IAsyncOperation<T> runIdleAsyncCore<T>(this CoreDispatcher dispatcher, Func<T> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunIdleAsync(args =>
            {
                if (r.Status == AsyncStatus.Canceled)
                    return;
                try
                {
                    var result = agileCallback();
                    r.SetResults(result);
                }
                catch (Exception ex)
                {
                    r.SetException(ex);
                }
            });
            return r;
        }

        #endregion RunAsyncResult
    }
}
