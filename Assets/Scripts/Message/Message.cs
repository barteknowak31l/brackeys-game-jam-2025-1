using Observers;
using StateMachine;
using StateMachine.states;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour, IObserver<StateDTO>
{
    public TextMeshPro textMeshPro;
    private string message;


    private void OnEnable()
    {
        StateMachineManager.instance.AddObserver(this);
    }

    private void OnDisable()
    {
        StateMachineManager.instance.RemoveObserver(this);
    }
    public void OnNotify(StateDTO dto)
    {
        States state = dto._state;
        Variant variant = dto._variant;
    }

    public void SetMessage(string newMessage)
    {
        message = newMessage;
        textMeshPro.text = message;
    }
}
