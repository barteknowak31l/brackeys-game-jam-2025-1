using StateMachine.states;
using System;
using System.Collections;
using UnityEngine;

public class Beaver : MonoBehaviour
{

    private bool _playerInRange = false;
    private bool _isMoving = false;
    private BeaverState _ctx;
    public float timeToKill = 15f;

    public void Start()
    {
        StartCoroutine(Timer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
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
