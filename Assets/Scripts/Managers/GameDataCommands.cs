using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
using System;

namespace CardMaga.Battle.Execution
{
    public class GameDataCommands : IDisposable
    {
        //Handling the data
        private CommandHandler<ICommand> _dataCommands;

        public CommandHandler<ICommand> DataCommands => _dataCommands;



        public GameDataCommands()
        {
            _dataCommands = new CommandHandler<ICommand>();
        }


        public void InsertCardDataCommand(BattleCardData battleCard, bool toReduceStamina, bool toDetectCombo)
        {
            CardCommandsHolder cardCommands = battleCard.CardCommands;

            if (toReduceStamina)
                _dataCommands.AddCommand(cardCommands.StaminaCostCommand);

            cardCommands.CardTypeCommand.ToNotify = toDetectCombo;

            _dataCommands.AddCommand(cardCommands);
        }
        public void ResetAll()
        {
            _dataCommands.ResetCommands();
        }


        public void Dispose()
        {
            ResetAll();

        }
    }


}