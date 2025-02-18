using System;
using StateMachine.states;
using UnityEngine;

public class Cow : MonoBehaviour
{
    private UfoState _ctx;
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Setup(UfoState ctx)
    {
        _ctx = ctx;
    }
    
    
    public void UseGravity()
    {
        _rb.useGravity = true;
        _rb.linearVelocity = Vector3.zero;
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_ctx.PlayerTag))
        {
            _ctx.OnCowHit();
        }
        Destroy(gameObject);
    }
}
