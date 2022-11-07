using CardMaga.Battle;
using CardMaga.Battle.Players;
using CardMaga.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.Commands
{
    public class CardsKeywordsCommands : ICommand
    {
        public static event Action<CommandType> OnCardsKeywordsStartedExecuted;

        public static event Action OnCardsKeywordsFinishedExecuted;

        private KeywordCommand[] _keywordCommand;
        public KeywordCommand[] KeywordCommand => _keywordCommand;

        private CommandType _commandType;
        public CardsKeywordsCommands(IEnumerable<KeywordData> keywordDatas, CommandType command)
        {
            _commandType = command;
            var array = keywordDatas.ToArray();
            _keywordCommand = new KeywordCommand[array.Length];
            for (int i = 0; i < array.Length; i++)
                _keywordCommand[i] = new KeywordCommand(array[i], i == 0 ? CommandType.AfterPrevious : CommandType.WithPrevious);
        }


        public void Init(bool isLeft, IPlayersManager playersManager, KeywordManager keywordManager)
        {
            IPlayer currentPlayer = playersManager.GetCharacter(isLeft);

            for (int i = 0; i < KeywordCommand.Length; i++)
                KeywordCommand[i].InitKeywordLogic(currentPlayer, keywordManager.GetLogic(KeywordCommand[i].KeywordType));

        }

        public void Execute()
        {
            OnCardsKeywordsStartedExecuted?.Invoke(_commandType);

            for (int i = 0; i < KeywordCommand.Length; i++)
                KeywordCommand[i].Execute();
            OnCardsKeywordsFinishedExecuted?.Invoke();
        }

        public void Undo()
        {
            OnCardsKeywordsStartedExecuted?.Invoke(_commandType);

            for (int i = KeywordCommand.Length - 1; i >= 0; i--)
                KeywordCommand[i].Undo();
            OnCardsKeywordsFinishedExecuted?.Invoke();
        }
    }
}