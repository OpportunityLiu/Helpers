using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    /// <summary>
    /// Wraps <see cref="IAsyncInfo"/> to make their delegates can be set more than once.
    /// </summary>
    public static class MulticastWrapper
    {
        /// <summary>
        /// Wraps <see cref="IAsyncAction"/> to make its delegates can be set more than once.
        /// </summary>
        /// <param name="action"><see cref="IAsyncAction"/> to wrap.</param>
        /// <returns>Warpped multicastable <see cref="IAsyncAction"/></returns>
        public static IAsyncAction AsMulticast(this IAsyncAction action)
        {
            if (action is MulticastAsyncAction mu)
                return mu;
            return new MulticastAsyncAction(action);
        }

        /// <summary>
        /// Wraps <see cref="IAsyncActionWithProgress{TProgress}"/> to make its delegates can be set more than once.
        /// </summary>
        /// <param name="action"><see cref="IAsyncActionWithProgress{TProgress}"/> to wrap.</param>
        /// <returns>Warpped multicastable <see cref="IAsyncActionWithProgress{TProgress}"/></returns>
        public static IAsyncActionWithProgress<TProgress> AsMulticast<TProgress>(this IAsyncActionWithProgress<TProgress> action)
        {
            if (action is MulticastAsyncAction<TProgress> mu)
                return mu;
            return new MulticastAsyncAction<TProgress>(action);
        }

        /// <summary>
        /// Wraps <see cref="IAsyncOperation{TResult}"/> to make its delegates can be set more than once.
        /// </summary>
        /// <param name="operation"><see cref="IAsyncOperation{TResult}"/> to wrap.</param>
        /// <returns>Warpped multicastable <see cref="IAsyncOperation{TResult}"/></returns>
        public static IAsyncOperation<T> AsMulticast<T>(this IAsyncOperation<T> operation)
        {
            if (operation is MulticastAsyncOperation<T> mu)
                return mu;
            return new MulticastAsyncOperation<T>(operation);
        }

        /// <summary>
        /// Wraps <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/> to make its delegates can be set more than once.
        /// </summary>
        /// <param name="operation"><see cref="IAsyncOperationWithProgress{TResult, TProgress}"/> to wrap.</param>
        /// <returns>Warpped multicastable <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/></returns>
        public static IAsyncOperationWithProgress<T, TProgress> AsMulticast<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
        {
            if (operation is MulticastAsyncOperation<T, TProgress> mu)
                return mu;
            return new MulticastAsyncOperation<T, TProgress>(operation);
        }
    }
}
