using Characters.Stats;

namespace Keywords
{
    public class DefenseKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Defense;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.AddShield(isToPlayer, keywordData.GetAmountToApply);
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);

                var currentPlayerDexterity = StatsHandler.GetInstance.GetCharacterStats(currentPlayer).Dexterity;
                switch (data.GetTarget)
                {
                    case TargetEnum.MySelf:
                        StatsHandler.GetInstance.AddShield(currentPlayer, currentPlayerDexterity + data.GetAmountToApply);
                        break;
                    case TargetEnum.All:

                        StatsHandler.GetInstance.AddShield(currentPlayer, currentPlayerDexterity + data.GetAmountToApply);
                        StatsHandler.GetInstance.AddShield(!currentPlayer, StatsHandler.GetInstance.GetCharacterStats(!currentPlayer).Dexterity+ data.GetAmountToApply);
                        break;

                    case TargetEnum.Opponent:
                        StatsHandler.GetInstance.AddShield(!currentPlayer, currentPlayerDexterity+ data.GetAmountToApply);
                        break;

                    case TargetEnum.None:
                    default:
                        break;
                }
            }
        }
    }
}