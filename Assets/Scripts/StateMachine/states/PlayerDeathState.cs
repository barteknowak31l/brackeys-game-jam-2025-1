using System.Collections;
using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class PlayerDeathState : Observable<PlayerDeathDTO>, IBaseState
    {
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Gracz nie zyje");
        }

        public void ExitState(StateMachineManager ctx)
        {
        }

        public States GetStateType()
        {
            return States.PlayerDeath;
        }

        public Variant GetVariant()
        {
            return Variant.First;
        }

        public IBaseState SetVariant(Variant variant)
        {
            return this;
        }

        public void UpdateState(StateMachineManager ctx)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StateMachineManager.instance.StartState();
            }
        }
        
        
        
    }


}
