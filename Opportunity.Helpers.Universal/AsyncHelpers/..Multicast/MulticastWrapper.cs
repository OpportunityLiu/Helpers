﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{
    public static class MulticastWrapper
    {
        public static IAsyncAction AsMulticast(this IAsyncAction action)
        {
            if (action is MulticastAsyncAction mu)
                return mu;
            return new MulticastAsyncAction(action);
        }

        public static IAsyncActionWithProgress<TProgress> AsMulticast<TProgress>(this IAsyncActionWithProgress<TProgress> action)
        {
            if (action is MulticastAsyncAction<TProgress> mu)
                return mu;
            return new MulticastAsyncAction<TProgress>(action);
        }

        public static IAsyncOperation<T> AsMulticast<T>(this IAsyncOperation<T> operation)
        {
            if (operation is MulticastAsyncOperation<T> mu)
                return mu;
            return new MulticastAsyncOperation<T>(operation);
        }

        public static IAsyncOperationWithProgress<T, TProgress> AsMulticast<T, TProgress>(this IAsyncOperationWithProgress<T, TProgress> operation)
        {
            if (operation is MulticastAsyncOperation<T, TProgress> mu)
                return mu;
            return new MulticastAsyncOperation<T, TProgress>(operation);
        }
    }
}
