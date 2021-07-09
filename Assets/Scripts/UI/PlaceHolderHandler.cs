using Battles.Deck;
using System;
using UnityEngine;
using System.Collections.Generic;
namespace Battles.UI
{
    public class PlaceHolderHandler : MonoBehaviour
    {
        //reset Slots UI
        //give refrence to crafring slot Data
        //get icon from slot data
        //set slots UI by Data and index
        //arr of crafting slot UI
        #region Fields
        [SerializeField] ArtSO _artSO;

        [SerializeField] PlaceHolderSlotUI[] _CraftingSlotsUIArr;
        [SerializeField] RectTransform _firstSlotTransform;
        [SerializeField] float _leanTweenTime;
        #endregion
        #region Events
        #endregion

        #region Properties
        public PlaceHolderSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;
        #endregion
        private void Start()
        {
            CraftingSlotsData.SetPlaceHolderHandler = this;
            ResetAllSlots();
        }
        void ResetAllSlots()
        {
            if(_CraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                ResetPlaceHolderUI(i);
            }
        }
        public void ResetPlaceHolderUI(int index)
        {
            if (_artSO.UIColorPalette == null)
            {
                Debug.LogError("Error in ResetCraftingSlot");
                return;
            }
            _CraftingSlotsUIArr[index].ResetSlotUI(_artSO.UIColorPalette);
        }
        public void ChangeSlotsPos()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                _CraftingSlotsUIArr[i].MovePlaceHolderSlot(GetRectTransform(i),_leanTweenTime);
            }
        }
        public RectTransform GetRectTransform(int index)
        {
            if(index == 0)
            {
                return _firstSlotTransform;
            }
            else
            {
                return _CraftingSlotsUIArr[index -1].RectTransform;
            }
        }
        public void PlaceOnPlaceHolder(int index, Cards.Card cardCache)
        {
            _CraftingSlotsUIArr[index].InitPlaceHolder(_artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
                 _artSO.DefaultSlotSO.GetBackground, _artSO.DefaultSlotSO.GetDecor,_artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));
        }
        //public void PlaceOnPlaceHolder(PlaceHolderSlotUI interactedSlot, Cards.Card cardCache)
        //{
        //    interactedSlot.InitCraftSlot(_artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
        //        _artSO.DefaultSlotSO.GetBackground, _artSO.DefaultSlotSO.GetDecor, _artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));

        //    CardUIManager.Instance.GetClickedCardUI.SetActive(false);

        //    CardUIManager.Instance.GetClickedCardUI = null;

        //    InputManager.Instance.RemoveObjectFromTouch();
        //}
    }
}

