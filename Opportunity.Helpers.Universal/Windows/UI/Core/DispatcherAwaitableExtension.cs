using Opportunity.Helpers.Universal.AsyncHelpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.UI.Core
{
    /// <summary>
    /// Extension methods to run <see cref="Func{TResult}"/>, <see cref="IAsyncAction"/> and <see cref="IAsyncOperation{TResult}"/>
    /// on UI thread.
    /// </summary>
    public static class DispatcherAwaitableExtension
    {
        #region RunTask

        /// <summary>
        /// Run an <see cref="Task"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="Task"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<Task> agileCallback)
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
        /// Run an <see cref="Task"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="Task"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncAction RunIdleAsync(this CoreDispatcher dispatcher, Func<Task> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        /// <summary>
        /// Run an <see cref="Task"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <returns>an <see cref="IAsyncAction"/> that will complete
        /// on returned <see cref="Task"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncAction RunAsync(this CoreDispatcher dispatcher, Func<Task> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        private static void core(AsyncAction returns, Func<Task> agileCallback)
        {
            if (returns.Status == AsyncStatus.Canceled)
                return;
            try
            {
                var task = agileCallback();
                task.ContinueWith((t, o) =>
                {
                    var action = (AsyncAction)o;
                    if (action.Status == AsyncStatus.Canceled)
                        return;
                    if (t.IsFaulted)
                        action.TrySetException(t.Exception);
                    else if (t.IsCanceled)
                        action.Cancel();
                    else
                        action.TrySetResults();
                }, returns);
            }
            catch (Exception ex)
            {
                returns.TrySetException(ex);
            }
        }

        private static IAsyncAction runAsyncCore(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<Task> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunAsync(priority, () => core(r, agileCallback));
            return r;
        }

        private static IAsyncAction runIdleAsyncCore(this CoreDispatcher dispatcher, Func<Task> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunIdleAsync(i => core(r, agileCallback));
            return r;
        }

        #endregion RunTask

        #region RunTask<T>

        /// <summary>
        /// Run an <see cref="Task{TResult}"/> on UI thread.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="priority">priority of execution</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="Task{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="Task{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<Task<T>> agileCallback)
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
        /// Run an <see cref="Task{TResult}"/> on UI thread with idle priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="Task{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="Task{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncOperation<T> RunIdleAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Idle, agileCallback);

        /// <summary>
        /// Run an <see cref="Task{TResult}"/> on UI thread with normal priority.
        /// </summary>
        /// <param name="dispatcher"><see cref="CoreDispatcher"/> to run <paramref name="agileCallback"/> on</param>
        /// <param name="agileCallback">callback to execute</param>
        /// <typeparam name="T">result type of <see cref="Task{TResult}"/></typeparam>
        /// <returns>an <see cref="IAsyncOperation{TResult}"/> that will complete
        /// on returned <see cref="Task{TResult}"/> of <paramref name="agileCallback"/> completing</returns>
        public static IAsyncOperation<T> RunAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> agileCallback)
            => RunAsync(dispatcher, CoreDispatcherPriority.Normal, agileCallback);

        private static void core<T>(AsyncOperation<T> returns, Func<Task<T>> agileCallback)
        {
            if (returns.Status == AsyncStatus.Canceled)
                return;
            try
            {
                var task = agileCallback();
                task.ContinueWith((t, o) =>
                {
                    var action = (AsyncOperation<T>)o;
                    if (action.Status == AsyncStatus.Canceled)
                        return;
                    if (t.IsFaulted)
                        action.TrySetException(t.Exception);
                    else if (t.IsCanceled)
                        action.Cancel();
                    else
                        action.TrySetResults(t.Result);
                }, returns);
            }
            catch (Exception ex)
            {
                returns.TrySetException(ex);
            }
        }

        private static IAsyncOperation<T> runAsyncCore<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<Task<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunAsync(priority, () => core(r, agileCallback));
            return r;
        }

        private static IAsyncOperation<T> runIdleAsyncCore<T>(this CoreDispatcher dispatcher, Func<Task<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunIdleAsync(i => core(r, agileCallback));
            return r;
        }

        #endregion RunTask<T>

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

        private static void core(AsyncAction returns, Func<IAsyncAction> agileCallback)
        {
            if (returns.Status == AsyncStatus.Canceled)
                return;
            try
            {
                var task = agileCallback();
                returns.RegisterCancellation(task.Cancel);
                task.Completed = (sender, e) =>
                {
                    if (returns.Status == AsyncStatus.Canceled)
                    {
                        sender.Close();
                        return;
                    }
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        sender.GetResults();
                        returns.TrySetResults();
                        break;
                    case AsyncStatus.Error:
                        returns.TrySetException(sender.ErrorCode);
                        break;
                    case AsyncStatus.Canceled:
                        returns.Cancel();
                        break;
                    }
                };
            }
            catch (Exception ex)
            {
                returns.TrySetException(ex);
            }
        }

        private static IAsyncAction runAsyncCore(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncAction> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunAsync(priority, () => core(r, agileCallback));
            return r;
        }

        private static IAsyncAction runIdleAsyncCore(this CoreDispatcher dispatcher, Func<IAsyncAction> agileCallback)
        {
            var r = new AsyncAction();
            var ignore = dispatcher.RunIdleAsync(args => core(r, agileCallback));
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

        private static void core<T>(AsyncOperation<T> returns, Func<IAsyncOperation<T>> agileCallback)
        {
            if (returns.Status == AsyncStatus.Canceled)
                return;
            try
            {
                var task = agileCallback();
                returns.RegisterCancellation(task.Cancel);
                task.Completed = (sender, e) =>
                {
                    if (returns.Status == AsyncStatus.Canceled)
                    {
                        sender.Close();
                        return;
                    }
                    switch (e)
                    {
                    case AsyncStatus.Completed:
                        returns.TrySetResults(sender.GetResults());
                        break;
                    case AsyncStatus.Error:
                        returns.TrySetException(sender.ErrorCode);
                        break;
                    case AsyncStatus.Canceled:
                        returns.Cancel();
                        break;
                    }
                };
            }
            catch (Exception ex)
            {
                returns.TrySetException(ex);
            }
        }

        private static IAsyncOperation<T> runAsyncCore<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<IAsyncOperation<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunAsync(priority, () => core(r, agileCallback));
            return r;
        }

        private static IAsyncOperation<T> runIdleAsyncCore<T>(this CoreDispatcher dispatcher, Func<IAsyncOperation<T>> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunIdleAsync(i => core(r, agileCallback));
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

        private static void core<T>(AsyncOperation<T> returns, Func<T> agileCallback)
        {
            if (returns.Status == AsyncStatus.Canceled)
                return;
            try
            {
                returns.TrySetResults(agileCallback());
            }
            catch (Exception ex)
            {
                returns.TrySetException(ex);
            }
        }

        private static IAsyncOperation<T> runAsyncCore<T>(this CoreDispatcher dispatcher, CoreDispatcherPriority priority, Func<T> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunAsync(priority, () => core(r, agileCallback));
            return r;
        }

        private static IAsyncOperation<T> runIdleAsyncCore<T>(this CoreDispatcher dispatcher, Func<T> agileCallback)
        {
            var r = new AsyncOperation<T>();
            var ignore = dispatcher.RunIdleAsync(i => core(r, agileCallback));
            return r;
        }

        #endregion RunAsyncResult
    }
}
