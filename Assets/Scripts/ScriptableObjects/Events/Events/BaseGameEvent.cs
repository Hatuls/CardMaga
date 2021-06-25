
using UnityEngine;
using System.Collections.Generic;
namespace Unity.Events
{
    #region One Generic
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
    #endregion

    #region Two Generics
    public abstract class BaseGameEvent<T1, T2> : ScriptableObject
    {
        private readonly List<IGameEventListener<T1, T2>> eventListeners = new List<IGameEventListener<T1, T2>>();
        public void Raise(T1 item, T2 item2)
        {
            if (eventListeners.Count > 0)
            {
                for (int i = eventListeners.Count - 1; i >= 0; i--)
                {
                    eventListeners[i]?.OnEventRaised(item, item2);
                }
            }
        }

        public void RegisterListener(IGameEventListener<T1, T2> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void UnRegisterListener(IGameEventListener<T1, T2> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
    #endregion
    #region Three Generics
    public abstract class BaseGameEvent<T1, T2, T3> : ScriptableObject
    {
        private readonly List<IGameEventListener<T1, T2, T3>> eventListeners = new List<IGameEventListener<T1, T2, T3>>();
        public void Raise(T1 item, T2 item2, T3 item3)
        {
            if (eventListeners.Count > 0)
            {
                for (int i = eventListeners.Count - 1; i >= 0; i--)
                {
                    eventListeners[i]?.OnEventRaised(item, item2, item3);
                }
            }
        }

        public void RegisterListener(IGameEventListener<T1, T2, T3> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void UnRegisterListener(IGameEventListener<T1, T2, T3> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
    #endregion
}