using Characters.Stats;

namespace Keywords
{
    public class StrengthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Strength;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null )
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.AddStrength(isToPlayer,keywordData.GetAmountToApply);
             
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);

            switch (data.GetTarget)
            {
                case TargetEnum.MySelf:
                    StatsHandler.GetInstance.AddStrength(currentPlayer, data.GetAmountToApply);
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