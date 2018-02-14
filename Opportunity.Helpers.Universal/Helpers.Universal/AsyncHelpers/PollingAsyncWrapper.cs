using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public static class PollingAsyncWrapper
    {
        public static IAsyncAction Wrap(IAsyncAction action)
            => Wrap(action, 250);

        public static IAsyncAction Wrap(IAsyncAction action, int millisecondsCycle)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (millisecondsCycle < 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondsCycle));
            switch (action.Status)
            {
            case AsyncStatus.Canceled:
                return AsyncAction.CreateCancelled();
            case AsyncStatus.Completed:
                return AsyncAction.CreateCompleted();
            case AsyncStatus.Error:
                return AsyncAction.CreateFault(action.ErrorCode);
            }
            return AsyncInfo.Run(async token =>
            {
                token.Register(action.Cancel);
                while (action.Status == AsyncStatus.Started)
                {
                    await Task.Delay(millisecondsCycle);
                    token.ThrowIfCancellationRequested();
                }
                switch (action.Status)
                {
                case AsyncStatus.Error:
                case AsyncStatus.Completed:
                    action.GetResults();
                    return;
                default:
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException(token);
                }
            });
        }

        public static IAsyncAction Wrap<TProgress>(IAsyncActionWithProgress<TProgress> action)
            => Wrap(action, 250);

        public static IAsyncAction Wrap<TProgress>(IAsyncActionWithProgress<TProgress> action, int millisecondsCycle)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (millisecondsCycle < 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondsCycle));
            switch (action.Status)
            {
            case AsyncStatus.Canceled:
                return AsyncAction.CreateCancelled();
            case AsyncStatus.Completed:
                return AsyncAction.CreateCompleted();
            case AsyncStatus.Error:
                return AsyncAction.CreateFault(action.ErrorCode);
            }
            return AsyncInfo.Run(async token =>
            {
                token.Register(action.Cancel);
                while (action.Status == AsyncStatus.Started)
                {
                    await Task.Delay(millisecondsCycle);
                    token.ThrowIfCancellationRequested();
                }
                switch (action.Status)
                {
                case AsyncStatus.Error:
                case AsyncStatus.Completed:
                    action.GetResults();
                    return;
                default:
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException(token);
                }
            });
        }

        public static IAsyncOperation<T> Wrap<T>(IAsyncOperation<T> operation)
            => Wrap(operation, 250);

        public static IAsyncOperation<T> Wrap<T>(IAsyncOperation<T> operation, int millisecondsCycle)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (millisecondsCycle < 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondsCycle));
            switch (operation.Status)
            {
            case AsyncStatus.Canceled:
                return AsyncOperation<T>.CreateCancelled();
            case AsyncStatus.Completed:
                return AsyncOperation<T>.CreateCompleted(operation.GetResults());
            case AsyncStatus.Error:
                return AsyncOperation<T>.CreateFault(operation.ErrorCode);
            }
            return AsyncInfo.Run(async token =>
            {
                token.Register(operation.Cancel);
                while (operation.Status == AsyncStatus.Started)
                {
                    await Task.Delay(millisecondsCycle);
                    token.ThrowIfCancellationRequested();
                }
                switch (operation.Status)
                {
                case AsyncStatus.Error:
                case AsyncStatus.Completed:
                    return operation.GetResults();
                default:
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException(token);
                }
            });
        }

        public static IAsyncOperation<T> Wrap<T, TProgress>(IAsyncOperationWithProgress<T, TProgress> operation)
            => Wrap(operation, 250);

        public static IAsyncOperation<T> Wrap<T, TProgress>(IAsyncOperationWithProgress<T, TProgress> operation, int millisecondsCycle)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (millisecondsCycle < 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondsCycle));
            switch (operation.Status)
            {
            case AsyncStatus.Canceled:
                return AsyncOperation<T>.CreateCancelled();
            case AsyncStatus.Completed:
                return AsyncOperation<T>.CreateCompleted(operation.GetResults());
            case AsyncStatus.Error:
                return AsyncOperation<T>.CreateFault(operation.ErrorCode);
            }
            return AsyncInfo.Run(async token =>
            {
                token.Register(operation.Cancel);
                while (operation.Status == AsyncStatus.Started)
                {
                    await Task.Delay(millisecondsCycle);
                    token.ThrowIfCancellationRequested();
                }
                switch (operation.Status)
                {
                case AsyncStatus.Error:
                case AsyncStatus.Completed:
                    return operation.GetResults();
                default:
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException(token);
                }
            });
        }
    }
}