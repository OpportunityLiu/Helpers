using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public static class AsExtension
    {
        public static IAsyncAction AsAsyncAction<T>(this IAsyncOperation<T> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncAction.CreateCanceled();
                    else if (operation.Status == AsyncStatus.Error)
                        return AsyncAction.CreateFault(operation.ErrorCode);
                    else
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
                    if (operation.Status == AsyncStatus.Canceled)
                        op.Cancel();
                    else if (operation.Status == AsyncStatus.Error)
                        op.TrySetException(operation.ErrorCode);
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

        public static IAsyncActionWithProgress<TProgress> AsAsyncActionWithProgress<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncAction<TProgress>.CreateCanceled();
                    else if (operation.Status == AsyncStatus.Error)
                        return AsyncAction<TProgress>.CreateFault(operation.ErrorCode);
                    else
                        return AsyncAction<TProgress>.CreateCompleted();
                }
                catch (Exception ex)
                {
                    return AsyncAction<TProgress>.CreateFault(ex);
                }
            }
            var op = new AsyncAction<TProgress>();
            op.RegisterCancellation(operation.Cancel);
            operation.Progress = (s, e) => op.Report(e);
            operation.Completed = (s, e) =>
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        op.Cancel();
                    else if (operation.Status == AsyncStatus.Error)
                        op.TrySetException(operation.ErrorCode);
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

        public static IAsyncOperation<T> AsAsyncOperation<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
            => AsAsyncOperation(operation, null);

        public static IAsyncOperation<T> AsAsyncOperation<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation, IProgress<TProgress> progress)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncOperation<T>.CreateCanceled();
                    else if (operation.Status == AsyncStatus.Error)
                        return AsyncOperation<T>.CreateFault(operation.ErrorCode);
                    else
                        return AsyncOperation<T>.CreateCompleted(operation.GetResults());
                }
                catch (Exception ex)
                {
                    return AsyncOperation<T>.CreateFault(ex);
                }
            }
            var op = new AsyncOperation<T>();
            op.RegisterCancellation(operation.Cancel);
            if (progress != null)
                operation.Progress = (s, p) => progress.Report(p);
            operation.Completed = (s, e) =>
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        op.Cancel();
                    else if (operation.Status == AsyncStatus.Error)
                        op.TrySetException(operation.ErrorCode);
                    else
                        op.TrySetResults(operation.GetResults());
                }
                catch (Exception ex)
                {
                    op.TrySetException(ex);
                }
            };
            return op;
        }

        public static IAsyncAction AsAsyncAction<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
            => AsAsyncAction(operation, null);

        public static IAsyncAction AsAsyncAction<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation, IProgress<TProgress> progress)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (operation.Status != AsyncStatus.Started)
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        return AsyncAction.CreateCanceled();
                    else if (operation.Status == AsyncStatus.Error)
                        return AsyncAction.CreateFault(operation.ErrorCode);
                    else
                        return AsyncAction.CreateCompleted();
                }
                catch (Exception ex)
                {
                    return AsyncAction.CreateFault(ex);
                }
            }
            var op = new AsyncAction();
            op.RegisterCancellation(operation.Cancel);
            if (progress != null)
                operation.Progress = (s, p) => progress.Report(p);
            operation.Completed = (s, e) =>
            {
                try
                {
                    if (operation.Status == AsyncStatus.Canceled)
                        op.Cancel();
                    else if (operation.Status == AsyncStatus.Error)
                        op.TrySetException(operation.ErrorCode);
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

        public static IAsyncAction AsAsyncAction<TProgress>(this IAsyncActionWithProgress<TProgress> action)
            => AsAsyncAction(action, null);

        public static IAsyncAction AsAsyncAction<TProgress>(this IAsyncActionWithProgress<TProgress> action, IProgress<TProgress> progress)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (action.Status != AsyncStatus.Started)
            {
                try
                {
                    if (action.Status == AsyncStatus.Canceled)
                        return AsyncAction.CreateCanceled();
                    else if (action.Status == AsyncStatus.Error)
                        return AsyncAction.CreateFault(action.ErrorCode);
                    else
                        return AsyncAction.CreateCompleted();
                }
                catch (Exception ex)
                {
                    return AsyncAction.CreateFault(ex);
                }
            }
            var op = new AsyncAction();
            op.RegisterCancellation(action.Cancel);
            if (progress != null)
                action.Progress = (s, p) => progress.Report(p);
            action.Completed = (s, e) =>
            {
                try
                {
                    if (action.Status == AsyncStatus.Canceled)
                        op.Cancel();
                    else if (action.Status == AsyncStatus.Error)
                        op.TrySetException(action.ErrorCode);
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
