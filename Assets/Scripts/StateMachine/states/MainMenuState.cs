using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class MainMenuState : MonoBehaviour, IBaseState
    {
        private Variant _variant;
        public void EnterState(StateMachineManager ctx)
        {
        
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.MainMenuState;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
    }
}
