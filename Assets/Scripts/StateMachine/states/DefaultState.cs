using System;
using System.Collections;
using UnityEngine;

namespace StateMachine.states
{
    public class DefaultState : MonoBehaviour, IBaseState
    {
        [SerializeField] float _stateDuration = 5.0f;
        private StateMachineManager _ctx;

        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered DefaultState");
            _ctx = ctx;
            
            if(!ctx.IsDebugMode)
                    StartCoroutine(ChangeStateCoroutine());
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            Debug.Log("Exited DefaultState");
            StopAllCoroutines();
        }

        public IBaseState SetVariant(Variant variant)
        {
            return this;
        }

        public States GetStateType()
        {
            return States.Default;
        }

        public Variant GetVariant()
        {
            return Variant.First;
        }

        IEnumerator ChangeStateCoroutine()
        {
            yield return new WaitForSeconds(_stateDuration);
            _ctx.NextState();
        }
        
        
    }
}
