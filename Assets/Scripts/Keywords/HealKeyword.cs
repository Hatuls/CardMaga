using Characters.Stats;

namespace Keywords
{
    public class HealKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Heal;



        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);
        }
    }
}