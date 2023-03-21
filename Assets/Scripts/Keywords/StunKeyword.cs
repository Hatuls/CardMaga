using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Commands;
using Characters.Stats;
using System;
using System.Collections;

namespace CardMaga.Keywords
{
    public class StunKeyword : BaseKeywordLogic
    {
        public override int Priority => int.MaxValue; 
        public StunKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void StartTurnEffect(IPlayer currentCharacterTurn, GameDataCommands gameDataCommands)
        {
            BaseStat stat = currentCharacterTurn.StatsHandler.GetStat(KeywordType.Stun);
            if (stat.Amount > 0)
            {
                //Stun
                var stunData = new KeywordData(KeywordSO, TargetEnum.MySelf, -1, 0);
                var command = new KeywordCommand(stunData, CommandType.WithPrevious);
                command.InitKeywordLogic(currentCharacterTurn, this);
                gameDataCommands.DataCommands.AddCommand(command);
                currentCharacterTurn.MyTurn.SkipTurn(); 
                currentCharacterTurn.EndTurnHandler.TutorialEndPressed(); // NOT SUPPOSE TO BE HERE!!!!
                InvokeOnKeywordFinished();
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }

            InvokeOnKeywordActivated();
            KeywordSO.SoundEventSO.PlaySound();
        }

   
        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }
        }
    }
}