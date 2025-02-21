using System;
using AudioManager;
using StateMachine;
using StateMachine.states;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    private AnvilState _ctx;
    private Variant _variant;

    
    private float _distance = 5f;
    private float _fallSpeed = 2f;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _elapsedTime = 0f;
    private bool _isMoving = true;
    private float _time;
    private AudioSource _audio;

    
    private void Start()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        AudioManager.AudioManager.PlaySound(AudioClips.AnvilFall,_audio, 1.0f);
    }

    public void Setup(AnvilState ctx, Variant variant, float fallSpeed, float distance)
    {
        _ctx = ctx;
        _variant = variant;
        _distance = distance;
        _fallSpeed = fallSpeed;
        
        _time = _distance/_fallSpeed;
        
        _startPos = transform.position;
        _targetPos = _startPos - new Vector3(0, distance, 0);
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
    void Update()
    {
        if (_isMoving)
        {
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / _time);
            transform.position = Vector3.Lerp(_startPos, _targetPos, t);
            if (t >= 1)
            {
                _isMoving = false;
            }
        }
    }
}
