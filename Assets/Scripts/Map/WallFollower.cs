using Observers;
using StateMachine;
using StateMachine.states;
using UnityEngine;


public class WallFollower : MonoBehaviour, IObserver<StateDTO>
{
    [SerializeField] private Transform _player;
    [SerializeField] private float distance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float offset = 5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = _player.position.x;
        float playerY = _player.position.y;
        distance = playerX - offset;
        if (distance > maxDistance)
        {
            maxDistance = distance;
        }
        transform.position = new Vector3(maxDistance, playerY, transform.position.z);
    }
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
        Debug.Log("Dostaje wiadomoœæ");
        States state = dto._state;
        if (state == States.StartState)
        {
            Debug.Log("Dzia³a State");
            ResetPosition();
        }
    }
    private void ResetPosition()
    {
        float playerY = _player.position.y;
        maxDistance = -9.495f;
        transform.position = new Vector3(maxDistance, playerY, transform.position.z);
        Debug.Log("Dzia³a reset");
    }
}
