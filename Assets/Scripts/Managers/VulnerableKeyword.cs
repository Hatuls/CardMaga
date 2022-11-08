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
            var characterStats = currentCharacterTurn.StatsHandler;
            var vulnrable = characterStats.GetStat(KeywordType.Vulnerable);
            if (vulnrable.Amount > 0)
            {
                var command = new StatCommand(vulnrable, -1);
                gameDataCommands.DataCommands.AddCommand(command);
                InvokeOnKeywordFinished();
            }
        }
    
    }
}