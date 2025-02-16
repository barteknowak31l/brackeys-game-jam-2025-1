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
        
        
        private StateQueue _stateQueue;
    
        void Start()
        {
            List<IBaseState> states = new List<IBaseState>();
            states.Add(_windState);
            
            _stateQueue = new StateQueue(states);
            _currentState = _defaultState;
            _currentState.EnterState(this);
        }

        void Update()
        {
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
