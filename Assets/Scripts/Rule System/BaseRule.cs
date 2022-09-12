using System;

namespace CardMaga.Rules
{
    public class BaseRule : IDisposable
    {

        private BaseRuleLogic _logic;
        private BaseRuleListener _listener;

        public BaseRuleLogic Logic
        {
            get => _logic;
        }

        public BaseRuleListener Listener
        {
            get => _listener;
        }  

        public BaseRule(BaseRuleLogic logic,BaseRuleListener listener)
        {
            _logic = logic;
            _listener = listener;
        
            _listener.OnActive += _logic.ActiveRule;
            _listener.OnDeActive += _logic.DeActiveRule;
        }
    
        public void Dispose()
        {
            _listener.OnActive -= _logic.ActiveRule;
            _listener.OnDeActive -= _logic.DeActiveRule;
            _logic.Dispose();
            _listener.Dispose();
        }
    }
}

