using Battle;
using CardMaga.Rules;

public class RightPlayerDiedFactoryListenerSO : BaseRuleListenerFactorySO<bool>
{
    protected override BaseRuleListener<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new RightPlayerDiedListener();
    }
}