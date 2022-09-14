using Battle;
using CardMaga.Rules;
using Characters.Stats;
using Keywords;

public class RightPlayerDiedListener : BaseRuleListener<bool>
{
    private BaseStat _heal;

    public override void InitRuleListener(IBattleManager battleManager)
    {
        _heal = battleManager.PlayersManager.RightCharacter.StatsHandler.GetStats(KeywordTypeEnum.Heal);
        _heal.OnValueChanged += CheckCondition;
    }

    private void CheckCondition(int playerHp)
    {
        if (playerHp < 0) Active(false);
    }

    public override void Dispose()
    {
        _heal.OnValueChanged -= CheckCondition;
    }
}