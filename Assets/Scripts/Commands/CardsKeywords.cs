
using Battle;
using CardMaga.Card;
using Characters.Stats;
using Keywords;
using Managers;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace CardMaga.Commands
{
    public class CardsKeywords
    {
        private StaminaCostCommand _staminaCommand;
        private KeywordCommand[] _keywordCommand;
        public KeywordCommand[] KeywordCommand => _keywordCommand;
        public StaminaCostCommand StaminaCommand => _staminaCommand;
        public CardsKeywords(CardData cardData)
        {
            KeywordData[] keywords = cardData.CardKeywords;
            keywords.Sort(OrderKeywords);
            _keywordCommand = new KeywordCommand[keywords.Length];
            int previousAnimationIndex = -1;
            CommandType commandType;
            for (int i = 0; i < keywords.Length; i++)
            {
                if (keywords[i].AnimationIndex != previousAnimationIndex)
                {
                    commandType = CommandType.AfterPrevious;
                    previousAnimationIndex++;
                }
                else 
                    commandType = CommandType.WithPrevious;

                _keywordCommand[i] = new KeywordCommand(keywords[i],commandType);
            }

            _staminaCommand = new StaminaCostCommand(cardData);
        }

        private int OrderKeywords(KeywordData myself, KeywordData other)
        {
            if (myself.AnimationIndex == other.AnimationIndex)
                return 0;
            else if (myself.AnimationIndex < other.AnimationIndex)
                return -1;
            else return 1;
        }

        internal void Init(bool isLeft, IPlayersManager playersManager, KeywordManager keywordManager)
        {
            IPlayer currentPlayer = playersManager.GetCharacter(isLeft);

            _staminaCommand.Init(currentPlayer.StaminaHandler);
            
            for (int i = 0; i < KeywordCommand.Length; i++)
                KeywordCommand[i].InitKeywordLogic(currentPlayer, keywordManager.GetLogic(KeywordCommand[i].KeywordType), playersManager);

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