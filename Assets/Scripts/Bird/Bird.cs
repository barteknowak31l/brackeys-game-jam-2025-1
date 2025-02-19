using StateMachine.states;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    private BirdState _ctx;
    private void Start()
    {
        Destroy(gameObject, _destroyDelay);
    }

    public void Setup(BirdState ctx)
    {
        _ctx = ctx;
    }
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_ctx._playerTag))
        {
            //_ctx.OnAnvilHitPlayer();//zadawanie dmg do zmiany
        }

        Destroy(gameObject);
    }
}
