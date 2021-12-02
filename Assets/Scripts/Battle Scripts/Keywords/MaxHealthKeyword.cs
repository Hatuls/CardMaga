using Characters.Stats;

namespace Keywords
{
    public class MaxHealthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.MaxHealth;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1*data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}