using StateMachine.states;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float speed;
    private BeaverState _ctx;
    private int _pigDestroyTime = 20;
    void Start()
    {
        Destroy(gameObject, _pigDestroyTime);
        transform.Rotate(0, 270, 0);
    }
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    public void Setup(BeaverState ctx)
    {
        _ctx = ctx;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_ctx._playerTag))
        {
            _ctx.EndTime();
        }

        Destroy(gameObject);
        
    }

    
}
