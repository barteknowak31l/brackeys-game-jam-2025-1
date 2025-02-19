using Observers;
using StateMachine;
using StateMachine.states;
using System.Collections;
using UnityEngine;

public class BirdState : Observable<BirdDTO>, IBaseState
{

    private Variant _variant;
    public string _playerTag;
    private Transform _playerTransform;

    [SerializeField] private float _birdSpawnDistance;
    [SerializeField] private float _birdSideOffset;
    [SerializeField] private float _birdSpawnTimeBase;
    [SerializeField] private float _birdSpawnTimeOffset;
    [SerializeField] private float _birdDamage;
    [SerializeField] private float _birdDamageVariant2;

    [Space]
    [SerializeField] private GameObject _littleBirdPrefab;
    [SerializeField] private GameObject _bigBirdPrefab;

    public void EnterState(StateMachineManager ctx)
    {
        //var dto = new BirdDTO().Damage(10);
        //NotifyObservers(dto);
        Debug.Log("Wariant ptaka - " + _variant.ToString());
        _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
        StartCoroutine(SpawnBirdCoroutine());
    }

    public void ExitState(StateMachineManager ctx)
    {
        StopAllCoroutines();
    }

    public States GetStateType()
    {
        return States.Bird;
    }

    public Variant GetVariant()
    {
        return _variant;
    }

    public IBaseState SetVariant(Variant variant)
    {
        _variant = variant;
        return this;
    }

    public void UpdateState(StateMachineManager ctx)
    {
        
    }

    private void SpawnBird()
    {
        
        Vector3 position = _playerTransform.position + _playerTransform.forward * _birdSpawnDistance;

        float side = Random.Range(0,2) == 0 ? -1f : 1f;
        position += _playerTransform.right * side * _birdSideOffset;

        var prefab = _variant == Variant.First ? _littleBirdPrefab : _bigBirdPrefab;
        var rotation = Quaternion.Euler(0, 0, 0);
        if (side == -1f)
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
            
        
        var bird = Instantiate(prefab, position, rotation).GetComponent<Bird>();
        bird.Setup(this);
    }

    public void OnBirdHitPlayer()
    {
        var damage = _variant == Variant.First ? _birdDamage : _birdDamageVariant2;
        BirdDTO birdDTO = new BirdDTO().Damage(damage);
        NotifyObservers(birdDTO);
    }

    IEnumerator SpawnBirdCoroutine()
    {
        while (true)
        {
            var spawnTime = _birdSpawnTimeBase + Random.Range(-_birdSpawnTimeOffset,_birdSpawnTimeOffset);
            yield return new WaitForSeconds(spawnTime);
            SpawnBird();
        }
    }
}
