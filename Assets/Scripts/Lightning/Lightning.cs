using System;
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
        
        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
            _boxCollider.enabled = false;
        }

        public void Setup(StormState stormState)
        {
            _stormState = stormState;
        }

        private void Update()
        {
            if (_particleSystem && _boxCollider)
            {
                _boxCollider.enabled = _particleSystem.particleCount > 0;
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
