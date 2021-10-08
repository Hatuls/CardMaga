using Characters.Stats;

namespace Keywords
{
    public class BleedKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Bleed;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount of " + keywordData.GetAmountToApply);


                CharacterStatsManager.GetCharacterStatsHandler(isToPlayer).GetStats(GetKeyword).Add(keywordData.GetAmountToApply);

            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);



                switch (data.GetTarget)
                {

                    case TargetEnum.All:
                        CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);
                        CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);
                        break;

                    case TargetEnum.MySelf:
                    case TargetEnum.Opponent:
                        CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(GetKeyword).Add(data.GetAmountToApply);
                        break;
                    case TargetEnum.None:
                    default:
                        break;
                }
            }
        }
    }
}