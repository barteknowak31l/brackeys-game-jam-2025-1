using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Transform firePoint;
    public float fireForce = 10f;
    public float angle = 45f;
    public float rotation = 180f;
    public float shootInterval = 2f;
    private float shootTimer = 0f;
    public List<GameObject> _instantiatedFruit;
    private void Start()
    {
        


    }
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        var rot = Quaternion.Euler(rotation, 0, 0);
        GameObject bullet = Instantiate(_instantiatedFruit[Random.Range(0,_instantiatedFruit.Count)], firePoint.position, rot);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        
        Vector3 shootDirection = Quaternion.AngleAxis(angle, transform.right) * transform.forward;
        rb.linearVelocity = shootDirection * fireForce;
    }
}
