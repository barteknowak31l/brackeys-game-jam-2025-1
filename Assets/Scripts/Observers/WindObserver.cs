using Observers.dto;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace Observers
{
    public class WindObserver : MonoBehaviour, IObserver<WindDTO>, IObserver<AnvilDTO>, IObserver<StateDTO>
    {
        public WindState windState;
        public AnvilState anvilState;
        
        private void OnEnable()
        {
            windState.AddObserver(this);
            anvilState.AddObserver(this);
            
            StateMachineManager.instance.AddObserver(this);
        }

        private void OnDisable()
        {
            windState.RemoveObserver(this);
            anvilState.RemoveObserver(this);
            StateMachineManager.instance.RemoveObserver(this);
        }
        
        public void OnNotify(WindDTO dto)
        {
            Debug.Log("received WindDTO: " +  + dto._speed + " " + dto._direction);
        }

        public void OnNotify(AnvilDTO dto)
        {
            Debug.Log("received AnvilDTO: " + dto._damage);
        }

        public void OnNotify(StateDTO dto)
        {
            Debug.Log("received StateDTO: " + dto._state +" " + dto._isDefault);
        }
    }
}
