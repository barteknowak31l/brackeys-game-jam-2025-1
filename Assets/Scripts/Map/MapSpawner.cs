using Unity.VisualScripting;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    ObjectPooler objectPooler;
    [SerializeField]
    Transform spawnPositon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        spawnPositon = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Spawner"))
        {
            objectPooler.SpawnObject(spawnPositon);
        }

    }
}
