using Characters.Stats;

namespace Keywords
{
    public class StrengthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Strength;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);
       
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer)
             .GetStats(Keyword)
             .Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer)
                .GetStats(Keyword)
                .Add(data.GetAmountToApply);
            }

        }
    }
}