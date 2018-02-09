using System;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public static class AsyncWrapper
    {
        public static IAsyncAction CreateCompleted()
            => new CompletedAsyncAction(AsyncStatus.Completed, null);

        public static IAsyncActionWithProgress<TProgress> CreateCompletedWithProgress<TProgress>()
            => new CompletedAsyncAction<TProgress>(AsyncStatus.Completed, null);

        public static IAsyncOperation<TResult> CreateCompleted<TResult>(TResult result)
            => new CompletedAsyncOperation<TResult>(AsyncStatus.Completed, result, null);

        public static IAsyncOperationWithProgress<TResult, TProgress> CreateCompletedWithProgress<TResult, TProgress>(TResult result)
            => new CompletedAsyncOperation<TResult, TProgress>(AsyncStatus.Completed, result, null);

        public static IAsyncAction CreateError(Exception error)
            => new CompletedAsyncAction(AsyncStatus.Error, error ?? throw new ArgumentNullException(nameof(error)));

        public static IAsyncActionWithProgress<TProgress> CreateErrorWithProgress<TProgress>(Exception error)
            => new CompletedAsyncAction<TProgress>(AsyncStatus.Error, error ?? throw new ArgumentNullException(nameof(error)));

        public static IAsyncOperation<TResult> CreateError<TResult>(Exception error)
            => new CompletedAsyncOperation<TResult>(AsyncStatus.Error, default, error ?? throw new ArgumentNullException(nameof(error)));

        public static IAsyncOperationWithProgress<TResult, TProgress> CreateErrorWithProgress<TResult, TProgress>(Exception error)
            => new CompletedAsyncOperation<TResult, TProgress>(AsyncStatus.Error, default, error ?? throw new ArgumentNullException(nameof(error)));

        public static IAsyncAction CreateCanceled()
            => new CompletedAsyncAction(AsyncStatus.Canceled, new OperationCanceledException());

        public static IAsyncActionWithProgress<TProgress> CreateCanceledWithProgress<TProgress>()
            => new CompletedAsyncAction<TProgress>(AsyncStatus.Canceled, new OperationCanceledException());

        public static IAsyncOperation<TResult> CreateCanceled<TResult>()
            => new CompletedAsyncOperation<TResult>(AsyncStatus.Canceled, default, new OperationCanceledException());

        public static IAsyncOperationWithProgress<TResult, TProgress> CreateCanceledWithProgress<TResult, TProgress>()
            => new CompletedAsyncOperation<TResult, TProgress>(AsyncStatus.Canceled, default, new OperationCanceledException());
    }
}
