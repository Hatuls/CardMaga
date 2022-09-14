using Battle;
using CardMaga.Rules;

public class CharactersDiedRuleFactorySO : BaseRuleFactorySO<bool>
{
    protected override BaseRule<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new CharactersDiedListener();
    }
}