using System.Collections;
using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class PlayerDeathState : Observable<PlayerDeathDTO>, IBaseState
    {
        [SerializeField] private float _resetDelay = 3.0f;
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Gracz nie zyje");
            StartCoroutine(ChangeToStartState());
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

        IEnumerator ChangeToStartState()
        {
            yield return new WaitForSeconds(_resetDelay);
            StateMachineManager.instance.StartState();
        }
        
        
    }


}
