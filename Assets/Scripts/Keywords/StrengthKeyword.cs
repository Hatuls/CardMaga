using Characters.Stats;

namespace Keywords
{
    public class StrengthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Strength;
        //public override void ProcessOnTarget(ref CharacterStats fromTarget, ref CharacterStats toTarget, ref KeywordData keywordData)
        //{
        //    if (keywordData != null )
        //    {
        //        UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
        //        StatsHandler.GetInstance.AddStrength(keywordData.GetAmountToApply);
        //        toTarget.AddStrength(keywordData.GetAmountToApply);
        //    }
        //}
        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null )
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.AddStrength(isToPlayer,keywordData.GetAmountToApply);
             
            }
        }
    }
}