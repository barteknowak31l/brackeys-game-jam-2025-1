using System;
using AudioManager;
using StateMachine;
using StateMachine.states;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    private AnvilState _ctx;
    private Variant _variant;

    private void Start()
    {
        Destroy(gameObject, _destroyDelay);
        AudioManager.AudioManager.PlaySound(AudioClips.AnvilFall);
    }

    public void Setup(AnvilState ctx, Variant variant)
    {
        _ctx = ctx;
        _variant = variant;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_ctx._playerTag))
        {
            _ctx.OnAnvilHitPlayer();
        }
    
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        AudioManager.AudioManager.StopSound(AudioClips.AnvilFall);

        if (_variant == Variant.First)
        {
            AudioManager.AudioManager.PlaySound(AudioClips.AnvilHit);
        }
        else
        {
            AudioManager.AudioManager.PlaySound(AudioClips.PianoHit);
        }
    }
}
