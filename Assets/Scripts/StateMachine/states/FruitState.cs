using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Lightning;
using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class FruitState : Observable<FruitDTO>, IBaseState
    {
        [Header("Variant 1")]
        [SerializeField] string _playerTag = "Player";
        [SerializeField] int _bananaCount = 5;
        [SerializeField] float _bananaOffsetBase = 5.0f;
        [SerializeField] float _bananaOffsetRandomness = 2.0f;
        [SerializeField] float _bananaRotationX = 0;
        [SerializeField] private float _bananaYPosition = 0.604f;
        [Space]
        [SerializeField] GameObject _bananaPrefab;         
        private List<GameObject> _instantiatedBanana;

        [Header("Variant 2")]
        [SerializeField] private float _shipSpeed = 1f;
        [SerializeField] float _startPositionOffset;
        [SerializeField] float _stopPositionOffset;
        [SerializeField] private float _stationaryPhaseDuration;
        [SerializeField] private float _stationaryPhaseDurationRandomness;
        [SerializeField] Vector3 _shipStartPositionOffset;
        [SerializeField] GameObject _shipPrefab;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _targetPosition;
        [SerializeField] private Ship _ship;


        private Transform _playerTransform;
        
        private Variant _variant;

        
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered FruitState variant: " + _variant.ToString());

                _instantiatedBanana = new List<GameObject>();
                _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

                Vector3 playerPos = new Vector3(_playerTransform.position.x, _bananaYPosition, _playerTransform.position.z);
                // wylosuj x pozycji i utworz tam particle, dodaj je do listy
                for (int i = 0; i < _bananaCount; i++)
                {
                var pos = playerPos
                          + _playerTransform.forward * _bananaOffsetBase * (i + 1)
                          + _playerTransform.forward * Random.Range(0, _bananaOffsetRandomness);
                    var rot = Quaternion.Euler(_bananaRotationX, 0, 0);
                    GameObject banana = Instantiate(_bananaPrefab, pos, rot);
                    banana.GetComponent<Banana>().Setup(this);
                    _instantiatedBanana.Add(banana);
                }
            if (_variant == Variant.Second)
            {
                _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
                Vector3 shipPosition = new Vector3(_playerTransform.position.x, _shipStartPositionOffset.y, _shipStartPositionOffset.z);
                _ship = Instantiate(_shipPrefab, shipPosition, Quaternion.identity).GetComponent<Ship>();
                _ship.Setup(this);
                EnterMovementPhase();
            }
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            foreach (var banana in _instantiatedBanana)
            {
                Destroy(banana,1);   
            }
            if (_variant == Variant.Second)
            {
                _ship.EndPhase(_playerTransform, _shipStartPositionOffset.y, _shipSpeed * 5.0f);
            }
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.Fruit;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
        
        public void OnFruitHitPlayer()
        {
            NotifyObservers(new FruitDTO());
            Debug.Log("Hit");
        }
        public void EnterMovementPhase()
        {
            FindNextShipPosition();
            MoveShipTowards();
        }
        private void FindNextShipPosition()
        {
            Vector3 shipPosition = new Vector3(_playerTransform.position.x, _shipStartPositionOffset.y, _shipStartPositionOffset.z);

            _startPosition = shipPosition + _playerTransform.forward * _startPositionOffset;
            _endPosition = _startPosition + _playerTransform.forward * _stopPositionOffset;

            float rnd = Random.Range(0f, 1f);
            _targetPosition = Vector3.Lerp(_startPosition, _endPosition, rnd);
        }
        private void MoveShipTowards()
        {
            _ship.MovePhase(_targetPosition, _shipSpeed);
        }
        public void EnterStationaryPhase()
        {
            var stationaryTime = _stationaryPhaseDuration + Random.Range(-_stationaryPhaseDurationRandomness, _stationaryPhaseDurationRandomness);
            _ship.StationaryPhase(stationaryTime);
        }
    }
}
