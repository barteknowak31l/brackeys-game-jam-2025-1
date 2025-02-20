using System.Collections;
using AudioManager;
using StateMachine.states;
using Unity.VisualScripting;
using UnityEngine;

public class Banana : MonoBehaviour
{
    FruitState _fruitState;
    public Vector3 startScale = Vector3.zero;
    public Vector3 targetScale = Vector3.one;
    public float duration = 2f;       
    
    private AudioSource _audioSource;
    private void Start()
    {
        transform.localScale = startScale;
        StartCoroutine(ScaleOverTime(duration));
        _audioSource =  gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    IEnumerator ScaleOverTime(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    public void Setup (FruitState fruitState)
    {
        _fruitState = fruitState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _fruitState.OnFruitHitPlayer();
            AudioManager.AudioManager.PlaySound(AudioClips.BananaSlip, _audioSource, 1.0f);
        }
    }
}
