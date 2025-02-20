using UnityEngine;

public class MapDestroyer : MonoBehaviour
{
    [SerializeField]
    GameObject mapTitle;
    [SerializeField]
    ObjectPooler objectPooler;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawner"))
        {
            objectPooler.DespawnObject(mapTitle);
        }
    }
}
