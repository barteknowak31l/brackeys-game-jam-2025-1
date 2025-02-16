using System;
using Observers.dto;
using UnityEngine;

namespace Observers
{
    public class WindObserver : MonoBehaviour, IObserver
    {
        public WindState windState;


        private void OnEnable()
        {
            windState.AddObserver(this);
        }

        private void OnDisable()
        {
            windState.RemoveObserver(this);
        }



        public void OnNotify<T>(T dto) where T : DataTransferObject
        {
            if (dto is WindDTO windDTO)
            {
                Debug.Log("received WindDTO: " +  + windDTO._speed + " " + windDTO._direction);
            }
            else
            {
                Debug.LogError("received wrong DTO: " + dto.GetType().Name);
            }

        }
    }
}
