using UnityEngine;

public class WaterFollower : MonoBehaviour
{
    public Transform player;         
    public float offset = 5f;        
     

    void Start()
    {
    }

    void Update()
    {
        float playerX = player.position.x;


            transform.position = new Vector3(playerX - offset, transform.position.y, transform.position.z);


    }
}
