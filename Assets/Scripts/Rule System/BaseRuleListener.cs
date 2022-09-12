using System;
using Battle;

namespace CardMaga.Rules
{
    public abstract class BaseRuleListener : IDisposable
    {
        public event Action OnActive;
    
        public event Action OnDeActive;

        public abstract void InitRuleListener(BattleManager battleManager);

        public abstract bool CheckCondition();

        public abstract void Dispose();
    }
}

