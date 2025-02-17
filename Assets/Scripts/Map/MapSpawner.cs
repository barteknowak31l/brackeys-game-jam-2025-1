using Unity.VisualScripting;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    ObjectPooler objectPooler;
    [SerializeField]
    Transform transform;
    [SerializeField]
    bool isSpawned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        transform = GetComponent<Transform>();

    }
    private void OnEnable()
    {
        isSpawned = false;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")&& !isSpawned)
        {
            objectPooler.SpawnObject(transform);
            isSpawned = true;
        }

    }
}
