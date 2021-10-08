using Characters.Stats;

namespace Keywords
{
    public class DefenseKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Shield;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);


                var characterStatHandler = CharacterStatsManager.GetCharacterStatsHandler(isToPlayer);
                var dexterity = characterStatHandler.GetStats(KeywordTypeEnum.Dexterity).Amount;

                characterStatHandler.GetStats(GetKeyword)
                    .Add(
                      dexterity + keywordData.GetAmountToApply
                    );

            }
        }

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

                    characterStatHandler.GetStats(GetKeyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                {
                    var characterStatHandler = CharacterStatsManager.GetCharacterStatsHandler(!currentPlayer);
                    var dexterity = characterStatHandler.GetStats(KeywordTypeEnum.Dexterity).Amount;

                    characterStatHandler.GetStats(GetKeyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }


            }
        }
    }
}