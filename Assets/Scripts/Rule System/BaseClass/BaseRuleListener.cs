using System;
using Battle;

namespace CardMaga.Rules
{
    public abstract class BaseRuleListener : IDisposable
    {
        public event Action OnActive;
    
        public event Action OnDeActive;

        public abstract void InitRuleListener(IBattleManager battleManager);

       // public abstract bool CheckCondition();

       protected void Active()
       {
           OnActive?.Invoke();
       }

       protected void DeActive()
       {
           OnDeActive?.Invoke();
       }

        public abstract void Dispose();
    }
    
    public abstract class BaseRuleListener<T> : IDisposable
    {
        public event Action<T> OnActive;
    
        public event Action<T> OnDeActive;

        public abstract void InitRuleListener(IBattleManager battleManager);

        // public abstract bool CheckCondition();

        protected void Active(T obj)
        {
            OnActive?.Invoke(obj);
        }

        protected void DeActive(T obj)
        {
            OnDeActive?.Invoke(obj);
        }

        public abstract void Dispose();
    }
}

