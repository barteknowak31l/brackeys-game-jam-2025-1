using StateMachine.states;
using UnityEngine;

public class Banana : MonoBehaviour
{
    FruitState _fruitState;

    public void Setup (FruitState fruitState)
    {
        _fruitState = fruitState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _fruitState.OnFruitHitPlayer();
        }
    }
}
