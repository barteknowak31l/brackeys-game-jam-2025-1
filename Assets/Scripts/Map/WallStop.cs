using UnityEngine;

public class WallStop : MonoBehaviour
{
    private void Start()
    {
        Collider colider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MovementController controller = other.GetComponent<MovementController>();
            controller.OnWallEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MovementController controller = other.GetComponent<MovementController>();
            controller.OnWallExit();
        }
    }
}
