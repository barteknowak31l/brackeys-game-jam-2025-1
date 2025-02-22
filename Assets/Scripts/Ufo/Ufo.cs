using System.Collections;
using AudioManager;
using StateMachine.states;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    private UfoState _ctx;
    public CapsuleCollider Collider;
    public GameObject UfoLaser; 
    
    private AudioSource _audioSource;
    private AudioSource _audioSourceKidnap;
    
    public void Setup(UfoState ctx)
    {
        _ctx = ctx;
        StartSound();
    }

    public void MovePhase(Vector3 targetPosition, float speed, bool continuePhases = true)
    {
        UfoLaser.SetActive(false);
        Collider.enabled = false;
        _ctx.OnBeamExit();
        StartCoroutine(MoveToPosition(targetPosition, speed, continuePhases));
        AudioManager.AudioManager.StopSound(AudioClips.UfoKidnapping, _audioSourceKidnap);
    }

    public void StationaryPhase(float duration)
    {
        UfoLaser.SetActive(true);
        Collider.enabled = true;
        StartCoroutine(StationaryPhaseCoroutine(duration));
        AudioManager.AudioManager.PlaySound(AudioClips.UfoKidnapping, _audioSourceKidnap, 1.0f);
    }
    
    IEnumerator MoveToPosition(Vector3 targetPosition, float speed, bool continuePhases = true)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition); 
        float duration = distance / speed; 

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
       
        if (continuePhases)
            FinishMovement();
    }

    IEnumerator StationaryPhaseCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        FinishStationaryPhase();
    }

    private void FinishMovement()
    {
        StopAllCoroutines();
        _ctx.EnterStationaryPhase();
    }

    private void FinishStationaryPhase()
    {
        StopAllCoroutines();
        _ctx.EnterMovementPhase();
    }
    
    
    public void EndPhase(Transform playerTransform, float offsetY, float speed)
    {
        StopAllCoroutines();
        Vector3 end = playerTransform.position + -playerTransform.forward * 10.0f + playerTransform.right * 10.0f + playerTransform.up * offsetY;
        MovePhase(end, speed, false);
        Destroy(gameObject, 2.0f);
        AudioManager.AudioManager.StopSound(AudioClips.UfoKidnapping);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_ctx.PlayerTag))
        {
            _ctx.OnBeamEnter();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_ctx.PlayerTag))
        {
            _ctx.OnBeamExit();
        }
    }

    private void StartSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        AudioManager.AudioManager.PlaySound(AudioClips.UfoShip, _audioSource, 1.0f);
        
        _audioSourceKidnap = gameObject.AddComponent<AudioSource>();
        _audioSourceKidnap.loop = true;
    }
    
    
    
}
