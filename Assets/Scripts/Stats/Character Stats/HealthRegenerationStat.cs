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
        public HealthRegenerationKeyword(BaseKeywordLogic healKeyword, KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
            _healLogic = healKeyword;
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data == null)
                throw new System.Exception("HealthRegen data is null!!");

            var target = data.GetTarget;


            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();

            InvokeOnKeywordActivated();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;


            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
        }

        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            BaseStat stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Regeneration);
            // Heal
            var keyword = new KeywordData(_healLogic.KeywordSO, TargetEnum.MySelf, stat.Amount, 0);
            var command = new KeywordCommand(keyword, CommandType.WithPrevious);
            command.InitKeywordLogic(currentCharacterTurn, _healLogic);
            gameDataCommands.DataCommands.AddCommand();

            //Reduce Regen
            keyword = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
            command = new KeywordCommand(keyword, CommandType.WithPrevious);
            command.InitKeywordLogic(currentCharacterTurn, this);
            gameDataCommands.DataCommands.AddCommand(command);

            if (!stat.HasValue())
                InvokeOnKeywordFinished();
        }
    }
}