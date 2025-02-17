using System;
using System.Collections;
using Observers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine.states
{
    public class AnvilState : Observable<AnvilDTO>, IBaseState
    {
        public string _playerTag;    
        [SerializeField] private float _anvilSpawnTimeBase;
        [SerializeField] private float _anvilSpawnTimeOffset;
        [SerializeField] private float _anvilSpawnPositionOffsetXZ;
        [SerializeField] private float _anvilSpawnPositionOffsetY;
        [SerializeField] private float _anvilDamage;
        [SerializeField] private float _anvilDamageVariant2;
        [SerializeField] private float _pianoZOffset;

        [Space]
        [SerializeField] private GameObject _anvilPrefab;
        [SerializeField] private GameObject _pianoPrefab;
    
        private Transform _playerTransform;
        private Variant _variant;

        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered AnvilState variant: " + _variant.ToString());
            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
            StartCoroutine(SpawnAnvilCoroutine());
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            StopAllCoroutines();
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        private void SpawnAnvil()
        {
            // random position based on player pos
            Vector3 position = _playerTransform.position + _playerTransform.forward * Random.Range(1.0f, _anvilSpawnPositionOffsetXZ);
            // move position up
            position.y += _anvilSpawnPositionOffsetY;
            
            // correct position of piano
            if (_variant == Variant.Second)
            {
                position.z += _pianoZOffset;
            }
            
            // spawn anvil object
            var prefab = _variant == Variant.First ? _anvilPrefab : _pianoPrefab;
            
            var rotation = Quaternion.Euler(0, 90, 0);
            
            
            var anvil = Instantiate(prefab, position, rotation).GetComponent<Anvil>();
            anvil.Setup(this);
        }
    

        IEnumerator SpawnAnvilCoroutine()
        {
            while (true)
            {
                var spawnTime = _anvilSpawnTimeBase + Random.Range(-_anvilSpawnTimeOffset, _anvilSpawnTimeOffset);
                yield return new WaitForSeconds(spawnTime);
                SpawnAnvil();   
            }
        }

        public void OnAnvilHitPlayer()
        {
            var damage = _variant == Variant.First ? _anvilDamage : _anvilDamageVariant2;
            AnvilDTO anvilDTO = new AnvilDTO().Damage(damage);
            NotifyObservers(anvilDTO);
        }
    
    }
}
