using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;
using Characters.Stats;

namespace CardMaga.Keywords
{
    public class ShieldKeywordLogic : BaseKeywordLogic
    {
        public ShieldKeywordLogic(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                var target = data.GetTarget;

                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    var characterStatHandler = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                    var dexterity = characterStatHandler.GetStat(KeywordType.Dexterity).Amount;

                    characterStatHandler.GetStat(KeywordType)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                {
                    var characterStatHandler = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                    var dexterity = characterStatHandler.GetStat(KeywordType.Dexterity).Amount;

                    characterStatHandler.GetStat(KeywordType)
                        .Add(
                          dexterity + data.GetAmountToApply
                        );
                }

                InvokeOnKeywordActivated();
                data.KeywordSO.SoundEventSO.PlaySound();
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var characterStatHandler = _playersManager.GetCharacter(currentPlayer).StatsHandler;
                var dexterity = characterStatHandler.GetStat(KeywordType.Dexterity).Amount;

                characterStatHandler.GetStat(KeywordType)
                    .Reduce(
                      dexterity + data.GetAmountToApply
                    );
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
            {
                var characterStatHandler = _playersManager.GetCharacter(!currentPlayer).StatsHandler;
                var dexterity = characterStatHandler.GetStat(KeywordType.Dexterity).Amount;

                characterStatHandler.GetStat(KeywordType)
                    .Reduce(
                      dexterity + data.GetAmountToApply
                    );
            }
        }

        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            BaseStat stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType);

            if (stat.Amount > 0)
                gameDataCommands.DataCommands.AddCommand(new ResetDefenseCommand(stat));
                InvokeOnKeywordFinished();
        }
    }

    public class ResetDefenseCommand : ICommand
    {
        private readonly BaseStat stat;
        private int _previousAmount;
        public ResetDefenseCommand(BaseStat stat)
        {
            this.stat = stat;
        }
        public void Execute()
        {
            _previousAmount = stat.Amount;
            stat.Reset();
        }

        public void Undo()
        {
            stat.Reset(_previousAmount);
        }
    }
}