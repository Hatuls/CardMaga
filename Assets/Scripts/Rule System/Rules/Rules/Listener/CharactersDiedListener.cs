using Battle;
using CardMaga.Rules;
using Characters.Stats;
using Keywords;
using Managers;

public class CharactersDiedListener : BaseRule<bool>
{
    private IPlayer _leftPlayerHeal;
    private IPlayer _rightPlayerHeal;

    public void InitRuleListener(IBattleManager battleManager)
    {
        _leftPlayerHeal = battleManager.PlayersManager.LeftCharacter;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckLeftPlayerCondition;
        
        _rightPlayerHeal = battleManager.PlayersManager.RightCharacter;
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckRightPlayerCondition;
    }

    private void CheckLeftPlayerCondition(int playerHp)
    {
        if (playerHp < 0)
        {
            Active(true);
        }
    }
    
    private void CheckRightPlayerCondition(int playerHp)
    {
        if (playerHp < 0)
        {
            Active(false);
        }
    }

    public override void Dispose()
    {
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckRightPlayerCondition;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckLeftPlayerCondition;
    }
}
