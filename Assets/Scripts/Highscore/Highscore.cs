using Observers;
using StateMachine;
using StateMachine.states;
using UnityEngine;

public class Highscore : MonoBehaviour, IObserver<StateDTO>
{
   public Transform player;
    int highscore;
   int distance;
    private void Update()
    {
        distance = Mathf.RoundToInt(player.position.x - transform.position.x);
    }
    private void Awake()
    {
      highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    public void OnNotify(StateDTO dto)
    {
        States state = dto._state;
        if (state == States.PlayerDeath)
        {
            CheckHighscore();
        }
    }
    private void OnEnable()
    {
        StateMachineManager.instance.AddObserver(this);
    }

    private void OnDisable()
    {
        StateMachineManager.instance.RemoveObserver(this);
    }


    private void CheckHighscore()
    {
        distance = Mathf.RoundToInt(player.position.x - transform.position.x);
        if (distance > highscore)
        {
            highscore = distance;
            PlayerPrefs.SetInt("Highscore", highscore);
            Debug.Log($"Nowy rekord wynosi {highscore}");
            PlayerPrefs.Save();
        }
    }
}
