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

                StatsHandler.GetInstance.AddBleedPoints(isToPlayer, keywordData.GetAmountToApply);
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
                        StatsHandler.GetInstance.AddBleedPoints(currentPlayer, data.GetAmountToApply);
                        StatsHandler.GetInstance.AddBleedPoints(!currentPlayer, data.GetAmountToApply);
                        break;

                    case TargetEnum.MySelf:
                    case TargetEnum.Opponent:
                        StatsHandler.GetInstance.AddBleedPoints(currentPlayer, data.GetAmountToApply);
                        break;
                    case TargetEnum.None:
                    default:
                        break;
                }
            }
        }
    }
}