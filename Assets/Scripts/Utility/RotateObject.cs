using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 100f;
    
    public bool startRotation = false;
    void Update()
    {
        if(startRotation)   
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
    
}