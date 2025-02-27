using System.Collections;
using Observers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace StateMachine.states
{
    public class AnvilState : Observable<AnvilDTO>, IBaseState
    {
        public string _playerTag;    
        
        [Header("Anvil")]
        
        [SerializeField] private float _firstAnvilSpawnDelay = 1.0f;
        [SerializeField] private float _anvilSpawnTimeBase;
        [SerializeField] private float _anvilSpawnTimeOffset;
        [SerializeField] private float _anvilSpawnPositionOffsetXZ;
        [SerializeField] private float _anvilSpawnPositionOffsetY;
        [SerializeField] private float _anvilDamage;
        [SerializeField] private float _anvilDamageVariant2;

        [Header("Piano")]
        [SerializeField] private float _pianoZOffset;
        
        [Header("Common")]
        [SerializeField] private float _fallSpeed;
        [SerializeField] private float _fallDistance;
        [SerializeField] private float _spawnYOffset = 0.604f;
        
        [Space]
        [SerializeField] private GameObject _anvilPrefab;
        [SerializeField] private GameObject _pianoPrefab;
    
        private Transform _playerTransform;
        private Variant _variant;

        public void EnterState(StateMachineManager ctx)
        {
            if (StateMachineManager.instance.IsDebugMode)
                Debug.Log("Entered AnvilState variant: " + _variant.ToString());


            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
            StartCoroutine(SpawnAnvilCoroutine());
            Invoke("SpawnAnvil", _firstAnvilSpawnDelay);
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

        public States GetStateType()
        {
            return States.Anvil;
        }

        public Variant GetVariant()
        {
            return _variant;
        }

        private void SpawnAnvil()
        {
            // random position based on player pos
            Vector3 position = new Vector3(_playerTransform.position.x,_spawnYOffset,_playerTransform.position.z) + _playerTransform.forward * Random.Range(2.5f, _anvilSpawnPositionOffsetXZ);
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
            anvil.Setup(this, _variant, _fallSpeed, _fallDistance);
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
