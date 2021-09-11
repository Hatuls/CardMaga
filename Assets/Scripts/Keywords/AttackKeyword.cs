using Characters.Stats;

namespace Keywords
{
    public class AttackKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Attack;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if( keywordData != null )
            {
              //  CameraController.ShakeCamera();

                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> "+ keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.RecieveDamage(
                    isToPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(isFromPlayer).Strength + keywordData.GetAmountToApply
                    );
            }
        }
    }
}