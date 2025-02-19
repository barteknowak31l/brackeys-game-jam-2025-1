using System;
using System.Collections.Generic;
using Observers;
using StateMachine.states;
using UnityEngine;

namespace StateMachine
{
    public class StateMachineManager : Observable<StateDTO>
    {
    
        
        private IBaseState _currentState;
        
        [SerializeField] DefaultState _defaultState;
        [SerializeField] WindState _windState;
        [SerializeField] AnvilState _anvilState;
        [SerializeField] StormState _stormState;
        [SerializeField] UfoState _ufoState;
        [SerializeField] BirdState _birdState;
        [SerializeField] FruitState _fruitState;
        [SerializeField] PlayerDeathState _playerDeathState;
        

        private StateQueue _stateQueue;

        public static StateMachineManager instance { get; private set; }

        public override void Awake()
        {
            base.Awake();
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            List<IBaseState> states = new List<IBaseState>();
            states.Add(_windState);
            states.Add(_anvilState);
            states.Add(_stormState);
            states.Add(_ufoState);
            states.Add(_birdState);
            states.Add(_fruitState);
            
            _stateQueue = new StateQueue(states);
            _currentState = _defaultState;
            NotifyObservers(createDTO(_currentState, true));
            _currentState.EnterState(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndMechanicState();
            }
            _currentState.UpdateState(this);
        }

        public void NextState()
        {
            if (_currentState != null) _currentState.ExitState(this);
            _currentState = _stateQueue.NextState();
            _currentState.EnterState(this);
            NotifyObservers(createDTO(_currentState));
        }

        public void EndMechanicState()
        {
            if (_currentState != null) _currentState.ExitState(this);
            _currentState = _defaultState;
            
            NotifyObservers(createDTO(_stateQueue.Peek(), true));
            _currentState.EnterState(this);
        }
        
        public void PlayerDeathState()
        {
            if (_currentState != null) _currentState.ExitState(this);
            _currentState = _playerDeathState;
            _currentState.EnterState(this);
            NotifyObservers(createDTO(_currentState));
        }

        private StateDTO createDTO(IBaseState state, bool isDefault = false)
        {
            return new StateDTO()
                .State(state.GetStateType())
                .Variant(state.GetVariant())
                .IsDefault(isDefault);
        }
        
    }
}
