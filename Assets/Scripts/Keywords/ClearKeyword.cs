using CardMaga.Battle;
using CardMaga.Card;
using System.Collections.Generic;

namespace CardMaga.Keywords
{
    public class ClearKeyword : BaseKeywordLogic
    {

        private List<CardTypeData> _leftCharacterSlots;
        private List<CardTypeData> _rightCharacterSlots;

        public ClearKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            KeywordSO.SoundEventSO.PlaySound();

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CraftingHandler craftingSlots = _playersManager.GetCharacter(currentPlayer).CraftingHandler;
                _leftCharacterSlots = GetCardTypeData(craftingSlots);
                craftingSlots.ResetCraftingSlots();
                InvokeKeywordVisualEffect(currentPlayer);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CraftingHandler craftingSlots = _playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                _rightCharacterSlots = GetCardTypeData(craftingSlots);
                craftingSlots.ResetCraftingSlots();
                InvokeKeywordVisualEffect(!currentPlayer);
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



        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                CraftingHandler craftingSlots = _playersManager.GetCharacter(currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_leftCharacterSlots);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                CraftingHandler craftingSlots = _playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                craftingSlots.AssignCraftingSlots(_rightCharacterSlots);
            }
        }
    }
}