using Battle;

namespace Keywords
{
    public class HealKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Heal;



        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);
        }
    }
}