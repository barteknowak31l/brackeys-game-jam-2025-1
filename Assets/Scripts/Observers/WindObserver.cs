using AudioManager;
using Observers.dto;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace Observers
{

    public class WindObserver : MonoBehaviour, IObserver<WindDTO>, IObserver<AnvilDTO>, IObserver<StateDTO>, IObserver<PlayerDeathDTO>, IObserver<StormDTO>,
    IObserver<UfoDTO>, IObserver<BirdDTO>, IObserver<SharkDTO>
    {
        public WindState windState;
        public AnvilState anvilState;
        public StormState stormState;
        public UfoState ufoState;
        public PlayerDeathState playerDeathState;
        public BirdState birdState;
        public SharkState sharkState;
        
        private void OnEnable()
        {
            windState.AddObserver(this);
            anvilState.AddObserver(this);
            stormState.AddObserver(this);
            ufoState.AddObserver(this);
            playerDeathState.AddObserver(this);
            birdState.AddObserver(this);
            sharkState.AddObserver(this);
            StateMachineManager.instance.AddObserver(this);
        }

        private void OnDisable()
        {
            windState.RemoveObserver(this);
            anvilState.RemoveObserver(this);
            stormState.RemoveObserver(this);
            ufoState.RemoveObserver(this);
            playerDeathState.RemoveObserver(this);
            birdState.RemoveObserver(this);
            sharkState.RemoveObserver(this);
            StateMachineManager.instance.RemoveObserver(this);
        }
        
        public void OnNotify(WindDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("received WindDTO: " +  + dto._speed + " " + dto._direction);

        }

        public void OnNotify(AnvilDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("received AnvilDTO: " + dto._damage);

        }

        public void OnNotify(StateDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("received StateDTO: " + dto._state + " " + dto._isDefault);

        }
        
        public void OnNotify(StormDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("Lightning has struck player");

        }

        public void OnNotify(UfoDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log($"UFO!!! player in beam: {dto._playerInBeam}, cow hit: {dto._cowHit}");

        }

        public void OnNotify(PlayerDeathDTO dto)
        {
        }
           
        public void OnNotify(BirdDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log(dto._damage + "Bird!!!");

        }

        public void OnNotify(SharkDTO dto)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.unityLogger.Log("Got SharkDTO");

        }
    }
}
