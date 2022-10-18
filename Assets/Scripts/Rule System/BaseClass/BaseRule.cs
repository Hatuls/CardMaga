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
            if (ruleLogics != null)
                _ruleLogics = ruleLogics;
        }

        protected virtual void Active(T obj)
        {
            OnActive?.Invoke(obj);

            if (_ruleLogics == null)
                return;
            
            for (int i = 0; i < _ruleLogics.Length; i++)
            {
                if (_ruleLogics[i].CheckCondition())
                {
                    _ruleLogics[i].ActiveRule(obj);
                }
            }
        }

        protected virtual void DeActive(T obj)
        {
            OnDeActive?.Invoke(obj);
           
            if (_ruleLogics == null)
                return;
            
            for (int i = 0; i < _ruleLogics.Length; i++)
            {
                _ruleLogics[i].DeActiveRule(obj);
            }
        }

        public abstract void Dispose();
    }

    public abstract class BaseEndGameRule : BaseRule<bool>
    {
        public event Action<float, bool> OnEndGameRuleActive; 

        private float _delayToEndGame;

        public float DelayToEndGame
        {
            get => _delayToEndGame;
        }
        
        public BaseEndGameRule(float delayToEndGame)
        {
            _delayToEndGame = delayToEndGame;
        }

        protected override void Active(bool obj)
        {
            base.Active(obj);
            OnEndGameRuleActive?.Invoke(_delayToEndGame,obj);
        }
    }
}

