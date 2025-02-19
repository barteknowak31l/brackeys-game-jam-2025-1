using StateMachine.states;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float lifeTime = 5f;
    FruitState _state;

    public void Setup(FruitState state)
    {
        _state = state;
    }
    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }
}
