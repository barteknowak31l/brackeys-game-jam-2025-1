using System;
using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    private string _playerTag;
    private string _logTag;
    private AnvilState _ctx;

    private void Start()
    {
        Destroy(gameObject, _destroyDelay);
        AudioManager.AudioManager.PlaySound(AudioClips.AnvilFall);
    }

    public void Setup(AnvilState ctx)
    {
        _ctx = ctx;
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
        AudioManager.AudioManager.PlaySound(AudioClips.AnvilHit);
    }
}
