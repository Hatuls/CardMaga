using Characters.Stats;

namespace Keywords
{
    public class DefenseKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shield;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);


                var target = data.GetTarget;

                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    var characterStatHandler = CharacterStatsManager.GetCharacterStatsHandler(currentPlayer);
                    var dexterity = characterStatHandler.GetStats(KeywordTypeEnum.Dexterity).Amount;

                    characterStatHandler.GetStats(Keyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                {
                    var characterStatHandler = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);
                    var dexterity = characterStatHandler.GetStats(KeywordTypeEnum.Dexterity).Amount;

                    characterStatHandler.GetStats(Keyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }
                data.KeywordSO.SoundEventSO.PlaySound();
            }
        }
    }
}