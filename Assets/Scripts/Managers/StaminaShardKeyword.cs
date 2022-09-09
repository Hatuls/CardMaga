using Battle;

namespace Keywords
{
    public class StaminaShardKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StaminaShards;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;

            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
        }
    }
}