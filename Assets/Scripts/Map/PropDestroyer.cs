using Unity.VisualScripting;
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Prop"))
        {
            Destroy(collision.gameObject);
        }
    }
}

