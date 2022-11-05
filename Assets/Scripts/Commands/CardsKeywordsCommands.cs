using CardMaga.Battle;
using CardMaga.Battle.Players;
using CardMaga.Card;
using CardMaga.Keywords;
using Characters.Stats;
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


    public class StaminaCostCommand : ICommand
    {
        private CardData _cardData;
        private StaminaHandler _staminaHandler;


        public StaminaCostCommand(CardData cardData)
        {
            _cardData = cardData;
        }
        public void Init(StaminaHandler staminaHandler)
        {
            _staminaHandler = staminaHandler;
        }
        public void Execute()
        {
            _staminaHandler.ReduceStamina(_cardData);
        }

        public void Undo()
        {
            _staminaHandler.AddStamina(_cardData.StaminaCost);
        }
    }
    public class CardTypeCommand : ICommand
    {
        private CraftingHandler _craftingHandler;
        private List<CardTypeData> _previousSlots;
        private bool _toNotify;
        private CardTypeData _cardTypeData;

        public bool ToNotify { get => _toNotify; set => _toNotify = value; }

        public CardTypeCommand(CardData card)
        {
            _cardTypeData = card.CardTypeData;
        }

        public void Init(CraftingHandler craftingHandler)
        {
            _craftingHandler = craftingHandler;

        }
        public void Execute()
        {
            _previousSlots = new List<CardTypeData>(_craftingHandler.CardsTypeData.Count());
            foreach (var item in _craftingHandler.CardsTypeData)
                _previousSlots.Add(item);

            _craftingHandler.AddFront(_cardTypeData, ToNotify);
        }

        public void Undo()
        {
            _craftingHandler.AssignCraftingSlots(_previousSlots);
        }
    }
}