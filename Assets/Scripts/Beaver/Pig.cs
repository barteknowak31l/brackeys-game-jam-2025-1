using System;
using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float speed;
    private BeaverState _ctx;
    private int _pigDestroyTime = 20;
    
    private AudioSource _audioSourceSteps;
    private AudioSource _audioSourceScream;
    
    void Start()
    {
        Destroy(gameObject, _pigDestroyTime);
        transform.Rotate(0, 270, 0);
        
        _audioSourceSteps = gameObject.AddComponent<AudioSource>();
        _audioSourceScream = gameObject.AddComponent<AudioSource>();
        
        _audioSourceSteps.playOnAwake = false;
        _audioSourceSteps.loop = true;
        _audioSourceScream.playOnAwake = false;
        _audioSourceScream.loop = true;
        
        AudioManager.AudioManager.PlaySound(AudioClips.Pig, _audioSourceScream, 1.0f);
        AudioManager.AudioManager.PlaySound(AudioClips.SprintSteps, _audioSourceSteps, 1.0f);
        
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
            Destroy(gameObject);
        }
  
    }

    private void OnDestroy()
    {
        _ctx.RemovePigFromList(this);
    }
}
