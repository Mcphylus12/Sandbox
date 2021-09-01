using System;
using System.Threading.Tasks;

namespace StateMachine
{
    internal class Transition<TState, TTransition, TOperationContextData>
    {
        public TState From { get; internal init; }
        public TTransition Operation { get; internal init; }
        public TState To { get; internal init; }
        public Func<OperationContext<TState, TTransition, TOperationContextData>, Task> Behaviour { get; internal init; }
    }
}
