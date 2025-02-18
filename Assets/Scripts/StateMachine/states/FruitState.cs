using System.Collections.Generic;
using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class FruitState : Observable<FruitDTO>, IBaseState
    {
        [SerializeField] string _playerTag = "Player";
        [SerializeField] int _bananaCount = 5;
        [SerializeField] float _bananaOffsetBase = 5.0f;
        [SerializeField] float _bananaOffsetRandomness = 2.0f;
        [SerializeField] float _bananaRotationY = 90.0f;
        
        
        [Space]
        [SerializeField] GameObject _bananaPrefab;
         
        private List<GameObject> _instantiatedBanana;
        private Transform _playerTransform;
        
        private Variant _variant;

        
        public void EnterState(StateMachineManager ctx)
        {
            _instantiatedBanana = new List<GameObject>();
            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            Vector3 playerPos = _playerTransform.position;
            // wylosuj x pozycji i utworz tam particle, dodaj je do listy
            for (int i = 0; i < _bananaCount; i++)
            {
                var pos = playerPos 
                          + _playerTransform.forward * _bananaOffsetBase * (i+1) 
                          + _playerTransform.forward * Random.Range(-_bananaOffsetRandomness, _bananaOffsetRandomness);
                var rot = Quaternion.Euler(0, _bananaRotationY, 0);
                GameObject banana = Instantiate(_bananaPrefab, pos, rot);
                _instantiatedBanana.Add(banana);
            }
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            foreach (var lightning in _instantiatedBanana)
            {
                Destroy(lightning);   
            }
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.Storm;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
        
        public void OnLightningHitPlayer()
        {
            NotifyObservers(new FruitDTO());
        }
        
        
    }
}
