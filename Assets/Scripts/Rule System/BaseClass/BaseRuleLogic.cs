using System;
using Battle;

namespace CardMaga.Rules
{
    public abstract class BaseRuleLogic : IDisposable
    {
        public abstract void InitRuleLogic(IBattleManager battleManager);

        public abstract bool CheckCondition();

        public abstract void UpDateRule();

        public abstract void DeActiveRule();
    
        public abstract void ActiveRule();

        public abstract void Dispose();
    }
    
    public abstract class BaseRuleLogic<T> : IDisposable
    {
        public abstract void InitRuleLogic(IBattleManager battleManager);
        
        public abstract bool CheckCondition();

        public abstract void UpDateRule();

        public abstract void DeActiveRule(T obj);
    
        public abstract void ActiveRule(T obj);

        public abstract void Dispose();
    }
    
	public abstract class BaseBoolRuleLogic : BaseRuleLogic<bool>
	{
		
	}
}

