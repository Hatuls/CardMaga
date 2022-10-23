using Battle;

namespace Keywords
{
    public class DefenseKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shield;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data != null)
            {
                //UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);


                var target = data.GetTarget;

                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    var characterStatHandler = playersManager.GetCharacter(currentPlayer).StatsHandler;
                    var dexterity = characterStatHandler.GetStat(KeywordTypeEnum.Dexterity).Amount;

                    characterStatHandler.GetStat(Keyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                {
                    var characterStatHandler = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                    var dexterity = characterStatHandler.GetStat(KeywordTypeEnum.Dexterity).Amount;

                    characterStatHandler.GetStat(Keyword)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }
                data.KeywordSO.SoundEventSO.PlaySound();
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var characterStatHandler = playersManager.GetCharacter(currentPlayer).StatsHandler;
                var dexterity = characterStatHandler.GetStat(KeywordTypeEnum.Dexterity).Amount;

                characterStatHandler.GetStat(Keyword)
                    .Reduce(
                      dexterity + data.GetAmountToApply
                    );
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
            {
                var characterStatHandler = playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var dexterity = characterStatHandler.GetStat(KeywordTypeEnum.Dexterity).Amount;

                characterStatHandler.GetStat(Keyword)
                    .Reduce(
                      dexterity + data.GetAmountToApply
                    );
            }
        }
    }
}