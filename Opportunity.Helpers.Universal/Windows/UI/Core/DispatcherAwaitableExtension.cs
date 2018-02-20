using Opportunity.Helpers.Universal.AsyncHelpers;
using System;
using Windows.Foundation;

namespace Windows.UI.Core
{
    /// <summary>
    /// Extension methods to run <see cref="Func{TResult}"/>, <see cref="IAsyncAction"/> and <see cref="IAsyncOperation{TResult}"/>
    /// on UI thread.
    /// </summary>
    public static class DispatcherAwaitableExtension
    {
        #region RunAsyncAction

        /// <summary>
        /// Run an <see cref="IAsyncAction"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="IAsyncAction"/> of <paramref name="agileCallback"/> completing</returns>
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

        /// <summary>
        /// Run an <see cref="IAsyncAction"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="IAsyncAction"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncAction RunIdleAsync(this CoreDispatcher dispatcher, Func<IAsyncAction> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        /// <summary>
        /// Run an <see cref="IAsyncAction"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="IAsyncAction"/> of <paramref name="agileCallback"/> completing</returns>
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

        /// <summary>
        /// Run an <see cref="IAsyncOperation{TResult}"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="IAsyncOperation{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
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

        /// <summary>
        /// Run an <see cref="IAsyncOperation{TResult}"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="IAsyncOperation{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncOperation<T> RunIdleAsync<T>(this CoreDispatcher dispatcher, Func<IAsyncOperation<T>> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        /// <summary>
        /// Run an <see cref="IAsyncOperation{TResult}"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="IAsyncOperation{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
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

        /// <summary>
        /// Run an <see cref="Func{TResult}"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on <paramref name="agileCallback"/> completing</returns>
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

        /// <summary>
        /// Run an <see cref="Func{TResult}"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on <paramref name="agileCallback"/> completing</returns>
        public static IAsyncOperation<T> RunIdleAsync<T>(this CoreDispatcher dispatcher, Func<T> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        /// <summary>
        /// Run an <see cref="Func{TResult}"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="IAsyncOperation{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on <paramref name="agileCallback"/> completing</returns>
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
