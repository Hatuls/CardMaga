using Characters.Stats;

namespace Keywords
{
    public class StaminaKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Stamina;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
  
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

                switch (data.GetTarget)
                {

                    case TargetEnum.All:
                        StaminaHandler.Instance.AddStamina(currentPlayer, data.GetAmountToApply);
                        StaminaHandler.Instance.AddStamina(!currentPlayer, data.GetAmountToApply);
                        break;

                    case TargetEnum.MySelf:
                    case TargetEnum.Opponent:
                        StaminaHandler.Instance.AddStamina(currentPlayer, data.GetAmountToApply);
                        break;

                    case TargetEnum.None:
                    default:
                        break;
                }
            }
        }
    }
}