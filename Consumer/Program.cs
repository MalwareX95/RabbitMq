using Consumer.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Consumer.Pipes;

namespace Consumer
{

    public interface ICommand { }

    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }

    public class RegisterCommand : ICommand
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }


    class AccountService
    {
        //public void Register(RegisterCommand registerCommand)
        //{

        //}

        public int Register(int z)
        {
            return 2;
        }

        //public void Register()
        //{
        //    throw new NotImplementedException();
        //}
    }


    public interface IPipe
    {
        void Handle(object[] @params, MethodInfo methodInfo, Action next);
    }

    public interface IPipe<TResult>
    {
        TResult Handle(object[] @params, MethodInfo methodInfo, Func<TResult> next);
    }

    interface _IDecorator<T>
        where T : struct, ITuple
    {
        void Handle(T @params, MethodInfo methodInfo, Action<T> next);
    }

    class _Decorator5 : _IDecorator<(int, int)>
    {
        public void Handle((int, int) @params, MethodInfo methodInfo, Action<(int, int)> next)
        {
        }
    }

    class _Decorator1 : IPipe
    {
        public void Handle(object[] @params, MethodInfo methodInfo, Action next)
        {
            //on before 
            next();
            //throw new NotImplementedException();
        }
    }

    class _Decorator2 : IPipe
    {
        public void Handle(object[] @params, MethodInfo methodInfo, Action next)
        {
            //throw new NotImplementedException();
        }
    }

    class _Decorator3 : IPipe<int>
    {
        public int Handle(object[] @params, MethodInfo methodInfo, Func<int> next)
        {
            var x = next();
            return 3;
        }
    }

    class _Decorator4 : IPipe<int>
    {
        public int Handle(object[] @params, MethodInfo methodInfo, Func<int> next)
        {
            var x = next();
            return 4 + x;
        }
    }

    interface IPipeline
    {
        void Execute(IActionInvoker actionInvoker);
    }

    interface IPipeline<TResult>
    {
        TResult Execute(IActionInvoker<TResult> actionInvoker);
    }

    interface IPipelineRunner
    {
        void To<T>() where T : PipelineBase, new();
    }

    interface IPipelineRunner<TResult>
    {
        TResult To<T>() where T : PipelineBase<TResult>, new();
    }

    static class Pipes
    {
        class PipelineRunner<TResult> : IPipelineRunner<TResult>
        {
            private readonly IActionInvoker<TResult> actionInvoker;

            public PipelineRunner(IActionInvoker<TResult> actionInvoker)
            {
                this.actionInvoker = actionInvoker;
            }

            public TResult To<T>() where T : PipelineBase<TResult>, new()
                => new T().Execute(actionInvoker);
        }

        class PipelineRunner : IPipelineRunner
        {
            private readonly IActionInvoker actionInvoker;

            public PipelineRunner(IActionInvoker actionInvoker)
            {
                this.actionInvoker = actionInvoker;
            }

            public void To<T>() where T : PipelineBase, new()
                => new T().Execute(actionInvoker);
        }

        public static IPipelineRunner Pipe<TParam>(Action<TParam> @delegate, TParam param)
            => new PipelineRunner(new ActionInvoker<TParam>(@delegate, param));

        public static IPipelineRunner<TResult> Pipe<TParam, TResult>(Func<TParam, TResult> @delegate, TParam param)
            => new PipelineRunner<TResult>(new FuncInvoker<TParam, TResult>(@delegate, param));

    }

    class LogPipe : PipelineBase<int>
    {
        public LogPipe() : 
            base(new _Decorator4(),
                 new _Decorator3())
        {}
    }


    abstract class PipelineBase<TResult> : IPipeline<TResult>
    {
        private readonly IEnumerable<IPipe<TResult>> pipes;

        public PipelineBase(params IPipe<TResult>[] pipes)
        {
            this.pipes = pipes;
        }

        public TResult Execute(IActionInvoker<TResult> actionInvoker)
        {
            var enumerator = pipes.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current.Handle(actionInvoker.Parameters, actionInvoker.MethodInfo, next);

            TResult next()
            {
                if (!enumerator.MoveNext())
                {
                   return actionInvoker.Invoke();
                    //service.Register(2);
                }
                else
                {
                    return enumerator.Current.Handle(actionInvoker.Parameters, actionInvoker.MethodInfo, next);
                }
            };
        }
    }

    abstract class PipelineBase : IPipeline
    {
        private readonly IEnumerable<IPipe> pipes;

        public PipelineBase(IEnumerable<IPipe> pipes)
        {
            this.pipes = pipes;
        }

        public void Execute(IActionInvoker actionInvoker)
        {
            var enumerator = pipes.GetEnumerator();
            enumerator.MoveNext();

            enumerator.Current.Handle(actionInvoker.Parameters, actionInvoker.MethodInfo, next);

            void next()
            {
                if (!enumerator.MoveNext())
                {
                    actionInvoker.Invoke();
                    //service.Register(2);
                }
                else
                {
                    enumerator.Current.Handle(actionInvoker.Parameters, actionInvoker.MethodInfo, next);
                }
            };
        }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            var service = new AccountService();

            var result = Pipe(service.Register, 2).To<LogPipe>();

            //var x = (1);
            //IEnumerable<IPipe<int>> decorators = new IPipe<int>[]
            //{
            //    new _Decorator3(),
            //    new _Decorator4()
            //};

            //Decorate(service.Register, 2).With<>();
            //PipeInvoker.Pipe(service.Register, 1).To<>
            //Pipe(service.Register, 1).To<LogPipe>();

        }
    }
}
