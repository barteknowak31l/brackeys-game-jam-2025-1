using Observers;
using StateMachine;
using StateMachine.states;
using TMPro;
using UnityEngine;

public class HighscoreBillboard : MonoBehaviour, IObserver<StateDTO>
{
    public TextMeshPro textMeshPro;
    public Highscore highscoreData;

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
        if (state == States.StartState)
        {
            ChangeScores();
        }
    }

    private void ChangeScores()
    {
        textMeshPro.text = $"Highscore:\n{highscoreData.highscore}\nLast score:\n{highscoreData.lastDistance}";
    }
}
