using Observers;
using StateMachine;
using StateMachine.states;
using UnityEngine;

public class Highscore : MonoBehaviour, IObserver<StateDTO>
{
   public Transform player;
   public int highscore;
   public int lastDistance;
   int maxDistance=0;
   int distance;
    private void Update()
    {
        distance = Mathf.RoundToInt(player.position.x - transform.position.x);
        if (distance > maxDistance)
        {
            maxDistance = distance;
        }
    }
    private void Awake()
    {
      highscore = PlayerPrefs.GetInt("Highscore", 0);
      lastDistance = PlayerPrefs.GetInt("LastDistance", 0);
    }

    public void OnNotify(StateDTO dto)
    {
        States state = dto._state;
        if (state == States.PlayerDeath)
        {
            CheckHighscore();
        }
        else if (state == States.StartState)
        {
            maxDistance = 0;
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
        lastDistance = maxDistance;
        PlayerPrefs.SetInt("LastDistance", lastDistance);
        if (maxDistance > highscore)
        {
            highscore = maxDistance;
            PlayerPrefs.SetInt("Highscore", highscore);
        }
        PlayerPrefs.Save();
    }
}
