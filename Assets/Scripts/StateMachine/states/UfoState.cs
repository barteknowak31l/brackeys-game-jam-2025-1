using Observers;
using Observers.dto;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine.states
{
    public class UfoState : Observable<UfoDTO>, IBaseState
    {
        public string PlayerTag = "Player";
        [Space]
        [Header("Variant 1")] 
        [SerializeField] GameObject _ufoPrefab;
        [SerializeField] private float _ufoSpeed = 3.0f;
        [SerializeField] float _startPositionOffset;
        [SerializeField] float _stopPositionOffset;
        [SerializeField] private float _stationaryPhaseDuration;
        [SerializeField] private float _stationaryPhaseDurationRandomness;
        [SerializeField] Vector3 _ufoStartPositionOffset;

        [Space] [Header("Variant 2")] 
        [SerializeField] private GameObject _ufoCowDispenserPrefab;
        [SerializeField] private GameObject _cowPrefab;
        [SerializeField] private float _ufoCowDispenserOffsetY;
        public float orbitRadius = 20f; 
        public float orbitSpeed = 0.5f;
        
        private Transform _playerTransform;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _targetPosition;
        
        private Variant _variant;
        private Ufo _ufo;
        private UfoCowDispenser _ufoCowDispenser;
        
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered UfoState variant: " + _variant.ToString());

            _playerTransform = GameObject.FindGameObjectWithTag(PlayerTag).transform;
            Vector3 ufoPosition = new Vector3(_playerTransform.position.x, _ufoStartPositionOffset.y, _playerTransform.position.z);
            _ufo = Instantiate(_ufoPrefab, ufoPosition, Quaternion.identity).GetComponent<Ufo>();
            _ufo.Setup(this);


            if (_variant == Variant.Second)
            {
                _ufoCowDispenser =
                    Instantiate(_ufoCowDispenserPrefab, _playerTransform.position + _playerTransform.forward * -10.0f,
                        Quaternion.identity).GetComponent<UfoCowDispenser>();
                _ufoCowDispenser.Setup(this, _playerTransform, _ufoCowDispenserOffsetY, _cowPrefab);
                
            }
            
            EnterMovementPhase();
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            // ufo escape
            _ufo.EndPhase(_playerTransform, _ufoStartPositionOffset.y, _ufoSpeed * 5.0f);

            if (_ufoCowDispenser != null)
            {
                _ufoCowDispenser.EndPhase();
            }
            
            Debug.Log("Exit UfoState");
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.Ufo;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
        
        private void FindNextUfoPosition()
        {
            Vector3 ufoPosition = new Vector3(_playerTransform.position.x, _ufoStartPositionOffset.y, _playerTransform.position.z);

            _startPosition = ufoPosition + _playerTransform.forward * _startPositionOffset;
            _endPosition = _startPosition + _playerTransform.forward  * _stopPositionOffset;
            
            float rnd = Random.Range(0f, 1f);
            _targetPosition = Vector3.Lerp(_startPosition, _endPosition, rnd);
        }

        private void MoveUfoTowards()
        {
            _ufo.MovePhase(_targetPosition, _ufoSpeed);
        }

        public void EnterMovementPhase()
        {
            FindNextUfoPosition();
            MoveUfoTowards();
        }

        public void EnterStationaryPhase()
        {
            var stationaryTime = _stationaryPhaseDuration + Random.Range(-_stationaryPhaseDurationRandomness, _stationaryPhaseDurationRandomness);
            _ufo.StationaryPhase(stationaryTime);
        }
        
        public void OnBeamEnter()
        {
            var dto = new UfoDTO()
                .PlayerInBeam(true);
            NotifyObservers(dto);
        }

        public void OnBeamExit()
        {
            var dto = new UfoDTO()
                .PlayerInBeam(false);
            NotifyObservers(dto);
        }

        public void OnCowHit()
        {
            var dto = new UfoDTO()
                .CowHit(true);
            NotifyObservers(dto);
        }
    }
}
