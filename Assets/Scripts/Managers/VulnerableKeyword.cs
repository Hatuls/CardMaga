using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;

namespace CardMaga.Keywords
{
    public class VulnerableKeyword : BaseKeywordLogic
    {
        public VulnerableKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            var stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Vulnerable);
            if (stat.Amount > 0)
            {
                //vulnerable 
                var vulnerable = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
                var command = new KeywordCommand(vulnerable, CommandType.WithPrevious);
                command.InitKeywordLogic(currentCharacterTurn, this);

                gameDataCommands.DataCommands.AddCommand(command);

                InvokeKeywordVisualEffect(currentCharacterTurn.IsLeft, KeywordSO.OnStartTurnVFX);
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
            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
           
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
        }
    }
}