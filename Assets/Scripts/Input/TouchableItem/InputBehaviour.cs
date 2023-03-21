using System;
using UnityEngine;

namespace CardMaga.Input
{
    public class InputBehaviour : IEquatable<InputBehaviour>
    {
        public event Action OnClick; 
        public event Action OnBeginHold;
        public event Action OnEndHold;
        public event Action OnHold;
        public event Action OnPointDown;
        public event Action OnPointUp;
        
    
        public virtual void Click()
        {
            OnClick?.Invoke();
        }

        public virtual void Hold()
        {
            OnHold?.Invoke();
        }

        public virtual void BeginHold()
        {
            OnBeginHold?.Invoke();
        }

        public virtual void EndHold()
        {
            OnEndHold?.Invoke();
        }

        public virtual void PointDown()
        {
            OnPointDown?.Invoke();
        }

        public virtual void PointUp()
        {
            OnPointUp?.Invoke();
        }

        public bool Equals(InputBehaviour other)
        {
            if (other == null)
                return false;

            return other == this;
        }
    }
    
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

