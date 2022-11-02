using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;
using Characters.Stats;

namespace CardMaga.Keywords
{
    public class BleedKeyword : BaseKeywordLogic
    {
        private PierceDamageKeyword _pierceDamageKeyword;
        public BleedKeyword(PierceDamageKeyword pierceDamageKeyword,KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
            _pierceDamageKeyword = pierceDamageKeyword;
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            //UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);

            InvokeOnKeywordActivated();
            data.KeywordSO.SoundEventSO.PlaySound();
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
            BaseStat stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Bleed);
            if (stat.Amount > 0)
            {
                var pierceDamageKeyword = new KeywordData(_pierceDamageKeyword.KeywordSO, TargetEnum.MySelf, stat.Amount, 0);
                var command = new KeywordCommand(pierceDamageKeyword, CommandType.WithPrevious);
                command.InitKeywordLogic(currentCharacterTurn, _pierceDamageKeyword);
                gameDataCommands.DataCommands.AddCommand(command);
                //Bleed 
                var bleedDamage = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
                command = new KeywordCommand(bleedDamage, CommandType.WithPrevious);
                command.InitKeywordLogic(currentCharacterTurn, this);

                gameDataCommands.DataCommands.AddCommand(command);

                InvokeOnKeywordFinished();
            }
        }
    }
}