using Observers;
using UnityEngine;

namespace StateMachine.states
{
    public class StartState : Observable<StartDTO>, IBaseState
    {
        private Variant _variant = Variant.First;

        [SerializeField] string _playerTag;
        [SerializeField] GameObject _playerObject;
        [SerializeField] Transform _spawnPoint;
        
        public void EnterState(StateMachineManager ctx)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("Entered StartState variant: " + _variant.ToString());


            _playerObject = GameObject.FindGameObjectWithTag(_playerTag);
            SetupPlayerObject();
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
            return States.StartState;
        }

        public Variant GetVariant()
        {
            return _variant;
        }


        private void SetupPlayerObject()
        {
            _playerObject.transform.position = _spawnPoint.position;
            _playerObject.transform.rotation = _spawnPoint.rotation;
        }
        
        
        
    }
}
