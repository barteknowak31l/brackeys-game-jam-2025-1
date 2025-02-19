using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }
}
