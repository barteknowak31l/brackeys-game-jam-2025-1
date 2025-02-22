using System;
using AudioManager;
using StateMachine.states;
using Unity.VisualScripting;
using UnityEngine;

namespace Lightning
{
    public class Lightning : MonoBehaviour
    {
    
        BoxCollider _boxCollider;
        ParticleSystem _particleSystem;
        
        StormState _stormState;
        
        AudioSource _audioSource;
        
        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
            _boxCollider.enabled = false;
            
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Setup(StormState stormState)
        {
            _stormState = stormState;
        }

        private void Update()
        {
            if (_particleSystem && _boxCollider)
            {
                if (_particleSystem.particleCount > 0)
                {
                    _boxCollider.enabled = _particleSystem.particleCount > 0;
                    AudioManager.AudioManager.PlaySound(AudioClips.Thunder, _audioSource, 1.0f);    
                }
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _stormState.OnLightningHitPlayer();
            }
        }
    }
}
