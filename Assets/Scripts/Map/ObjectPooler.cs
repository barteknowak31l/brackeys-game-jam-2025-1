using System.Collections;
using System.Collections.Generic;
using Observers;
using StateMachine;
using StateMachine.states;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, IObserver<StateDTO>
{
    public List<GameObject> objectPool;
    [SerializeField]

    void Start()
    {
        foreach (var obj in objectPool)
        {
            obj.SetActive(false);
        }
    }

    public void SpawnObject(Transform spawnLocation)
    {
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = spawnLocation.position;
                obj.transform.rotation = spawnLocation.rotation;
                obj.SetActive(true);
                break;
            }
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

    public void DespawnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OnNotify(StateDTO dto)
    {
        States state = dto._state;
        if (state == States.PlayerDeath)
        {
            StartCoroutine(PullObject());
        }
    }
    IEnumerator PullObject()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("dzia³a");
        foreach (var obj in objectPool)
        {
            obj.SetActive(false);
        }
    }
}
