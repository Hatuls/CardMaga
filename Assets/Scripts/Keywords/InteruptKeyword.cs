using Battle;
using CardMaga.Card;
using System.Collections.Generic;

namespace Keywords
{

    public class InteruptKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Interupt;

        private List<CardTypeData> _leftLastCardType;
        private List<CardTypeData> _rightLastCardType;
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

                int length = data.GetAmountToApply;
                var target = data.GetTarget;
        
                CraftingHandler playerCraftingSlots;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    playerCraftingSlots = playersManager.GetCharacter(currentPlayer).CraftingHandler;
                        _leftLastCardType= CardTypeDatas(playerCraftingSlots.CraftingSlots);
                    for (int i = 0; i < length; i++)
                    {
                        playerCraftingSlots.PushFront(false);
                    }
                }

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                {
                    playerCraftingSlots = playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                    _rightLastCardType = CardTypeDatas(playerCraftingSlots.CraftingSlots);
                    for (int i = 0; i < length; i++)
                    {
                        playerCraftingSlots.PushFront(false);

                    }
                }
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Error Keyword Data is Null!! at "+ Keyword.ToString());


            List<CardTypeData> CardTypeDatas (IReadOnlyList<CraftingSlot> craftingSlots)
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

            CraftingHandler playerCraftingSlots;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playerCraftingSlots = playersManager.GetCharacter(currentPlayer).CraftingHandler;
                playerCraftingSlots.AssignCraftingSlots(_leftLastCardType);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                playerCraftingSlots = playersManager.GetCharacter(!currentPlayer).CraftingHandler;
                playerCraftingSlots.AssignCraftingSlots(_rightLastCardType);
            }
        }
    }
}