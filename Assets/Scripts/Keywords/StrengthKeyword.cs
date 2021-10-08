using Characters.Stats;

namespace Keywords
{
    public class StrengthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Strength;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);

                CharacterStatsManager.GetCharacterStatsHandler(isToPlayer)
                    .GetStats(KeywordTypeEnum.Strength)
                    .Add(keywordData.GetAmountToApply);
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);
       
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer)
             .GetStats(GetKeyword)
             .Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer)
                .GetStats(GetKeyword)
                .Add(data.GetAmountToApply);
            }

        }
    }
}