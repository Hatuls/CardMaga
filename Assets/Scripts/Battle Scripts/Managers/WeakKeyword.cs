using Characters.Stats;

namespace Keywords
{
    public class WeakKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Weak;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);
            

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);
        }
    }
}