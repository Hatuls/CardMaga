using Characters.Stats;

namespace Keywords
{
    public class HealKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Heal;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if ( keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                CharacterStatsManager.GetCharacterStatsHandler(isToPlayer).GetStats(GetKeyword).Add(keywordData.GetAmountToApply);
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);
        }
    }
}