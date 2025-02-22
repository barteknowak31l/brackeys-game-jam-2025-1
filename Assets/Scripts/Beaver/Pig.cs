using System;
using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float speed;
    private BeaverState _ctx;
    private int _pigDestroyTime = 20;
    
    private AudioSource _audioSourceScream;
    private AudioSource _audioSourceHit;
    
    void Start()
    {
        Destroy(gameObject, _pigDestroyTime);
        transform.Rotate(0, 270, 0);
        
        _audioSourceScream = gameObject.AddComponent<AudioSource>();
        _audioSourceHit = gameObject.AddComponent<AudioSource>();
        
        _audioSourceScream.playOnAwake = false;
        _audioSourceScream.loop = true;
        
        AudioManager.AudioManager.PlaySound(AudioClips.Pig, _audioSourceScream, 1.0f);
    }
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    public void Setup(BeaverState ctx)
    {
        _ctx = ctx;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_ctx._playerTag))
        {
            _ctx.EndTime();
            AudioManager.AudioManager.PlaySound(AudioClips.PlayerHit, _audioSourceHit, 1.0f);
            Destroy(gameObject,1.0f);
        }
  
    }

    private void OnDestroy()
    {
        _ctx.RemovePigFromList(this);
    }
}
