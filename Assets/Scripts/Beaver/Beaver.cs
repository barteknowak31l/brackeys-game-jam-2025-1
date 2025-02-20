using StateMachine.states;
using System;
using System.Collections;
using AudioManager;
using UnityEngine;

public class Beaver : MonoBehaviour
{

    private bool _playerInRange = false;
    private bool _isMoving = false;
    private BeaverState _ctx;
    public float timeToKill = 15f;

    private AudioSource _audioSource;
    private AudioSource _audioSource2;
    
    public void Start()
    {
        StartCoroutine(Timer());
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource2 = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource2.loop = true;
        AudioManager.AudioManager.PlaySound(AudioClips.Beaver, _audioSource, 1.0f);
        AudioManager.AudioManager.PlaySound(AudioClips.BeaverCut, _audioSource2, 1.0f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _playerInRange = true;
            _ctx.OnPlayerCollisionChange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _ctx.OnPlayerCollisionChange(false);
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.V) && !_isMoving)
        {
            _isMoving = true;
            StopCoroutine(Timer());
            StartCoroutine(MoveRightAndDestroy());
        }
    }

    private IEnumerator MoveRightAndDestroy()
    {
        
        AudioManager.AudioManager.StopSound(AudioClips.Beaver, _audioSource);
        AudioManager.AudioManager.StopSound(AudioClips.BeaverCut, _audioSource2);

        _audioSource.loop = false;
        _audioSource2.loop = false;
        AudioManager.AudioManager.PlaySound(AudioClips.Kick, _audioSource, 1.0f);
        AudioManager.AudioManager.PlaySound(AudioClips.BeaverKickScream, _audioSource, 1.0f);
        
        yield return new WaitForSeconds(0.5f); 

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, 0.8f);

        float elapsedTime = 0f;

        while (elapsedTime < 0.8f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.8f);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        yield return new WaitForSeconds(5f);

        transform.position = targetPosition; 
        Destroy(gameObject); 
    }
    public void Setup(BeaverState ctx)
    {
        _ctx = ctx;
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeToKill);

        if (!_isMoving) 
        {
            _ctx.EndTime();
        }
    }

   
}
