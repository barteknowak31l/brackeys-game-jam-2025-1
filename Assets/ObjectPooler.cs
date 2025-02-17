using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> objectPool; // Lista gotowych prefabów
    [SerializeField]

    void Start()
    {
        // Dezaktywujemy wszystkie obiekty na start
        foreach (var obj in objectPool)
        {
            obj.SetActive(false);
        }
    }

    public void SpawnObject(Transform spawnLocation)
    {
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy) // Znajdujemy pierwszy nieaktywny obiekt
            {
                obj.transform.position = spawnLocation.position;
                obj.transform.rotation = spawnLocation.rotation;
                obj.SetActive(true); // Aktywujemy obiekt
                break;
            }
        }
    }

    public void DespawnObject(GameObject obj)
    {
        obj.SetActive(false); // Dezaktywujemy obiekt, by wróci³ do puli
    }
}
