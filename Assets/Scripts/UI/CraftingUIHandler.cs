using Battles.Deck;
using TMPro;
using UnityEngine;

namespace Battles.UI
{
    public class CraftingUIHandler
    {
        //reset Slots UI
        //give refrence to crafring slot Data
        //get icon from slot data
        //set slots UI by Data and index
        //arr of crafting slot UI
        #region Fields
        static int _moveLeftAnimHash = Animator.StringToHash("MoveLeft");
        static int _AssignAnimHash = Animator.StringToHash("AssignedAnimation");
        static int _fadingInAnim = Animator.StringToHash("AlphaFade");

     

         CraftingSlotUI[] _CraftingSlotsUIArr;
         CraftingSlotUI _fadingOut;
         RectTransform _firstSlotTransform;

        static float leanTweenTime;
        #region Properties
        public CraftingSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;
        #endregion



        public CraftingUIHandler(CraftingSlotUI[] _CraftingSlotsUIArr, CraftingSlotUI _fadingOut, RectTransform _firstSlotTransform, float _leanTweenTime,bool isPlayersCrafting)
        {

           this. _fadingOut = _fadingOut;
            this._CraftingSlotsUIArr = _CraftingSlotsUIArr;
            this._firstSlotTransform = _firstSlotTransform;
            leanTweenTime = _leanTweenTime;
            ResetAllSlots();
        }




        internal void ResetSlotsDetection()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null)
                    _CraftingSlotsUIArr[i].ActivateGlow(false);
            }
     
        }

        internal void MarkSlotsDetected()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null && DeckManager.GetCraftingSlots(true).GetDeck[i]!= null)
                    _CraftingSlotsUIArr[i].ActivateGlow(true);
            }
        }
        #endregion

        public void ResetAllSlots()
        {
            if (_CraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                _CraftingSlotsUIArr[i].SlotID = i + 1;
                ResetPlaceHolderUI(i);
            }
        }
        public void ResetPlaceHolderUI(int index)
        {
     

            if (index < 0  || index >= _CraftingSlotsUIArr.Length)
            {
                Debug.LogError("Error in ResetCraftingSlot");
                return;
            }
            _CraftingSlotsUIArr[index].ResetSlotUI();
        }
        public void ChangeSlotsPos(Cards.Card[] cards , Cards.Card removedCard)
        {
 
            _fadingOut.InitPlaceHolder(removedCard?.CardSO?.CardType);
            _fadingOut.PlayAnimation(_fadingInAnim);
            _fadingOut.MoveLocation(_CraftingSlotsUIArr[0].RectTransform.localPosition, leanTweenTime);

            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                _CraftingSlotsUIArr[i].InitPlaceHolder(cards[i]?.CardSO?.CardType);
                Vector2 startPos = i == _CraftingSlotsUIArr.Length - 1 ? _firstSlotTransform.localPosition: _CraftingSlotsUIArr[i + 1].RectTransform.localPosition;
                _CraftingSlotsUIArr[i].MoveLocation(startPos, leanTweenTime);
            }

        }
        public RectTransform GetRectTransform(int index)
        => index == 0 ? _firstSlotTransform : _CraftingSlotsUIArr[index -1].RectTransform;
  
        public void PlaceOnPlaceHolder(int index, Cards.Card cardCache)
        {
            if (cardCache == null )
            {
                ResetPlaceHolderUI(index);
                return;
            }
            _CraftingSlotsUIArr[index].InitPlaceHolder(cardCache.CardSO.CardType);
            _CraftingSlotsUIArr[index].PlayAnimation(_AssignAnimHash);
        }

    }
}