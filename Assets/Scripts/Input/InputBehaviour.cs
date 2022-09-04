using System;
using UnityEngine;

namespace CardMaga.Input
{
    public class InputBehaviour<T> : IEquatable<InputBehaviour<T>>
    {
        public event Action<T> OnClick; 
        public event Action<T> OnBeginHold;
        public event Action<T> OnEndHold;
        public event Action<T> OnHold;
        public event Action<T> OnPointDown;
        public event Action<T> OnPointUp;
    
        public virtual void Click(T obj)
        {
            OnClick?.Invoke(obj);
        }

        public virtual void Hold(T obj)
        {
            OnHold?.Invoke(obj);
        }

        public virtual void BeginHold(T obj)
        {
            OnBeginHold?.Invoke(obj);
        }

        public virtual void EndHold(T obj)
        {
            OnEndHold?.Invoke(obj);
        }

        public virtual void PointDown(T obj)
        {
            OnPointDown?.Invoke(obj);
        }

        public virtual void PointUp(T obj)
        {
            OnPointUp?.Invoke(obj);
        }

        public bool Equals(InputBehaviour<T> other)
        {
            if (other == null)
                return false;

            return other == this;
        }
    }
}

