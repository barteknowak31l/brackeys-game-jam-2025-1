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
        
        public bool IsDebugMode;
        
        [SerializeField] DefaultState _defaultState;
        [SerializeField] MainMenuState _mainMenuState;
        [SerializeField] WindState _windState;
        [SerializeField] AnvilState _anvilState;
        [SerializeField] StormState _stormState;
        [SerializeField] UfoState _ufoState;
        [SerializeField] BirdState _birdState;
        [SerializeField] FruitState _fruitState;
        [SerializeField] BeaverState _beaverState;
        [SerializeField] PlayerDeathState _playerDeathState;
        [SerializeField] SharkState _sharkState;
        [SerializeField] StartState _startState;

        private List<IBaseState> states;

        [SerializeField] private StateQueue _stateQueue;

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
            InitQueue();
            _currentState = _mainMenuState;
            NotifyObservers(createDTO(_currentState));
            _currentState.EnterState(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && IsDebugMode)
            {
                EndMechanicState();
            }
            
            if (Input.GetKeyDown(KeyCode.O) && IsDebugMode)
            {
                ForceNextState(Variant.First);
            }
            if (Input.GetKeyDown(KeyCode.P) && IsDebugMode)
            {
                ForceNextState(Variant.Second);
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
        
        public void StartState()
        {
            InitQueue();
            if (_currentState != null) _currentState.ExitState(this);
            _currentState = _startState;
            NotifyObservers(createDTO(_currentState));
            _currentState.EnterState(this);
        }

        public IBaseState GetCurrentState()
        {
            return _currentState;
        }
        
        
        private StateDTO createDTO(IBaseState state, bool isDefault = false)
        {
            return new StateDTO()
                .State(state.GetStateType())
                .Variant(state.GetVariant())
                .IsDefault(isDefault);
        }

        private void InitQueue()
        {
            states = new List<IBaseState>();
            states.Add(_windState);
            states.Add(_anvilState);
            states.Add(_stormState);
            states.Add(_ufoState);
            states.Add(_birdState);
            states.Add(_fruitState);
            states.Add(_beaverState);
            states.Add(_sharkState);
            _stateQueue = new StateQueue(states);
        }
    
        private int iterator = 0;
        private void ForceNextState(Variant variant)
        {
            iterator += 1;
            if(iterator >= states.Count) iterator = 0;
            
            _currentState.ExitState(this);
            _currentState = _defaultState;

            // simulate default state
            NotifyObservers(createDTO(states[iterator].SetVariant(variant), true)); // send next state from default
            _currentState.EnterState(this);
            _currentState.ExitState(this);
            
            // set next state
            _currentState = states[iterator];
            _currentState.EnterState(this);
            NotifyObservers(createDTO(_currentState));

        }
    }
}
