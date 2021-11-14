using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    [CreateAssetMenu (fileName = "Observer", menuName = "ScriptableObjects/DesignPattern/Observer")]
    public class ObserverSO : ScriptableObject, ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        public void Notify(IObserver theOneWhoDidIt)
        {
            for (int i = 0; i < _observers.Count; i++)
                _observers[i].OnNotify(theOneWhoDidIt);
        }

        public void Subscribe(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnSubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }


}