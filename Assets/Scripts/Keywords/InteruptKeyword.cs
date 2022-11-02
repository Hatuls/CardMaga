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

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

                int length = data.GetAmountToApply;
                var target = data.GetTarget;

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
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Error Keyword Data is Null!! at " + KeywordType.ToString());


            List<CardTypeData> CardTypeDatas(IReadOnlyList<CraftingSlot> craftingSlots)
            {
                int length = craftingSlots.Count;
                List<CardTypeData> _cardTypeData = new List<CardTypeData>(length);
                for (int i = 0; i < length; i++)
                    _cardTypeData.Add(craftingSlots[i].CardType);

                return _cardTypeData;
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;

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