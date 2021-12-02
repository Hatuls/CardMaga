using Characters.Stats;

namespace Keywords
{
    public class DexterityKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Dexterity;

   

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);

            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);
            data.KeywordSO.PlaySound();
        }
    }
}