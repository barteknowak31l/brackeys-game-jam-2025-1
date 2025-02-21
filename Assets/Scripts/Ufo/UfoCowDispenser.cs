using AudioManager;
using StateMachine.states;
using UnityEngine;

public class UfoCowDispenser : MonoBehaviour
{
    public float _orbitRadius;
    public float _orbitSpeed;
    public float _angle = 180.0f;
    private float _ufoYOffset = 0.604f;

    public UfoState _ctx;
    public Transform _playerTransform;
    public bool _endState = false;
    public GameObject _cowPrefab;
    public Cow _cow;
    public Transform _cowSpawnPoint;
    
    private AudioSource _audioSource;

    
    
    public void Setup(UfoState ctx, Transform playerTransform, float offsetY, GameObject cowPrefab)
    {
        _ctx = ctx;
        _playerTransform = playerTransform;
        _orbitRadius = ctx.orbitRadius;
        _orbitSpeed = ctx.orbitSpeed;
        transform.position = new Vector3(transform.position.x, _ufoYOffset + offsetY, transform.position.z);
        _cowPrefab = cowPrefab;
        _cow = null;
        SpawnCow();
        StartSound();
    }
    void Update()
    {
        _angle += _orbitSpeed * Time.deltaTime;

        float x = _playerTransform.position.x + _orbitRadius * Mathf.Cos(_angle);
        float z = _playerTransform.position.z + _orbitRadius * Mathf.Sin(_angle);

        transform.position = new Vector3(x, transform.position.y, z);
        CheckIfInFront();
        CheckIfInBack();
        
    }

    void CheckIfInFront()
    {
        Vector3 toObject = (transform.position - _playerTransform.position);
        toObject.y = 0; 
        toObject.Normalize();

        Vector3 playerForward = _playerTransform.forward;
        playerForward.y = 0; 
        playerForward.Normalize();

        float dot = Vector3.Dot(playerForward, toObject);

        if (dot > 0.99f) 
        {
            ReleaseCow();
        }
    }

    void CheckIfInBack()
    {
        Vector3 toObject = (transform.position - _playerTransform.position);
        toObject.y = 0; 
        toObject.Normalize();

        Vector3 playerBackward = -_playerTransform.forward;
        playerBackward.y = 0; 
        playerBackward.Normalize();

        float dot = Vector3.Dot(playerBackward, toObject);

        if (dot > 0.99f)
        {
            if(_endState)
                Destroy(gameObject);
            else
                SpawnCow();
        }
    }

    public void EndPhase()
    {
        _endState = true;
    }

    private void SpawnCow()
    {
        if (_cow == null)
        {
            _cow = Instantiate(_cowPrefab, _cowSpawnPoint.position, Quaternion.identity).GetComponent<Cow>();
            _cow.transform.parent = _cowSpawnPoint;
            _cow.Setup(_ctx);   
        }
    }

    private void ReleaseCow()
    {
        if (_cow != null)
        {
            _cow.UseGravity();
            _cow = null;   
        }
    }
    
    private void StartSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        AudioManager.AudioManager.PlaySound(AudioClips.UfoShip, _audioSource, 1.0f);
    }
}
