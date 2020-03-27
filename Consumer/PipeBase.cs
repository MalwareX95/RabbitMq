using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consumer
{
    public abstract class PipeBase
    {
        private readonly IPipe[] decorators;

        public PipeBase(IPipe[] decorator)
        {
            this.decorators = decorator ?? new IPipe[] {};
        }

        public void OnBeforeExecute(object[] args)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnBeforeExecute(args, null);
            }
        }

        public void OnAfterExecute(object[] args)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnAfterExecute(args, null);
            }
        }
    }


    public abstract class PipeBase<TParam>
    {
        private readonly IPipe<TParam>[] decorators;

        public PipeBase(IPipe<TParam>[] decorator)
        {
            this.decorators = decorator;
        }

        public void OnBeforeExecute(TParam param)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnBeforeExecute(param, null);
            }
        }

        public void OnAfterExecute(TParam param)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnAfterExecute(param, null);
            }
        }
    }

    public abstract class PipeBase<TParam1, TParam2>
    {
        private readonly IPipe<TParam1>[] decorators;

        public PipeBase(IPipe<TParam1>[] decorator)
        {
            this.decorators = decorator;
        }

        public void OnBeforeExecute(TParam1 param)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnBeforeExecute(param, null);
            }
        }

        public void OnAfterExecute(TParam1 param)
        {
            foreach (var decorator in decorators)
            {
                //decorator.OnAfterExecute(param, null);
            }
        }
    }


}
