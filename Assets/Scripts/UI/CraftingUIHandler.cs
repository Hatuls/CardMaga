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


         CraftingSlotUI[] _CraftingSlotsUIArr;
         RectTransform _firstSlotTransform;
         float _leanTweenTime;

         float _offsetPos =1 ;


         GameObject _buttonGlow;

         TextMeshProUGUI _buttonText;


        bool isPlayersCrafting;
        #region Properties
        public CraftingSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;
        #endregion



        public CraftingUIHandler(CraftingSlotUI[] _CraftingSlotsUIArr, RectTransform _firstSlotTransform, float _leanTweenTime,bool isPlayersCrafting, GameObject _buttonGlow = null, TextMeshProUGUI _buttonText = null)
        {
            this._CraftingSlotsUIArr = _CraftingSlotsUIArr;
            this._firstSlotTransform = _firstSlotTransform;
            this._leanTweenTime = _leanTweenTime;
            this._buttonGlow = _buttonGlow;
            this._buttonText = _buttonText;
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

            if (_buttonGlow!= null && _buttonGlow.activeSelf != false)
            {
                _buttonGlow?.SetActive(false);
                _buttonText.text = "Clear";
            }
            
            
        }

        internal void MarkSlotsDetected()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null && DeckManager.GetCraftingSlots.GetDeck[i]!= null)
                    _CraftingSlotsUIArr[i].ActivateGlow(true);
            }

            if (_buttonGlow != null && _buttonGlow.activeSelf != true)
            {
                _buttonGlow?.SetActive(true);
                _buttonText.text = "Craft";
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
        public void ChangeSlotsPos(Cards.Card[] cards )
        {
            // check thread possability for color check


            var type = cards[0].CardSO.CardType._cardType;

            _CraftingSlotsUIArr[0].Appear(_leanTweenTime, type);
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                _CraftingSlotsUIArr[i].MovePlaceHolderSlot(ref isPlayersCrafting, GetRectTransform(i), _CraftingSlotsUIArr.Length - 1 == i? 0: _offsetPos);
                _CraftingSlotsUIArr[i].MoveDown(_leanTweenTime);
            }
            if (cards[_CraftingSlotsUIArr.Length - 1] != null)
            {

                type = cards[_CraftingSlotsUIArr.Length - 1].CardSO.CardType._cardType;

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