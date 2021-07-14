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

        [SerializeField] float _offsetPos =1 ;
      static  PlaceHolderHandler _instance;
        #endregion
        #region Events
        #endregion

        #region Properties
        public PlaceHolderSlotUI[] GetCraftingSlotsUIArr => _CraftingSlotsUIArr;

        internal void ResetSlotsDetection()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null)
            _CraftingSlotsUIArr[i].SetBackGroundColor(_artSO.UIColorPalette, _artSO.UIColorPalette.GetBackgroundColor);
            }
            LeanTween.alpha(_instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);

        }

        internal void MarkSlotsDetected()
        {
            for (int i = 0; i < _CraftingSlotsUIArr.Length; i++)
            {
                if (_CraftingSlotsUIArr[i] != null && DeckManager.GetCraftingSlots.GetDeck[i]!= null)
                {
                    _CraftingSlotsUIArr[i].SetBackGroundColor(_artSO.UIColorPalette,_artSO.DefaultSlotSO.GetDetectedBackgroundColor);
                }
            }

        }
        #endregion
        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {

            ResetAllSlots();
         
        }
        public static void ResetAllSlots()
        {
            if(_instance._CraftingSlotsUIArr == null)
            {
                Debug.LogError("Error in ResetAllSlots");
                return;
            }
            for (int i = 0; i < _instance._CraftingSlotsUIArr.Length; i++)
            {
                _instance._CraftingSlotsUIArr[i].SlotID = i + 1;
                ResetPlaceHolderUI(i);
            }

            LeanTween.alpha(_instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].RectTransform, 0, 0.001f);
        }
        public static void ResetPlaceHolderUI(int index)
        {
            if (_instance._artSO.UIColorPalette == null)
            {
                Debug.LogError("Error in ResetCraftingSlot");
                return;
            }
            _instance._CraftingSlotsUIArr[index].ResetSlotUI(_instance._artSO.UIColorPalette);
        }
        public static void ChangeSlotsPos()
        {
            _instance._CraftingSlotsUIArr[0].Appear(_instance._leanTweenTime , _instance._artSO.UIColorPalette);

            for (int i = 0; i < _instance._CraftingSlotsUIArr.Length; i++)
            {
                _instance._CraftingSlotsUIArr[i].MovePlaceHolderSlot(ref _instance.GetRectTransform(i), _instance._CraftingSlotsUIArr.Length - 1 == i? 0: _instance._offsetPos);
                _instance._CraftingSlotsUIArr[i].MoveDown(_instance._leanTweenTime);
            }
            _instance._CraftingSlotsUIArr[_instance._CraftingSlotsUIArr.Length - 1].Disapear(_instance._leanTweenTime, _instance._artSO.UIColorPalette);

        }
        public ref RectTransform GetRectTransform(int index)
        {
            if(index == 0)
            {
                return ref _firstSlotTransform;
            }
            else
            {

                return ref _CraftingSlotsUIArr[index -1].RectTransform;
            }
        }
        public  static void PlaceOnPlaceHolder(int index, Cards.Card cardCache)
        {
            if (cardCache == null )
            {
                ResetPlaceHolderUI(index);
                return;
            }
            _instance._CraftingSlotsUIArr[index].InitPlaceHolder(_instance._artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
               _instance._artSO.DefaultSlotSO.GetBackground, _instance._artSO.DefaultSlotSO.GetDecor, _instance._artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));
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

