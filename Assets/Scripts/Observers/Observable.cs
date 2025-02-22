using System;
using System.Collections.Generic;
using Observers.dto;
using StateMachine;
using UnityEngine;

namespace Observers
{
    public abstract class Observable<T> : MonoBehaviour where T : DataTransferObject
    {
        private List<IObserver<T>>  _observers ;

        public virtual void Awake()
        {
            _observers = new List<IObserver<T>>();
        }

        public void AddObserver(IObserver<T> o)
        {
            try
            {
                _observers.Add(o);
            }
            catch (Exception e)
            {
                if (StateMachineManager.instance.IsDebugMode)
                    Debug.LogException(e);

            }
        }

        public void RemoveObserver(IObserver<T> o)
        {
            try
            {
                _observers.Remove(o);
            }
            catch (Exception e)
            {
                if (StateMachineManager.instance.IsDebugMode)
                    Debug.LogException(e);

            }
        }

        public void NotifyObservers(T dto)
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify(dto);
            }
        }

    }
}
