using StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private StateMachineManager _stateMachineBehaviour;
    private bool _active = false;
    void Start()
    {
        _collider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        _active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _active == false)
        {
            Debug.Log("COLLIDER!!!!!!!!!!!");
            _stateMachineBehaviour.EndMechanicState();
            _active = true;
        }
    }
}

