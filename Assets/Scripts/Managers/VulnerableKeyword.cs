using Battle;
using Characters.Stats;

namespace Keywords
{
    public class VulnerableKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Vulnerable;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
   
            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
        }
    }
}