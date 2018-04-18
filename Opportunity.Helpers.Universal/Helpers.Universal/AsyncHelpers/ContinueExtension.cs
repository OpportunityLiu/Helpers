using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// ContinueWith adapters for <see cref="IAsyncInfo"/>
    /// </summary>
    public static class ContinueExtension
    {
        public static IAsyncOperation<TTo> ContinueWith<TFrom, TTo>(this IAsyncOperation<TFrom> operation, Func<IAsyncOperation<TFrom>, TTo> continuation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    var r = continuation(operation);
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncOperation<TTo>.CreateCanceled();
                    return AsyncOperation<TTo>.CreateCompleted(r);
                }
                catch (Exception ex)
                {
                    return AsyncOperation<TTo>.CreateFault(ex);
                }
            }
            var op = new AsyncOperation<TTo>();
            op.RegisterCancellation(operation.Cancel);
            operation.Completed = (s, e) =>
            {
                try
                {
                    var r = continuation(s);
                    if (operation.Status == AsyncStatus.Canceled)
                        op.TrySetCanceled();
                    else
                        op.TrySetResults(r);
                }
                catch (Exception ex)
                {
                    op.TrySetException(ex);
                }
            };
            return op;
        }

        public static IAsyncOperationWithProgress<TTo, TProgress> ContinueWith<TFrom, TTo, TProgress>(this IAsyncOperationWithProgress<TFrom, TProgress> operation, Func<IAsyncOperationWithProgress<TFrom, TProgress>, TTo> continuation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    var r = continuation(operation);
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncOperation<TTo, TProgress>.CreateCanceled();
                    return AsyncOperation<TTo, TProgress>.CreateCompleted(r);
                }
                catch (Exception ex)
                {
                    return AsyncOperation<TTo, TProgress>.CreateFault(ex);
                }
            }
            var op = new AsyncOperation<TTo, TProgress>();
            op.RegisterCancellation(operation.Cancel);
            operation.Progress = (s, p) => op.Report(p);
            operation.Completed = (s, e) =>
            {
                try
                {
                    var r = continuation(s);
                    if (operation.Status == AsyncStatus.Canceled)
                        op.TrySetCanceled();
                    else
                        op.TrySetResults(r);
                }
                catch (Exception ex)
                {
                    op.TrySetException(ex);
                }
            };
            return op;
        }

        public static IAsyncAction ContinueWith<T>(this IAsyncOperation<T> operation, Action<IAsyncOperation<T>> continuation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    continuation(operation);
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncAction.CreateCanceled();
                    return AsyncAction.CreateCompleted();
                }
                catch (Exception ex)
                {
                    return AsyncAction.CreateFault(ex);
                }
            }
            var op = new AsyncAction();
            op.RegisterCancellation(operation.Cancel);
            operation.Completed = (s, e) =>
            {
                try
                {
                    continuation(s);
                    if (operation.Status == AsyncStatus.Canceled)
                        op.TrySetCanceled();
                    else
                        op.TrySetResults();
                }
                catch (Exception ex)
                {
                    op.TrySetException(ex);
                }
            };
            return op;
        }

        public static IAsyncActionWithProgress<TProgress> ContinueWith<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation, Action<IAsyncOperationWithProgress<T, TProgress>> continuation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    continuation(operation);
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncAction<TProgress>.CreateCanceled();
                    return AsyncAction<TProgress>.CreateCompleted();
                }
                catch (Exception ex)
                {
                    return AsyncAction<TProgress>.CreateFault(ex);
                }
            }
            var op = new AsyncAction<TProgress>();
            op.RegisterCancellation(operation.Cancel);
            operation.Progress = (s, p) => op.Report(p);
            operation.Completed = (s, e) =>
            {
                try
                {
                    continuation(s);
                    if (e == AsyncStatus.Canceled)
                        op.TrySetCanceled();
                    else
                        op.TrySetResults();
                }
                catch (Exception ex)
                {
                    op.TrySetException(ex);
                }
            };
            return op;
        }
    }
}
