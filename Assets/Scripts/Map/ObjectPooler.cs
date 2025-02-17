using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> objectPool; 
    [SerializeField]

    void Start()
    {
        foreach (var obj in objectPool)
        {
            obj.SetActive(false);
        }
    }

    public void SpawnObject(Transform spawnLocation)
    {
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy) 
            {
                obj.transform.position = spawnLocation.position;
                obj.transform.rotation = spawnLocation.rotation;
                obj.SetActive(true);
                break;
            }
        }
    }

    public void DespawnObject(GameObject obj)
    {
        obj.SetActive(false); 
    }
}
