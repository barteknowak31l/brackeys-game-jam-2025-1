using NUnit.Framework;
using Observers;
using StateMachine;
using StateMachine.states;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

namespace StateMachine.states
{
    public class BirdState : Observable<BirdDTO>, IBaseState
    {

        private Variant _variant;
        public string _playerTag;
        private Transform _playerTransform;

        [SerializeField] private float _birdSpawnDistance;
        [SerializeField] private float _birdSideOffset;
        [SerializeField] private float _birdSpawnTimeBase;
        [SerializeField] private float _birdSpawnTimeOffset;
        [SerializeField] private float _birdDamage;
        [SerializeField] private float _birdDamageVariant2;
        [SerializeField] private int _birdCount;
        [SerializeField] private float _birdYOffset = 1.5f;
        [SerializeField] private float _frontBirdDistance;
        [SerializeField] private float _birdFrontYOffset;


        [Space]
        [SerializeField] private GameObject _littleBirdPrefab;
        [SerializeField] private GameObject _bigBirdPrefab;
        [SerializeField] private GameObject _bigBirdFrontPrefab;


        public void EnterState(StateMachineManager ctx)
        {
            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
            StartCoroutine(SpawnBirdCoroutine());
            if (_variant == Variant.Second)
            {
                StartCoroutine(SpawnFrontBird());
            }
        }

        public void ExitState(StateMachineManager ctx)
        {
            StopAllCoroutines();
        }

        public States GetStateType()
        {
            return States.Bird;
        }

        public Variant GetVariant()
        {
            return _variant;
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public void UpdateState(StateMachineManager ctx)
        {

        }

        IEnumerator SpawnBird()
        {
            _birdSpawnDistance *= Random.Range(0.9f, 1.1f);
            Vector3 position = new Vector3(_playerTransform.position.x, _birdYOffset, _playerTransform.position.z) + _playerTransform.forward * _birdSpawnDistance;

            float side = Random.Range(0, 2) == 0 ? -1f : 1f;
            position += _playerTransform.right * side * _birdSideOffset;
            for (int i = 0; i < _birdCount; i++)
            {
                float spawnDelay = Random.Range(1f, 1.5f);

                var prefab = _variant == Variant.First ? _littleBirdPrefab : _bigBirdPrefab;

                var rotation = side == -1f ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

                float birdXOffset = 2f;
                position = position + new Vector3(i+ birdXOffset, 0f, 0f);


                GameObject bird = Instantiate(prefab, position, rotation);
                bird.GetComponent<Bird>().Setup(this);
                yield return new WaitForSeconds(spawnDelay);
            }

        }
        IEnumerator SpawnFrontBird()
        {
            while (true)
            {
                Vector3 position = new Vector3(_playerTransform.position.x, _birdFrontYOffset, _playerTransform.position.z) + _playerTransform.forward * _frontBirdDistance;
                var prefab = _bigBirdFrontPrefab;
                var rotation = Quaternion.Euler(0, 90, 0);

                float spawnDelay = Random.Range(4f, 8f);
                yield return new WaitForSeconds(spawnDelay);
                GameObject bird = Instantiate(prefab, position, rotation);
                bird.GetComponent<Bird>().Setup(this);

            }
        }

        public void OnBirdHitPlayer()
        {
            var damage = _variant == Variant.First ? _birdDamage : _birdDamageVariant2;
            BirdDTO birdDTO = new BirdDTO().Damage(damage)
                .Variant(_variant);
            NotifyObservers(birdDTO);
        }

        IEnumerator SpawnBirdCoroutine()
        {
            while (true)
            {
                var spawnTime = _birdSpawnTimeBase + Random.Range(-_birdSpawnTimeOffset, _birdSpawnTimeOffset);
                yield return new WaitForSeconds(spawnTime);
                StartCoroutine(SpawnBird());

            }
        }
    }
}
