using Characters.Stats;

namespace Keywords
{
    public class AttackKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Attack;
        //  public override void ProcessOnTarget(ref CharacterStats fromTarget, ref CharacterStats toTarget, ref KeywordData keywordData)
        //{
        //    if (toTarget != null && keywordData != null && fromTarget != null)
        //    {
        //        UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> "+ keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
        //        toTarget.RecieveDamage(fromTarget.GetStrengthPoints + keywordData.GetAmountToApply);
        //    }
        //}  
        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if( keywordData != null )
            {
              //  CameraController.ShakeCamera();

                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> "+ keywordData.GetTarget.ToString() + " recieved " + keywordData.GetKeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.RecieveDamage(
                    isToPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(isFromPlayer).Strength + keywordData.GetAmountToApply
                    );
            }
        }
    }
}