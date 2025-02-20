using System.Collections;
using System.Collections.Generic;
using AudioManager;
using NUnit.Framework;
using StateMachine.states;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _fireForce = 10f;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private float _rotation = 180f;
    [SerializeField] private float _shootInterval = 1.5f;
    [SerializeField] private float _shootTimer = 0f;
    [SerializeField] private List<GameObject> _fruits;
    [SerializeField] private List<Transform> _fireFruitPoints;

    [SerializeField] private FruitState _state;
    
    private AudioSource _audioSource;

    public void Setup(FruitState state)
    {
        _state = state;
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

    }

    void Update()
    {
        {
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= _shootInterval)
            {
                Shoot();
                _shootTimer = 0f;
            }
        }
    }
    public void MovePhase(Vector3 targetPosition, float speed)
    {

        StartCoroutine(MoveToPosition(targetPosition, speed));
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
    private void FinishMovement()
    {
        StopAllCoroutines();
        _state.EnterStationaryPhase();
    }
    public void StationaryPhase(float duration)
    {
        StartCoroutine(StationaryPhaseCoroutine(duration));
    }
    IEnumerator StationaryPhaseCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        FinishStationaryPhase();
    }
    private void FinishStationaryPhase()
    {
        StopAllCoroutines();
        _state.EnterMovementPhase();
    }
    void Shoot()
    {
        var rot = Quaternion.Euler(_rotation, 0, 0);
        CannonBall cannonBall = Instantiate(_fruits[Random.Range(0, _fruits.Count)], _fireFruitPoints[Random.Range(0, _fireFruitPoints.Count)].position, rot).GetComponent<CannonBall>();
        cannonBall.Setup(_state);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        
        AudioManager.AudioManager.PlaySound(AudioClips.Cannon, _audioSource, 1.0f);

        Vector3 shootDirection = Quaternion.AngleAxis(_angle, transform.right) * transform.forward;
        rb.linearVelocity = shootDirection * _fireForce;
    }
    public void EndPhase(Transform playerTransform, float offsetY, float speed)
    {
        StopAllCoroutines();
        Vector3 end = playerTransform.position + -playerTransform.forward * 10.0f + playerTransform.right * 10.0f + playerTransform.up * offsetY;
        MovePhase(end, speed);
        Destroy(gameObject, 5.0f);
    }
}
