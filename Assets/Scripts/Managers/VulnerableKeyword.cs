using Battle;
using Characters.Stats;

namespace Keywords
{
    public class VulnerableKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Vulnerable;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);
        }
    }
}