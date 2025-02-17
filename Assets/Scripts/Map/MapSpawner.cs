using Unity.VisualScripting;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    ObjectPooler objectPooler;
    [SerializeField]
    Transform transform;
    [SerializeField]
    GameObject mapTitle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            objectPooler.SpawnObject(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectPooler.DespawnObject(mapTitle);
        }
    }
}
