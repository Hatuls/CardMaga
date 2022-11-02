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

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);

            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data )
        {
            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);

            InvokeOnKeywordActivated();
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
    }
}