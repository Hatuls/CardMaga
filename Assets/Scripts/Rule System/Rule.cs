using System;
using Battle;

namespace CardMaga.Rules
{
    public class Rule : IDisposable
    {
        public Rule(BaseRuleLogic logic, BaseRuleListener listener, IBattleManager battleManager)
        {
            Logic = logic;
            Listener = listener;

            Listener.InitRuleListener(battleManager);
            Logic.InitRuleLogic(battleManager);

            Listener.OnActive += Logic.ActiveRule;
            Listener.OnDeActive += Logic.DeActiveRule;
        }

        public BaseRuleLogic Logic { get; }

        public BaseRuleListener Listener { get; }

        public void Dispose()
        {
            Listener.OnActive -= Logic.ActiveRule;
            Listener.OnDeActive -= Logic.DeActiveRule;
            Logic.Dispose();
            Listener.Dispose();
        }
    }
    
    public class Rule<T> : IDisposable
    {
        public Rule(BaseRuleLogic<T> logic, BaseRuleListener<T> listener, IBattleManager battleManager)
        {
            Logic = logic;
            Listener = listener;

            Listener.InitRuleListener(battleManager);
            Logic.InitRuleLogic(battleManager);

            Listener.OnActive += Logic.ActiveRule;
            Listener.OnDeActive += Logic.DeActiveRule;
        }

        public BaseRuleLogic<T> Logic { get; }

        public BaseRuleListener<T> Listener { get; }

        public void Dispose()
        {
            Listener.OnActive -= Logic.ActiveRule;
            Listener.OnDeActive -= Logic.DeActiveRule;
            Logic.Dispose();
            Listener.Dispose();
        }
    }
}