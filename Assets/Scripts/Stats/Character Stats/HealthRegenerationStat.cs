using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;
using CardMaga.Keywords;
namespace Characters.Stats
{
    public class HealthRegenerationStat : BaseStat
    {
        public HealthRegenerationStat(int amount) : base(amount)
        {
        }

        public override void Reduce(int amount)
        {
            if (Amount - amount >= 0)
                base.Reduce(amount);
        }
        public override KeywordType Keyword => KeywordType.Regeneration;
    }


    public class HealthRegenerationKeyword : BaseKeywordLogic
    {
        private BaseKeywordLogic _healLogic;
        private KeywordData keyword;
        private KeywordCommand command;
        public HealthRegenerationKeyword(BaseKeywordLogic healKeyword, KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
            _healLogic = healKeyword;
        }
        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            BaseStat stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Regeneration);
            if (stat.IsEmpty)
                return;
            // Heal
            keyword = new KeywordData(_healLogic.KeywordSO, TargetEnum.MySelf, stat.Amount, 0);
            command = new KeywordCommand(keyword, CommandType.WithPrevious);
            command.InitKeywordLogic(currentCharacterTurn, _healLogic);
            gameDataCommands.DataCommands.AddCommand();

            //Reduce Regen
            keyword = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
            command = new KeywordCommand(keyword, CommandType.WithPrevious);
            command.InitKeywordLogic(currentCharacterTurn, this);
            gameDataCommands.DataCommands.AddCommand(command);
            InvokeKeywordVisualEffect(currentCharacterTurn.IsLeft, KeywordSO.OnApplyVFX);
            if (!stat.HasValue())
                InvokeOnKeywordFinished();
        }

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            KeywordSO.SoundEventSO.PlaySound();

            InvokeKeywordVisualEffect(currentPlayer, KeywordSO.OnApplyVFX);
            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
        }
    }
}