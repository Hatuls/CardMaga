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

        private KeywordManager _keywordManager;
        private IPlayersManager _playersManager;

        public CommandHandler<ICommand> DataCommands => _dataCommands;



        public GameDataCommands(IPlayersManager playersManager, KeywordManager keywordManager)
        {
            _playersManager = playersManager;
            _keywordManager = keywordManager;
            _dataCommands = new CommandHandler<ICommand>();
        }


        public void InsertCardDataCommand(CardData card, bool toReduceStamina, bool toDetectCombo)
        {
            CardCommandsHolder cardCommands = card.CardCommands;

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