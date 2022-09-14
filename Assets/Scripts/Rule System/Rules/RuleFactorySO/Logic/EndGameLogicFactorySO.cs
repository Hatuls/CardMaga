using Battle;
using CardMaga.Rules;

public class EndGameLogicFactorySO : BaseRuleLogicFactorySO<bool>
{
    public override BaseRuleLogic<bool> CreateRuleLogic(IBattleManager battleManager)
    {
        return new EndGameLogic();
    }
}