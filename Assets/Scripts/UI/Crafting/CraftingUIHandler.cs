using Battle.Deck;
using CardMaga.Card;
using UnityEngine;

namespace Battle.UI
{
    public class CraftingUIHandler
    {
        #region Fields

        private static int _moveLeftAnimHash = Animator.StringToHash("MoveLeft");
        private static readonly int _AssignAnimHash = Animator.StringToHash("AssignedAnimation");
        private static readonly int _fadingInAnim = Animator.StringToHash("AlphaFade");

        private readonly CraftingSlotUI _fadingOut;
        private readonly RectTransform _firstSlotTransform;
        private readonly DeckHandler _deckHandler;
        private static float leanTweenTime;

        #region Properties

        public CraftingSlotUI[] GetCraftingSlotsUIArr { get; }

        #endregion

        public CraftingUIHandler(CraftingSlotUI[] _CraftingSlotsUIArr, CraftingSlotUI _fadingOut,
            RectTransform _firstSlotTransform, float _leanTweenTime,DeckHandler deckHandler)
        {
            this._fadingOut = _fadingOut;
            GetCraftingSlotsUIArr = _CraftingSlotsUIArr;
            this._firstSlotTransform = _firstSlotTransform;
            leanTweenTime = _leanTweenTime;
            _deckHandler = deckHandler;
            ResetAllSlots();
        }


        internal void ResetSlotsDetection()
        {
            for (var i = 0; i < GetCraftingSlotsUIArr.Length; i++)
                if (GetCraftingSlotsUIArr[i] != null)
                    GetCraftingSlotsUIArr[i].ActivateGlow(false);
        }

        internal void MarkSlotsDetected()
        {
            for (var i = 0; i < GetCraftingSlotsUIArr.Length; i++)
                if (GetCraftingSlotsUIArr[i] != null && (_deckHandler[DeckEnum.CraftingSlots]as PlayerCraftingSlots).GetDeck[i] != null)
                    GetCraftingSlotsUIArr[i].ActivateGlow(true);
        }

        #endregion
        
        public void ResetAllSlots()
        {
            if (GetCraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }

            for (var i = 0; i < GetCraftingSlotsUIArr.Length; i++)
            {
                GetCraftingSlotsUIArr[i].SlotID = i + 1;
                ResetPlaceHolderUI(i);
            }
        }

        public void ResetPlaceHolderUI(int index)
        {
            if (index < 0 || index >= GetCraftingSlotsUIArr.Length)
            {
                Debug.LogError("Error in ResetCraftingSlot");
                return;
            }

            GetCraftingSlotsUIArr[index].ResetSlotUI();
        }

        public void ChangeSlotsPos(CardData[] cards, CardData removedCard)
        {
            _fadingOut.InitPlaceHolder(removedCard?.CardSO?.CardType);
            _fadingOut.PlayAnimation(_fadingInAnim);
            _fadingOut.MoveLocation(GetCraftingSlotsUIArr[0].RectTransform.localPosition, leanTweenTime);

            for (var i = 0; i < GetCraftingSlotsUIArr.Length; i++)
            {
                GetCraftingSlotsUIArr[i].InitPlaceHolder(cards[i]?.CardSO?.CardType);
                Vector2 startPos = i == GetCraftingSlotsUIArr.Length - 1
                    ? _firstSlotTransform.localPosition
                    : GetCraftingSlotsUIArr[i + 1].RectTransform.localPosition;
                GetCraftingSlotsUIArr[i].MoveLocation(startPos, leanTweenTime);
            }
        }

        public RectTransform GetRectTransform(int index)
        {
            return index == 0 ? _firstSlotTransform : GetCraftingSlotsUIArr[index - 1].RectTransform;
        }

        public void PlaceOnPlaceHolder(int index, CardData cardCache)
        {
            if (cardCache == null)
            {
                ResetPlaceHolderUI(index);
                return;
            }

            GetCraftingSlotsUIArr[index].InitPlaceHolder(cardCache.CardSO.CardType);
            GetCraftingSlotsUIArr[index].PlayAnimation(_AssignAnimHash);
        }

        //reset Slots UI
        //give refrence to crafring slot Data
        //get icon from slot data
        //set slots UI by Data and index
        //arr of crafting slot UI
    }
}