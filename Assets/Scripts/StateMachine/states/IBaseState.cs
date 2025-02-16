namespace StateMachine.states
{
    public interface IBaseState 
    {
        void EnterState(StateMachineManager ctx);
        void UpdateState(StateMachineManager ctx);
        void ExitState(StateMachineManager ctx);
        
        IBaseState SetVariant(Variant variant);
        
    }
}
