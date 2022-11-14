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

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);

            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data )
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
        }
        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {


            var stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Vulnerable);
            if (stat.Amount > 0)
            {
                var vulnerableKeyword = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
                var command = new KeywordCommand(vulnerableKeyword, CommandType.WithPrevious);
                command.InitKeywordLogic(currentCharacterTurn, this);
                gameDataCommands.DataCommands.AddCommand(command);
                //Bleed 

                InvokeOnKeywordFinished();
            }
        }
    
    }
}