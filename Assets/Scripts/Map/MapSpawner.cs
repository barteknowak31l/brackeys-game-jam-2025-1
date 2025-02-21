using Unity.VisualScripting;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    ObjectPooler objectPooler;
    [SerializeField]
    Transform transform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Spawner"))
        {
            objectPooler.SpawnObject(transform);
        }

    }
}
