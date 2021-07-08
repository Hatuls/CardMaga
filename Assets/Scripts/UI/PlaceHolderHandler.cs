using Battles.Deck;
using System;
using UnityEngine;
using System.Collections.Generic;
namespace Battles.UI
{
    public class PlaceHolderHandler: MonoBehaviour
    {
        //reset Slots UI
        //give refrence to crafring slot Data
        //get icon from slot data
        //set slots UI by Data and index
        //arr of crafting slot UI
        #region Fields
        [SerializeField] ArtSO _artSO;
        #endregion
        #region Events
        #endregion

        #region Properties 
        #endregion
        private void Start()
        {
            CraftingSlotsData.SetPlaceHolderHandler = this;
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

