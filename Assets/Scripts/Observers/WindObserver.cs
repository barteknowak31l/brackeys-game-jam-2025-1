using Observers.dto;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace Observers
{
    public class WindObserver : MonoBehaviour, IObserver<WindDTO>, IObserver<AnvilDTO>, IObserver<StateDTO>, IObserver<StormDTO>
    , IObserver<UfoDTO>
    {
        public WindState windState;
        public AnvilState anvilState;
        public StormState stormState;
        public UfoState ufoState;
        
        private void OnEnable()
        {
            windState.AddObserver(this);
            anvilState.AddObserver(this);
            stormState.AddObserver(this);
            ufoState.AddObserver(this);
            StateMachineManager.instance.AddObserver(this);
        }

        private void OnDisable()
        {
            windState.RemoveObserver(this);
            anvilState.RemoveObserver(this);
            stormState.RemoveObserver(this);
            ufoState.RemoveObserver(this);
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
            Debug.Log("received StateDTO: " + dto._state + " " + dto._isDefault);
        }
        
        public void OnNotify(StormDTO dto)
        {
            Debug.Log("Lightning has struck player");
        }

        public void OnNotify(UfoDTO dto)
        {
            Debug.Log($"UFO!!! player in beam: {dto._playerInBeam}");
        }
    }
}
