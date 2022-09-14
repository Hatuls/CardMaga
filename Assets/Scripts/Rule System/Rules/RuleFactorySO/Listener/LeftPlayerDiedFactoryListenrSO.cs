using Battle;
using CardMaga.Rules;

public class LeftPlayerDiedFactoryListenrSO : BaseRuleListenerFactorySO<bool>
{
    protected override BaseRuleListener<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new CharactersDiedListener();
    }
}