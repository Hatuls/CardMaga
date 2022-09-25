using System;
using Battle;

namespace CardMaga.Rules
{
    public abstract class BaseRule : IDisposable
    {
        public event Action OnActive;
        public event Action OnDeActive;

        private BaseRuleLogic[] _ruleLogics;

        public virtual void InitRuleListener(IBattleManager battleManager, BaseRuleLogic[] ruleLogics)
        {
            _ruleLogics = ruleLogics;
        }

        protected void Active()
       {
           OnActive?.Invoke();

           for (int i = 0; i < _ruleLogics.Length; i++)
           {
               if (_ruleLogics[i].CheckCondition())
               {
                   _ruleLogics[i].ActiveRule();
               }
           }
       }

       protected void DeActive()
       {
           OnDeActive?.Invoke();
           
           for (int i = 0; i < _ruleLogics.Length; i++)
           {
               _ruleLogics[i].DeActiveRule();
           }
       }

        public abstract void Dispose();
    }
    
    public abstract class BaseRule<T> : IDisposable
    {
        public event Action<T> OnActive;
        public event Action<T> OnDeActive;

        private BaseRuleLogic<T>[] _ruleLogics;

        public virtual void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<T>[] ruleLogics)
        {
            _ruleLogics = ruleLogics;
        }

        protected void Active(T obj)
        {
            OnActive?.Invoke(obj);

            for (int i = 0; i < _ruleLogics.Length; i++)
            {
                if (_ruleLogics[i].CheckCondition())
                {
                    _ruleLogics[i].ActiveRule(obj);
                }
            }
        }

        protected void DeActive(T obj)
        {
            OnDeActive?.Invoke(obj);
           
            for (int i = 0; i < _ruleLogics.Length; i++)
            {
                _ruleLogics[i].DeActiveRule(obj);
            }
        }

        public abstract void Dispose();
    }
}

