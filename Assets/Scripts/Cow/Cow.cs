using System;
using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Cow : MonoBehaviour
{
    private UfoState _ctx;
    private Rigidbody _rb;
    private AudioSource _cowHit1;
    private AudioSource _cowHit2;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _cowHit1 = gameObject.AddComponent<AudioSource>();
        _cowHit2 = gameObject.AddComponent<AudioSource>();
        
    }

    public void Setup(UfoState ctx)
    {
        _ctx = ctx;
    }
    
    
    public void UseGravity()
    {
        _rb.useGravity = true;
        _rb.linearVelocity = Vector3.zero;
        transform.parent = null;
        AudioManager.AudioManager.PlaySound(AudioClips.Cow);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_ctx.PlayerTag))
        {
            AudioManager.AudioManager.PlaySound(AudioClips.PlayerHit, _cowHit1, 1.0f);
            AudioManager.AudioManager.PlaySound(AudioClips.Cow, _cowHit2, 1.0f);
            _ctx.OnCowHit();
        }
        Destroy(gameObject, 1.5f);
    }
}
