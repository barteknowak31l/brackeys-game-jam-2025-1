using System;
using System.Collections.Generic;
using Observers.dto;
using UnityEngine;

namespace Observers
{
    public abstract class Observable<T> : MonoBehaviour where T : DataTransferObject
    {
        private List<IObserver<T>>  _observers ;

        private void Awake()
        {
            _observers = new List<IObserver<T>>();
        }

        public void AddObserver(IObserver<T> o)
        {
            _observers.Add(o);
        }

        public void RemoveObserver(IObserver<T> o)
        {
            _observers.Remove(o);
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
