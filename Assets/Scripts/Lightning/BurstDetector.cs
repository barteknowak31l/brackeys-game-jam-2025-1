using AudioManager;
using UnityEngine;

namespace Lightning
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BurstDetector : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private int _previousParticleCount = 0;


        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (_particleSystem == null) return;

            int currentParticleCount = _particleSystem.particleCount;

            if (currentParticleCount > _previousParticleCount + 1)
            {
                OnParticleBurst();
            }

            _previousParticleCount = currentParticleCount;
        }

        void OnParticleBurst()
        {
            Debug.Log("Burst detected");
            AudioManager.AudioManager.PlaySound(AudioClips.Thunder);
        }
    }
}