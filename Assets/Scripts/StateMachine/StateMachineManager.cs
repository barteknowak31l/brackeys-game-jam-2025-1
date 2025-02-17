using System.Collections.Generic;
using StateMachine.states;
using UnityEngine;

namespace StateMachine
{
    public class StateMachineManager : MonoBehaviour
    {
    
        private IBaseState _currentState;
        
        [SerializeField] DefaultState _defaultState;
        [SerializeField] WindState _windState;
        [SerializeField] AnvilState _anvilState;
        
        
        private StateQueue _stateQueue;
    
        void Start()
        {
            List<IBaseState> states = new List<IBaseState>();
            states.Add(_windState);
            states.Add(_anvilState);
            
            _stateQueue = new StateQueue(states);
            _currentState = _defaultState;
            _currentState.EnterState(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextState();
            }
            _currentState.UpdateState(this);
        }

        public void NextState()
        {
            if (_currentState != null) _currentState.ExitState(this);
            _currentState = _stateQueue.NextState();
            _currentState.EnterState(this);
        }
    }
}
