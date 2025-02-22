using Unity.VisualScripting;
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Prop"))
        {
            Destroy(other.gameObject);
        }
    }
}
