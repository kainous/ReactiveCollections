﻿using System;
using System.Runtime.CompilerServices;

namespace CSharp.Collections.Monadic.Tasks {
    public class OptionAsyncMethodBuilder<T> {
        public Option<T> Task { get; private set; } =
            Option.None<T>();

        public static OptionAsyncMethodBuilder<T> Create() =>
            new OptionAsyncMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine =>
            stateMachine.MoveNext();

        public void SetResult(T result) =>
           Task = Option.Some(result);

        public void SetException(Exception ex) {
            //Empty
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine {
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine {
        }
    }
}
