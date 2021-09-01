namespace StateMachine
{
    public class OperationContext<TState, TTransition, TOperationContextData>
    {
        public TOperationContextData Data { get; }
        public TState OldState { get; }
        public TTransition Transition { get; }
        public TState NewState { get; }

        public OperationContext(TOperationContextData contextData, TState oldState, TTransition process, TState newState)
        {
            this.Data = contextData;
            this.OldState = oldState;
            this.Transition = process;
            this.NewState = newState;
        }
    }
}