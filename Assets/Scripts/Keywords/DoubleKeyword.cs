using Battle;
using CardMaga.Card;
using System.Collections.Generic;

namespace Keywords
{

    public class DoubleKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Double;
        private List<CardTypeData> _leftCardTypeDatas;
        private List<CardTypeData> _rightCardTypeDatas;
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;
            var characters = playersManager;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var craftingSlots = characters.GetCharacter(currentPlayer).CraftingHandler;
                _leftCardTypeDatas = CardTypeDatas(craftingSlots.CraftingSlots);
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false) ;
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var craftingSlots = characters.GetCharacter(!currentPlayer).CraftingHandler;
                _rightCardTypeDatas = CardTypeDatas(craftingSlots.CraftingSlots);
                craftingSlots.AddFront(craftingSlots.LastCardTypeData, false);
            }


            List<CardTypeData> CardTypeDatas(IReadOnlyList<CraftingSlot> craftingSlots)
            {
                int length = craftingSlots.Count;
                List<CardTypeData> _cardTypeData = new List<CardTypeData>(length);
                for (int i = 0; i < length; i++)
                    _cardTypeData.Add(craftingSlots[i].CardType);

                return _cardTypeData;
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            var characters = playersManager;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                var craftingSlots = characters.GetCharacter(currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_leftCardTypeDatas);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var craftingSlots = characters.GetCharacter(!currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_rightCardTypeDatas);
            }

        }
    }
}