using Battle;
using CardMaga.Card;
using System.Collections.Generic;

namespace Keywords
{
    public class ClearKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Clear;
        private List<CardTypeData> _leftCharacterSlots;
        private List<CardTypeData> _rightCharacterSlots;
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
        
            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CraftingHandler craftingSlots = playersManager.GetCharacter(currentPlayer).CraftingHandler;
                _leftCharacterSlots = GetCardTypeData(craftingSlots);
                craftingSlots.ResetCraftingSlots();
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CraftingHandler craftingSlots = playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                _rightCharacterSlots = GetCardTypeData(craftingSlots);
                craftingSlots.ResetCraftingSlots();
            }

             List<CardTypeData> GetCardTypeData(CraftingHandler craftingHandler)
            {
                int length = craftingHandler.CraftingSlots.Count;
                List<CardTypeData> cardTypeDatas = new List<CardTypeData>(length);

                for (int i = 0; i < length; i++)
                    cardTypeDatas.Add(craftingHandler.CraftingSlots[i].CardType);

                return cardTypeDatas;
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CraftingHandler craftingSlots = playersManager.GetCharacter(currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_leftCharacterSlots);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CraftingHandler craftingSlots = playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_rightCharacterSlots);
            }
        }
    }
}