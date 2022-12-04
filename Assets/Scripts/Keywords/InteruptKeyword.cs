using CardMaga.Battle;
using CardMaga.Card;
using System.Collections.Generic;

namespace CardMaga.Keywords
{

    public class InteruptKeyword : BaseKeywordLogic
    {

        private List<CardTypeData> _leftLastCardType;
        private List<CardTypeData> _rightLastCardType;

        public InteruptKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }



        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + target.ToString() + " recieved " + KeywordSO.GetKeywordType.ToString() + " with Amount of " + amount);

            int length = amount;


            CraftingHandler playerCraftingSlots;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playerCraftingSlots = _playersManager.GetCharacter(currentPlayer).CraftingHandler;
                _leftLastCardType = CardTypeDatas(playerCraftingSlots.CraftingSlots);
                for (int i = 0; i < length; i++)
                {
                    playerCraftingSlots.PushFront(false);
                }
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                playerCraftingSlots = _playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                _rightLastCardType = CardTypeDatas(playerCraftingSlots.CraftingSlots);
                for (int i = 0; i < length; i++)
                {
                    playerCraftingSlots.PushFront(false);

                }
            }
            KeywordSO.SoundEventSO.PlaySound();


            List<CardTypeData> CardTypeDatas(IReadOnlyList<CraftingSlot> craftingSlots)
            {
             
                List<CardTypeData> _cardTypeData = new List<CardTypeData>(craftingSlots.Count);
                for (int i = 0; i < craftingSlots.Count; i++)
                    _cardTypeData.Add(craftingSlots[i].CardType);

                return _cardTypeData;

            }
        }

  
        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            CraftingHandler playerCraftingSlots;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playerCraftingSlots = _playersManager.GetCharacter(currentPlayer).CraftingHandler;
                playerCraftingSlots.AssignCraftingSlots(_leftLastCardType);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                playerCraftingSlots = _playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                playerCraftingSlots.AssignCraftingSlots(_rightLastCardType);
            }
        }
    }
}