using Unity.VisualScripting;
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
