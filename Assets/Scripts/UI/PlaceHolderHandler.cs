using Battles.Deck;
using System;
using UnityEngine;
namespace Battles.UI
{
    public class PlaceHolderHandler : MonoSingleton<PlaceHolderHandler>
    {
        #region Fields
        [SerializeField] PlaceholderUI _playerPlaceHolder;
        [SerializeField] PlaceholderUI _opponentPlaceHolder;
        [SerializeField] ArtSO _artSO;
        #endregion
        #region Events

        #endregion

        #region Properties 
        public ref PlaceholderUI PlayerPlaceHolder => ref _playerPlaceHolder;
        public ref PlaceholderUI OpponentPlaceHolder => ref _opponentPlaceHolder;
        #endregion

        #region Opponent PlaceHolder
        public void AssignEnemyActionOnSlot(Cards.Card Action)
        {
         //   _enemyAction.InitCard(ref ArtSO.UIColorPalette, Action.GetSetCard.GetCardTypeEnum, ArtSO.DefaultSlotSO.GetBackground, ArtSO.DefaultSlotSO.GetDecor, ArtSO.IconCollection.GetSprite(Action.GetSetCard.GetBodyPartEnum));
        }
#endregion
        #region Player PlaceHolder
        public void SetCardUI(PlaceHolderSlotUI placeHolderSlotUI)
        {
            if (placeHolderSlotUI == null)
            {
                Debug.LogError("CardUIManager : Placeholder is null");
                return;
            }

            var cache = DeckManager.Instance.GetCardFromDeck(placeHolderSlotUI.GetSlotID, DeckEnum.Placement);
            if (cache == null)
                return;

            DeckManager.Instance.TransferCard(DeckEnum.Placement, DeckEnum.Hand, cache, placeHolderSlotUI.GetSlotID);
            placeHolderSlotUI.ResetSlot(_artSO.UIColorPalette);
            CardUIManager.Instance.SetCardUI(CardUIManager.Instance.ActivateCard(cache, placeHolderSlotUI.RectTransform.anchoredPosition));
            CardUIManager.Instance.GetClickedCardUI.GetCanvasGroup.blocksRaycasts = false;
        }

        public void OnSlotInteract(PlaceHolderSlotUI interactedSlot)
        {
            if (interactedSlot == null ||
                (interactedSlot.IsHoldingCard == false && CardUIManager.Instance.GetClickedCardUI == null))
                return;



            Cards.Card cardCache = DeckManager.Instance.GetCardFromDeck(interactedSlot.GetSlotID, DeckEnum.Placement);

            if (cardCache == null&& !interactedSlot.IsHoldingCard && CardUIManager.Instance.GetClickedCardUI == null) 
                return;

            if (interactedSlot.IsHoldingCard == false && CardUIManager.Instance.GetClickedCardUI != null) // place a card on top of placement
            {
                DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Placement, CardUIManager.Instance.GetClickedCardUI.GetCardReference, interactedSlot.GetSlotID);

                if (cardCache == null)
                    cardCache = DeckManager.Instance.GetCardFromDeck(interactedSlot.GetSlotID, DeckEnum.Placement);

                interactedSlot.InitCard(_artSO.UIColorPalette, cardCache.GetSetCard.GetCardTypeEnum,
                    _artSO.DefaultSlotSO.GetBackground, _artSO.DefaultSlotSO.GetDecor, _artSO.IconCollection.GetSprite(cardCache.GetSetCard.GetBodyPartEnum));

                CardUIManager.Instance.GetClickedCardUI.SetActive(false);

                CardUIManager.Instance.GetClickedCardUI = null;

                InputManager.Instance.RemoveObjectFromTouch();

            }
            else if (interactedSlot.IsHoldingCard && CardUIManager.Instance.GetClickedCardUI != null)
            {
                interactedSlot.InitCard(_artSO.UIColorPalette, CardUIManager.Instance.GetClickedCardUI.GetCardReference.GetSetCard.GetCardTypeEnum,
                    _artSO.DefaultSlotSO.GetBackground, _artSO.DefaultSlotSO.GetDecor, _artSO.IconCollection.GetSprite(CardUIManager.Instance.GetClickedCardUI.GetCardReference.GetSetCard.GetBodyPartEnum));

                DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Placement, CardUIManager.Instance.GetClickedCardUI.GetCardReference, interactedSlot.GetSlotID);
                CardUIManager.Instance.AssignDataToCardUI(ref CardUIManager.Instance.GetClickedCardUI, ref cardCache);

            }

        }
        public override void Init()
        {
            _playerPlaceHolder.Init();
            _opponentPlaceHolder.Init();
        }
    }
    #endregion 
}

