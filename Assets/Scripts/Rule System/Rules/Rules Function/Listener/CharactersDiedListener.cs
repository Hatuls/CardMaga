using Battle;
using CardMaga.Rules;
using Keywords;
using Managers;

public class CharactersDiedListener : BaseEndGameRule
{
    private IPlayer _leftPlayerHeal;
    private IPlayer _rightPlayerHeal;

    public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
    {
        base.InitRuleListener(battleManager,ruleLogics);
        _leftPlayerHeal = battleManager.PlayersManager.LeftCharacter;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckLeftPlayerCondition;
        
        _rightPlayerHeal = battleManager.PlayersManager.RightCharacter;
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckRightPlayerCondition;
    }

    private void CheckLeftPlayerCondition(int playerHp)
    {
        if (playerHp <= 0)
        {
            Active(false);
        }
    }
    
    private void CheckRightPlayerCondition(int playerHp)
    {
        if (playerHp <= 0)
        {
            Active(true);
        }
    }

    public override void Dispose()
    {
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckRightPlayerCondition;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckLeftPlayerCondition;
    }

    public CharactersDiedListener(float delayToEndGame) : base(delayToEndGame)
    {
    }
}
