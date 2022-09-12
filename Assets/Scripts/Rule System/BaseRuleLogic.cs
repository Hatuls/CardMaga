using System;
using Battle;

namespace CardMaga.Rules
{
    public abstract class BaseRuleLogic : IDisposable
    {
        public abstract void InitRuleLogic(BattleManager battleManager);

        public abstract void UpDateRule();

        public abstract void DeActiveRule();
    
        public abstract void ActiveRule();

        public abstract void Dispose();
    }
}

