using StateMachine.states;
using UnityEngine;

public class UfoCowDispenser : MonoBehaviour
{
    public float _orbitRadius;
    public float _orbitSpeed;
    public float _angle = 180.0f; 

    public UfoState _ctx;
    public Transform _playerTransform;
    public bool _endState = false;
    public GameObject _cowPrefab;
    public Cow _cow;
    public Transform _cowSpawnPoint;
    
    public void Setup(UfoState ctx, Transform playerTransform, float offsetY, GameObject cowPrefab)
    {
        _ctx = ctx;
        _playerTransform = playerTransform;
        _orbitRadius = ctx.orbitRadius;
        _orbitSpeed = ctx.orbitSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        _cowPrefab = cowPrefab;
        _cow = null;
        SpawnCow();
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
        toObject.y = 0; // Ignorujemy wysokość
        toObject.Normalize();

        Vector3 playerForward = _playerTransform.forward;
        playerForward.y = 0; // Ignorujemy wysokość
        playerForward.Normalize();

        float dot = Vector3.Dot(playerForward, toObject);

        if (dot > 0.99f) // Blisko 1 oznacza, że obiekt jest prawie idealnie przed graczem
        {
            ReleaseCow();
        }
    }

    void CheckIfInBack()
    {
        Vector3 toObject = (transform.position - _playerTransform.position);
        toObject.y = 0; // Ignorujemy wysokość
        toObject.Normalize();

        Vector3 playerBackward = -_playerTransform.forward;
        playerBackward.y = 0; // Ignorujemy wysokość
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
}
