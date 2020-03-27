using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Consumer
{
    //struct ActionInvoker : IActionInvoker
    //{
    //    private readonly Delegate @delegate;

    //    public ActionInvoker(Delegate @delegate, params object[] parameters)
    //    {
    //        Parameters = parameters;
    //        this.@delegate = @delegate;
    //    }

    //    public object[] Parameters { get; }



    //    public void Invoke() => @delegate.DynamicInvoke(Parameters);
    //}

    class ActionInvoker<TParam> : IActionInvoker
    {
        private readonly Action<TParam> action;
        private readonly TParam param;

        public object[] Parameters { get; }

        public MethodInfo MethodInfo { get; }

        public ActionInvoker(Action<TParam> action, TParam param)
        {
            this.action = action;
            this.param = param;
        }
        public void Invoke() => action(param);
    }

    class FuncInvoker<TParam, TResult> : IActionInvoker<TResult>
    {
        private readonly Func<TParam, TResult> func;
        private readonly TParam param;

        public FuncInvoker(Func<TParam, TResult> func, TParam param)
        {
            this.func = func;
            this.param = param;
        }

        public MethodInfo MethodInfo { get; }

        public object[] Parameters { get; }

        public TResult Invoke() => func(param);
    }
}
