using Characters.Stats;

namespace Keywords
{
    public class BleedKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Bleed;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer).GetStats(Keyword).Add(data.GetAmountToApply);

            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}