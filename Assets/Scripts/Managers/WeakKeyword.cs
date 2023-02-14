using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;

namespace CardMaga.Keywords
{
    public class WeakKeyword : BaseKeywordLogic
    {
        private readonly KeywordData _weak;
        private readonly KeywordCommand _keywordCommand;
        public WeakKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
            _weak = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0); 
            _keywordCommand = new KeywordCommand(_weak, CommandType.WithPrevious);
        }



    

        public override void EndTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            var Weak = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Weak);

            if (Weak.Amount > 0)
            {
                _keywordCommand.InitKeywordLogic(currentCharacterTurn, this);

                gameDataCommands.DataCommands.AddCommand(_keywordCommand);

                InvokeKeywordVisualEffect(currentCharacterTurn.IsLeft,KeywordSO.OnEndTurnVFX);
                InvokeOnKeywordFinished();
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {



            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
                InvokeKeywordVisualEffect(currentPlayer, KeywordSO.OnApplyVFX);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
                InvokeKeywordVisualEffect(!currentPlayer, KeywordSO.OnApplyVFX);
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