using Characters.Stats;

namespace Keywords
{
    public class DexterityKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Dexterity;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
   
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);

            switch (data.GetTarget)
            {
                case TargetEnum.MySelf:
                    break;
                case TargetEnum.All:
                    StatsHandler.GetInstance.AddStrength(currentPlayer, data.GetAmountToApply);
                    StatsHandler.GetInstance.AddStrength(!currentPlayer, data.GetAmountToApply);
                    break;

                case TargetEnum.Opponent:
                    StatsHandler.GetInstance.AddStrength(!currentPlayer, data.GetAmountToApply);
                    break;

                case TargetEnum.None:
                default:
                    break;
            }
        }
    }
}