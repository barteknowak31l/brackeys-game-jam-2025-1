using System.Collections.Generic;
using AudioManager;
using Observers;
using Observers.dto;
using UnityEngine;

namespace StateMachine.states
{
    public class StormState : Observable<StormDTO>, IBaseState
    {
        [SerializeField] string _playerTag = "Player";
        [SerializeField] int _lightningCount = 5;
        [SerializeField] float _lightningOffsetBase = 5.0f;
        [SerializeField] float _lightningOffsetRandomness = 2.0f;
        [SerializeField] float _lightningRotationY = 90.0f;
        [SerializeField] float _lightingParticleSpawnYOffset = 0.604f;
        
        
        [Space]
        [SerializeField] GameObject _LightningParticleSystemPrefab;
         
        private List<GameObject> _instantiatedLightnings;
        private Transform _playerTransform;

        
        private Variant _variant;
        
        private AudioSource _audioSource;

        void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
        }

        
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered StormState variant: " + _variant.ToString());

            _instantiatedLightnings = new List<GameObject>();
            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            Vector3 playerPos = new Vector3(_playerTransform.position.x, _lightingParticleSpawnYOffset, _playerTransform.position.z);
            // wylosuj x pozycji i utworz tam particle, dodaj je do listy
            for (int i = 0; i < _lightningCount; i++)
            {
                var pos = playerPos 
                          + _playerTransform.forward * _lightningOffsetBase * (i+1) 
                          + _playerTransform.forward * Random.Range(-_lightningOffsetRandomness, _lightningOffsetRandomness);
                var rot = Quaternion.Euler(0, _lightningRotationY, 0);
                GameObject lightning = Instantiate(_LightningParticleSystemPrefab, pos, rot);
                lightning.GetComponent<Lightning.Lightning>().Setup(this);
                _instantiatedLightnings.Add(lightning);
            }

            if (_variant == Variant.Second)
            {
                StartRainSound();
            }
            
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            Debug.Log("Exit Storm State");
            StopRainSound();
            foreach (var lightning in _instantiatedLightnings)
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
            NotifyObservers(new StormDTO());
        }


        private void StartRainSound()
        {
            AudioManager.AudioManager.PlaySound(AudioClips.Rain, _audioSource, 1.0f);
        }

        private void StopRainSound()
        {
            AudioManager.AudioManager.StopSound(AudioClips.Rain, _audioSource);
        }
        
        
    }
}
