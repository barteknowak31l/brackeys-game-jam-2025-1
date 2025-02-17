using UnityEngine;

public class WallFollower : MonoBehaviour
{
    public Transform player;         
    public float offset = 5f;        
    private float lastPlayerX;       

    void Start()
    {
        lastPlayerX = player.position.x;
    }

    void Update()
    {
        float playerX = player.position.x;

        
        if (playerX > lastPlayerX)
        {
            transform.position = new Vector3(playerX - offset, transform.position.y, transform.position.z);
        }

        lastPlayerX = playerX;
    }
}
