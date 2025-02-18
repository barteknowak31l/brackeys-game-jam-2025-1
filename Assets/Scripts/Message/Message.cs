using Observers;
using StateMachine;
using StateMachine.states;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour, IObserver<StateDTO>
{
    public TextMeshPro textMeshPro;
    public MessagesManager messagesManager;
    public void SetMessage(string newMessage)
    {
        textMeshPro.text = newMessage;
    }

    public void OnNotify(StateDTO dto)
    {
        States state = dto._state;
        Variant variant = dto._variant;

        string newMessage = messagesManager.GetMessage(state, variant);
        SetMessage(newMessage);
    }

    private void OnEnable()
    {
        StateMachineManager.instance.AddObserver(this);
    }

    private void OnDisable()
    {
        StateMachineManager.instance.RemoveObserver(this);
    }
  
}
