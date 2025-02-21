using System.Collections.Generic;
using AudioManager;
using Observers;
using Observers.dto;
using Shark;
using Unity.VisualScripting;
using UnityEngine;

namespace StateMachine.states
{
    public class SharkState : Observable<SharkDTO>, IBaseState
    {
        public string PlayerTag = "Player";

        [Header("Variant1")] 
        [SerializeField] private int _numberOfSharkSpawners = 5;
        [SerializeField] private float _sharkDuration = 2.0f;
        [SerializeField] private float _sharkDurationRandomness = 0.7f;
        [SerializeField] private float _sharkSpawnDelay = 5f;
        [SerializeField] private float _sharkSpawnDelayRandomness = 2f;
        [SerializeField] private float _sharkHeight = 5f;
        [SerializeField] private float _sharkYOffset = -2.0f;
      
        
        [Header("Variant2")]
        [SerializeField] private int _numberOfSharknados = 5;
        [SerializeField] private float _sharknadoSpeed = 2.0f;
        [SerializeField] private float _sharknadoSpeedRandomness = 0.7f;

        [Header("Common")]
        [SerializeField] private float _sideOffset = 5.0f;
        [SerializeField] float _frontOffsetBase = 10.0f;
        [SerializeField] float _frontOffsetBaseRandomness = 2.0f;
        [SerializeField] float _logPositionY = 0.604f;
        
        [Space]
        [SerializeField] GameObject _sharkSpawnerPrefab;
        [SerializeField] GameObject _sharkPrefab;
        [SerializeField] GameObject _sharknadoPrefab;
        
        private Variant _variant;
        private List<GameObject> _instantiatedSharknados;
        private List<GameObject> _instantiatedSharkSpawners;
        private Transform _playerTransform;

    
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered SharkState variant: " + _variant.ToString());

            
            _instantiatedSharknados = new List<GameObject>();
            _instantiatedSharkSpawners = new List<GameObject>();
            _playerTransform = GameObject.FindGameObjectWithTag(PlayerTag).transform;

            Vector3 playerPos = new Vector3(_playerTransform.position.x, _logPositionY, _playerTransform.position.z);

            // shark spawners
            for (int i = 0; i < _numberOfSharkSpawners; i++)
            {
                var pos = playerPos 
                          + _playerTransform.forward * (_frontOffsetBase * (i+1)) 
                          + _playerTransform.forward * Random.Range(-_frontOffsetBase, _frontOffsetBase);
                var duration = _sharkDuration + Random.Range(-_sharkDurationRandomness, _sharkDurationRandomness);
                var delay =  _sharkSpawnDelay + Random.Range(-_sharkSpawnDelayRandomness, _sharkSpawnDelayRandomness); 
                var rot = Quaternion.Euler(0, 90.0f, 0);
                GameObject sharkSpawner = Instantiate(_sharkSpawnerPrefab, pos, rot);
                sharkSpawner.GetComponent<SharkSpawner>().Setup(this, delay, _sideOffset, _sharkPrefab, _sharkHeight, duration, _sharkYOffset);
                _instantiatedSharkSpawners.Add(sharkSpawner);
            } 
            
            // sharknados
            if (_variant == Variant.Second)
            {
                StartSharknadoSound();   
                for (int i = 0; i < _numberOfSharknados; i++)
                {
                    var pos = playerPos 
                              + _playerTransform.forward * (_frontOffsetBase * (i+1)) 
                              + _playerTransform.forward * Random.Range(-_frontOffsetBase, _frontOffsetBase);
                    var rot = Quaternion.Euler(-90.0f, 0, 0);
                    var spd = _sharknadoSpeed + Random.Range(-_sharknadoSpeedRandomness, _sharknadoSpeedRandomness);
                    GameObject sharknado = Instantiate(_sharknadoPrefab, pos, rot);
                    sharknado.GetComponent<Sharknado.Sharknado>().Setup(this, _sideOffset, spd);
                    _instantiatedSharknados.Add(sharknado);
                }   
            }
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            if (_variant == Variant.Second)
            {
                StopSharknadoSound();
                foreach (var sharknado in _instantiatedSharknados)
                {
                    sharknado.GetComponent<Sharknado.Sharknado>().PrepareToDestroy();
                }
            }
            
            foreach (var sharkSpawner in _instantiatedSharkSpawners)
            {
                Destroy(sharkSpawner);
            }
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.SharkState;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
        
        public void OnSharknadoHitPlayer()
        {
            NotifyObservers(new SharkDTO());
        }
        public void OnSharkHitPlayer()
        {
            NotifyObservers(new SharkDTO());
        }


        private void StartSharknadoSound()
        {
            AudioManager.AudioManager.PlaySound(AudioClips.Sharknado);
        }

        private void StopSharknadoSound()
        {
            AudioManager.AudioManager.StopSound(AudioClips.Sharknado);
        }
        
    }
    
}


