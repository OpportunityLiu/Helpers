using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public static class CastWrapper
    {
        public static IAsyncOperation<TTo> Cast<TFrom, TTo>(this IAsyncOperation<TFrom> operation)
        {
            if (operation is CastAsyncOperation<TTo, TFrom> c)
                return c.Operation;
            return new CastAsyncOperation<TFrom, TTo>(operation);
        }

        public static IAsyncOperationWithProgress<TTo, TProgress> Cast<TFrom, TTo, TProgress>(this IAsyncOperationWithProgress<TFrom, TProgress> operation)
        {
            if (operation is CastAsyncOperation<TTo, TFrom, TProgress> c)
                return c.Operation;
            return new CastAsyncOperation<TFrom, TTo, TProgress>(operation);
        }

        public static IAsyncAction AsAsyncAction<T>(this IAsyncOperation<T> operation)
        {
            return new CastAsyncAction<T>(operation);
        }

        public static IAsyncActionWithProgress<TProgress> AsAsyncAction<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
        {
            return new CastAsyncAction<T, TProgress>(operation);
        }
    }
}
