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
        int EnterToLeftHash = Animator.StringToHash("EnterToLeft");
        Animator _animator;
         CraftingSlotUI[] _CraftingSlotsUIArr;
         RectTransform _firstSlotTransform;
         float _leanTweenTime;

         float _offsetPos =1 ;

        bool isPlayersCrafting;
        #region Properties
        public CraftingSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;
        #endregion



        public CraftingUIHandler(CraftingSlotUI[] _CraftingSlotsUIArr,Animator _anim ,RectTransform _firstSlotTransform, float _leanTweenTime,bool isPlayersCrafting)
        {
            this._animator = _anim;
            this._CraftingSlotsUIArr = _CraftingSlotsUIArr;
            this._firstSlotTransform = _firstSlotTransform;
            this._leanTweenTime = _leanTweenTime;
            this.isPlayersCrafting = isPlayersCrafting;
            ResetAllSlots();
        }




        internal void ResetSlotsDetection()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null)
                    _CraftingSlotsUIArr[i].ActivateGlow(false);
            }
            LeanTween.alpha(_CraftingSlotsUIArr[_CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);

            
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
            if(_CraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
               _CraftingSlotsUIArr[i].SlotID = i + 1;
                ResetPlaceHolderUI(i);
            }

            LeanTween.alpha(_CraftingSlotsUIArr[_CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);
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
        public void ChangeSlotsPos(Cards.Card[] cards , bool isFull)
        {
         

            var type = cards[0].CardSO.CardType.CardType;

            _CraftingSlotsUIArr[0].Appear(_leanTweenTime, type);
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                _CraftingSlotsUIArr[i].MovePlaceHolderSlot(ref isPlayersCrafting, GetRectTransform(i), _CraftingSlotsUIArr.Length - 1 == i ? 0 : _offsetPos);
                _CraftingSlotsUIArr[i].MoveDown(_leanTweenTime);
            }



            if (cards[_CraftingSlotsUIArr.Length - 1] != null)
            {

                type = cards[_CraftingSlotsUIArr.Length - 1].CardSO.CardType.CardType;

                _CraftingSlotsUIArr[_CraftingSlotsUIArr.Length - 1].Disapear(_leanTweenTime, type);
            }
            else
            {
                _CraftingSlotsUIArr[_CraftingSlotsUIArr.Length - 1].Disapear(_leanTweenTime);

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
        }

    }
}