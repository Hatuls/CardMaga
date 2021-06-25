using Characters.Stats;

namespace Keywords
{
    public class HealKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Heal;
        //public override void ProcessOnTarget(ref CharacterStats fromTarget, ref CharacterStats toTarget, ref  KeywordData keywordData)
        //{
        //    if (toTarget != null && keywordData != null && fromTarget != null)
        //    {
        //        UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
        //        toTarget.AddHealh(keywordData.GetAmountToApply);
        //    }
        //}
        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if ( keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.AddHealh(isToPlayer,keywordData.GetAmountToApply);
            }
        }

    }
}