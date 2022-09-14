using Battle;
using CardMaga.Rules;
using Characters.Stats;
using Keywords;
using Managers;

public class CharactersDiedListener : BaseRuleListener<bool>
{
    private IPlayer _leftPlayerHeal;
    private IPlayer _rightPlayerHeal;

    public override void InitRuleListener(IBattleManager battleManager)
    {
        _leftPlayerHeal = battleManager.PlayersManager.LeftCharacter;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckCondition;
        
        _rightPlayerHeal = battleManager.PlayersManager.RightCharacter;
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged += CheckCondition;
    }

    private void CheckCondition(int playerHp)
    {
        if (playerHp < 0)
        {
            
        }
    }

    public override void Dispose()
    {
        _rightPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckCondition;
        _leftPlayerHeal.StatsHandler.GetStats(KeywordTypeEnum.Heal).OnValueChanged -= CheckCondition;
    }
}
