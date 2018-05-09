﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Opportunity.Helpers.Universal.AsyncHelpers
{

    public struct AsyncActionMethodBuilder
    {
        public static AsyncActionMethodBuilder Create() => default;

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        public void SetException(Exception exception) => Task.TrySetException(exception);
        public void SetResult() => Task.TrySetResults();
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        private AsyncAction task;
        public AsyncAction Task
        {
            get
            {
                var t = this.task;
                if (t is null)
                {
                    t = new AsyncAction();
                    this.task = t;
                }
                return t;
            }
        }
    }
    public struct AsyncOperationMethodBuilder<T>
    {
        public static AsyncOperationMethodBuilder<T> Create() => default;

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        public void SetException(Exception exception) => Task.TrySetException(exception);
        public void SetResult(T result) => Task.TrySetResults(result);
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        private AsyncOperation<T> task;
        public AsyncOperation<T> Task
        {
            get
            {
                var t = this.task;
                if (t is null)
                {
                    t = new AsyncOperation<T>();
                    this.task = t;
                }
                return t;
            }
        }
    }
}