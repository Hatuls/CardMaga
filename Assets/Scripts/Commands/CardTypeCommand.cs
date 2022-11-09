using CardMaga.Card;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.Commands
{
    public class CardTypeCommand : ICommand
    {
        private CraftingHandler _craftingHandler;
        private List<CardTypeData> _previousSlots;
        private bool _toNotify;
        private CardTypeData _cardTypeData;

        public bool ToNotify { get => _toNotify; set => _toNotify = value; }

        public CardTypeCommand(BattleCardData battleCard)
        {
            _cardTypeData = battleCard.CardTypeData;
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