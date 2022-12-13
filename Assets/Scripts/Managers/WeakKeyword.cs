using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;

namespace CardMaga.Keywords
{
    public class WeakKeyword : BaseKeywordLogic
    {
        public WeakKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }



    

        public override void EndTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            var Weak = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Weak);
            if (Weak.Amount > 0)
            {
                var command = new StatCommand(Weak, -1);
                gameDataCommands.DataCommands.AddCommand(command);
                InvokeOnKeywordFinished();
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {



            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
                InvokeKeywordVisualEffect(currentPlayer);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            InvokeKeywordVisualEffect(!currentPlayer);
            }

            KeywordSO.SoundEventSO.PlaySound();
            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            KeywordSO.SoundEventSO.PlaySound();
            InvokeOnKeywordActivated();
        }
    }
}