using System.Collections.Generic;
using Lightning;
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
        [SerializeField] float _bananaRotationX = 0;
        
        
        [Space]
        [SerializeField] GameObject _bananaPrefab;
         
        private List<GameObject> _instantiatedBanana;
        private Transform _playerTransform;
        
        private Variant _variant;

        
        public void EnterState(StateMachineManager ctx)
        {
            if (_variant == Variant.First)
            {
                _instantiatedBanana = new List<GameObject>();
                _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

                Vector3 playerPos = _playerTransform.position;
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
            }
            else
            {

            }
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            foreach (var banana in _instantiatedBanana)
            {
                Destroy(banana);   
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
            Debug.Log("Hit)");
        }
        
        
    }
}
