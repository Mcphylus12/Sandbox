using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StateMachine
{
    public class StateMachineBuilder<TState, TTransition, TOperationContextData>
    {
        private readonly List<Transition<TState, TTransition, TOperationContextData>> transitions;
        private TState startState;

        public StateMachineBuilder()
        {
            transitions = new List<Transition<TState, TTransition, TOperationContextData>>();
        }

        public StateMachineBuilder<TState, TTransition, TOperationContextData> WithTransition(TState from, TTransition operation, TState to, Func<OperationContext<TState, TTransition, TOperationContextData>, Task> behaviour = null)
        {
            transitions.Add(new ()
            {
                From = from,
                Operation = operation,
                To = to,
                Behaviour = behaviour
            });

            return this;
        }

        public StateMachine<TState, TTransition, TOperationContextData> Build()
        {
            return Build(startState);
        }

        public StateMachine<TState, TTransition, TOperationContextData> Build(TState startState)
        {
            return new StateMachine<TState, TTransition, TOperationContextData>(startState, transitions);
        }

        public StateMachineBuilder<TState, TTransition, TOperationContextData> SetStart(TState start)
        {
            startState = start;
            return this;
        }
    }
}
