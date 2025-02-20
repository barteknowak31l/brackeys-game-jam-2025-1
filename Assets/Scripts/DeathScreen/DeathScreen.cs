using Observers;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace DeathScreen
{
    public class DeathScreen : MonoBehaviour, IObserver<StateDTO>
    {
        Animator _animator;
        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _animator.Play("idle");
            StateMachineManager.instance.AddObserver(this);
            
        }

        private void OnDisable()
        {
            StateMachineManager.instance.RemoveObserver(this);
        }


        public void OnNotify(StateDTO dto)
        {
            if (dto._state == States.PlayerDeath)
            {
                _animator.Play("fade_in");
            }
        }

        public void OnAnimationEnd()
        {
            StateMachineManager.instance.StartState();
        }
        
    }
}
