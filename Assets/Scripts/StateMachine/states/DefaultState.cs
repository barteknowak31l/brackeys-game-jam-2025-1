using System;
using UnityEngine;

namespace StateMachine.states
{
    // stan poczatkowy
    
    
    public class DefaultState : MonoBehaviour, IBaseState
    {
        private StateMachineManager _ctx;

        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered DefaultState");
        }

        public void UpdateState(StateMachineManager ctx)
        {
            Debug.Log("Updated DefaultState");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ctx.NextState();
            }
        }

        public void ExitState(StateMachineManager ctx)
        {
            Debug.Log("Exited DefaultState");
        }

        public IBaseState SetVariant(Variant variant)
        {
            return this;
        }

        private void OnCollisionEnter(Collision other)
        {
            _ctx.NextState();
        }
    }
}
