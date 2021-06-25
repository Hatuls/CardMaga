using Characters.Stats;

namespace Keywords
{
    public class BleedKeyword: KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Bleed;
   
        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount of " + keywordData.GetAmountToApply);

                StatsHandler.GetInstance.AddBleedPoints(isToPlayer,keywordData.GetAmountToApply);
            }
        }
    }
}