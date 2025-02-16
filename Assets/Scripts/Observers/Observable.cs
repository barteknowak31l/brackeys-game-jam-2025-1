using System;
using System.Collections.Generic;
using Observers.dto;
using UnityEngine;

namespace Observers
{
    public abstract class Observable : MonoBehaviour
    {
        private List<IObserver> _observers;

        private void Awake()
        {
            _observers = new List<IObserver>();
        }

        public void AddObserver(IObserver o)
        {
            _observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            _observers.Remove(o);
        }

        public void NotifyObservers(DataTransferObject dto)
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify(dto);
            }
        }

    }
}
