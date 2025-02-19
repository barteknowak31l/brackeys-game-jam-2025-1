using UnityEngine;

public class Pig : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
