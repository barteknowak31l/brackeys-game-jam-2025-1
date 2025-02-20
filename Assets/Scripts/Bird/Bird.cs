using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    private BirdState _ctx;

    [SerializeField] private float _birdSpeed;
    public Vector3 direction = Vector3.back;
    
    private AudioSource _audioSource;
    
    
    private void Start()
    {
        Destroy(gameObject, _destroyDelay);
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        
        AudioManager.AudioManager.PlaySound(AudioClips.BirdSound, _audioSource, 1.0f);
        
    }

    public void Setup(BirdState ctx)
    {
        _ctx = ctx;
    }
    void Update()
    {
        transform.Translate(_birdSpeed * Time.deltaTime * direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_ctx._playerTag))
        {
            _ctx.OnBirdHitPlayer();
        }

        Destroy(gameObject);
    }
}
