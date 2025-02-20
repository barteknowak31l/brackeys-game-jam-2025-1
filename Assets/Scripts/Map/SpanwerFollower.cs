using UnityEngine;

public class SpanwerFollower : MonoBehaviour
{
    public Transform player;         
    public float offset = 5f;        
     

    void Start()
    {
    }

    void Update()
    {
        float playerX = player.position.x;
        float playerY = player.position.y;

            transform.position = new Vector3(playerX - offset, playerY, transform.position.z);


    }
}
