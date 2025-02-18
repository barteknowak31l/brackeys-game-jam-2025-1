using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class UfoState : Observable<UfoDTO>, IBaseState
    {
        public string PlayerTag = "Player";
        [SerializeField] GameObject _ufoPrefab;

        [Space] [Header("Variant 1")] [SerializeField]
        private float _ufoSpeed = 3.0f;
        [SerializeField] float _startPositionOffset;
        [SerializeField] float _stopPositionOffset;
        [SerializeField] private float _stationaryPhaseDuration;
        [SerializeField] private float _stationaryPhaseDurationRandomness;
        
        [SerializeField] Vector3 _ufoStartPositionOffset;
        
        private Transform _playerTransform;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _targetPosition;
        
        private Variant _variant;
        private Ufo _ufo;
        
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Enter UfoState");
            _playerTransform = GameObject.FindGameObjectWithTag(PlayerTag).transform;
            _ufo = Instantiate(_ufoPrefab, _playerTransform.position + _ufoStartPositionOffset, Quaternion.identity).GetComponent<Ufo>();
            _ufo.Setup(this);
            
            EnterMovementPhase();
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            // ufo escape
            _ufo.EndPhase(_playerTransform, _ufoStartPositionOffset.y, _ufoSpeed * 5.0f);
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
            _startPosition = _playerTransform.position + _playerTransform.forward * _startPositionOffset;
            _endPosition = _startPosition + _playerTransform.forward  * _stopPositionOffset;
            
            float rnd = Random.Range(0f, 1f);
            _targetPosition = Vector3.Lerp(_startPosition, _endPosition, rnd);
            _targetPosition += _playerTransform.up * _ufoStartPositionOffset.y;
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
    }
}
