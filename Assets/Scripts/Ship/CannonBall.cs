using AudioManager;
using StateMachine.states;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float lifeTime = 5f;
    FruitState _state;
    
    private AudioSource _audioSource;
    
    
    public void Setup(FruitState state)
    {
        _state = state;
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _state.OnFruitHitPlayer();
            AudioManager.AudioManager.PlaySound(AudioClips.FruitHit, _audioSource, 1.0f);
        }
    }
}
