using Battle;

namespace Keywords
{
    public class WeakKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Weak;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);
        }
    }
}