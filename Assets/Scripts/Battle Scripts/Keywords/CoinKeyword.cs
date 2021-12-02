using Characters.Stats;

namespace Keywords
{
    public class CoinKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Coins;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            if (target == TargetEnum.All|| target == TargetEnum.MySelf)
            {
                var gold = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1*data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var gold = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword);
                if (data.GetAmountToApply > 0)
                    gold.Add(data.GetAmountToApply);
                else
                    gold.Reduce(-1 * data.GetAmountToApply);
            }
        }
    }
}