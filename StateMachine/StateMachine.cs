using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StateMachine
{
    public class StateMachine<TState, TTransition, TOperationContextData>
    {
        private readonly Dictionary<TState, Dictionary<TTransition, TransitionTarget<TState, TTransition, TOperationContextData>>> transitionTree;

        public TState State { get; private set; }
        public bool IsFinished => !transitionTree.ContainsKey(State) || transitionTree[State].Count == 0;

        internal StateMachine(TState startState, List<Transition<TState, TTransition, TOperationContextData>> transitions)
        {
            State = startState;
            this.transitionTree = BuildTransitions(transitions);
        }

        private Dictionary<TState, Dictionary<TTransition, TransitionTarget<TState, TTransition, TOperationContextData>>> BuildTransitions(List<Transition<TState, TTransition, TOperationContextData>> transitions)
        {
            var result = new Dictionary<TState, Dictionary<TTransition, TransitionTarget<TState, TTransition, TOperationContextData>>>();

            foreach (var transition in transitions)
            {
                if (!result.ContainsKey(transition.From))
                {
                    result[transition.From] = new Dictionary<TTransition, TransitionTarget<TState, TTransition, TOperationContextData>>();
                }

                result[transition.From][transition.Operation] = new TransitionTarget<TState, TTransition, TOperationContextData>(transition.To, transition.Behaviour);
            }

            return result;
        }

        public Task Process(TTransition process, TOperationContextData contextData = default)
        {
            if (!transitionTree.ContainsKey(State))
            {
                throw new NotSupportedException("Cannot Transition From this state");
            }
            
            if (!transitionTree[State].ContainsKey(process))
            {
                throw new NotSupportedException("Unsupported transition on this state");
            }

            var target = transitionTree[State][process];
            var oldState = State;
            State = target.State;

            if (target.Behaviour is null) return Task.CompletedTask;

            return target.Behaviour.Invoke(new OperationContext<TState, TTransition, TOperationContextData>(contextData, oldState, process, State));
        } 
    }

    internal struct TransitionTarget<TState, TTransition, TOperationContextData>
    {
        public TState State;
        public Func<OperationContext<TState, TTransition, TOperationContextData>, Task> Behaviour;

        public TransitionTarget(TState state, Func<OperationContext<TState, TTransition, TOperationContextData>, Task> behaviour)
        {
            State = state;
            Behaviour = behaviour;
        }
    }
}
