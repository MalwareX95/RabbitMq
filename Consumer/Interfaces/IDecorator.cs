using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Consumer.Interfaces
{
    public interface IDecorator
    {
        void OnBeforeExecute(object[] args, MethodInfo methodInfo);
        void OnAfterExecute(object[] args, MethodInfo methodInfo);
    }

    public interface IDecorator<TParam>
    {
        void OnBeforeExecute(TParam value, MethodInfo methodInfo);
        void OnAfterExecute(TParam value, MethodInfo methodInfo);
    }

    public interface ICallPipe<TParam, TResult>
    {
        TResult To<TPipe, TActionInvoker>() where TPipe : PipeBase<TParam>, new();
    }

    public interface ICallPipe<TParam>
    {
        void To<TPipe>() where TPipe : PipeBase<TParam>, new();
    }

    public interface ICallPipe
    {
        void To<TPipe>() where TPipe : PipeBase, new();
    }


    public interface IDelegateInfo
    {
        MethodInfo MethodInfo { get; }
        object[] Parameters { get; }
    }

    public interface IActionInvoker : IDelegateInfo
    {
        void Invoke();
    }

    public interface IActionInvoker<TResult> : IDelegateInfo
    {
        public TResult Invoke();
    }

}
