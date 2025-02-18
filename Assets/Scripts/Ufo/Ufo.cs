using System.Collections;
using StateMachine.states;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    private UfoState _ctx;
    public CapsuleCollider Collider;
    
    public void Setup(UfoState ctx)
    {
        _ctx = ctx;
    }

    public void MovePhase(Vector3 targetPosition, float speed)
    {
        Collider.enabled = false;
        _ctx.OnBeamExit();
        StartCoroutine(MoveToPosition(targetPosition, speed));
    }

    public void StationaryPhase(float duration)
    {
        Collider.enabled = true;
        StartCoroutine(StationaryPhaseCoroutine(duration));
    }
    
    IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
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
        MovePhase(end, speed);
        Destroy(gameObject, 5.0f);
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
    
    
}
