
using UnityEngine;
using System.Collections.Generic;
namespace Unity.Events
{

    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();
        public void Raise(T item)
        {
            if (eventListeners.Count > 0)
            {
                for (int i = eventListeners.Count - 1; i >= 0; i--)
                {
                    eventListeners[i]?.OnEventRaised(item);
                }
            }
        }

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void UnRegisterListener(IGameEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }


}