﻿
using UnityEngine;

using System.Collections;
using Battles.Deck;
namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>, ITouchable
    {
        #region Field
        [Tooltip("List of cards that UI Know About")]
        [SerializeField] CardUI[] _cardUIArr;

        [Tooltip("Current Clicked Card UI")]
        [SerializeField] CardUI _clickedCardUI;

        [Tooltip("Hand Middle Position")]
        [SerializeField] RectTransform _handMiddlePosition;

        [Tooltip("Draw Card position")]
        [SerializeField] RectTransform _drawDeckPosition;

        [Tooltip("Discard Card position")]
        [SerializeField] RectTransform _discardDeckPosition;

        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _exhaustDeckPosition;

        [Tooltip("Card UI Settings")]
        [SerializeField] CardUISO _cardUISettings;

        private HandUI _handUI;
        private CardUI _zoomedCard;
        bool _isTryingToPlace;

        [SerializeField]
        Art.ArtSO _artSO;
        #endregion

        #region Events
        [SerializeField]
        Unity.Events.SoundsEvent _soundEvent;
        #endregion

        #region Properties
        public CardUI[] CardUIArr => _cardUIArr;

        public RectTransform GetDrawDeckPosition => _drawDeckPosition;
        public RectTransform GetDiscardDeckPosition => _discardDeckPosition;
        public RectTransform GetExhaustDeckPosition => _exhaustDeckPosition;
        public RectTransform GetHandMiddlePosition => _handMiddlePosition;
        public ref CardUI GetClickedCardUI { get => ref _clickedCardUI; }
        public CardUI SetClickedCardUI {
            set 
            {

                (value == null ? _clickedCardUI : value)?.GFX.GlowCard(value!=null);

                  _clickedCardUI = value;

            } 
        }
        public bool IsTryingToPlace { get => _isTryingToPlace; set { _isTryingToPlace = value; } }

        #endregion

        #region MonoBehaiviour callbacks
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DrawCards(null, DeckEnum.PlayerDeck);
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                RemoveHands();
            }

        }

        #endregion

        #region Private Methods
        private IEnumerator RemoveCards()
        {
            if (_zoomedCard != null)
            {
                _zoomedCard.CardTranslations?.MoveCard(true, GetDeckPosition(DeckEnum.Disposal), _cardUISettings.GetTimerForCardGoingToDiscardPile, false);
                _soundEvent?.Raise(SoundsNameEnum.DisacrdCard);
            }
            if (GetClickedCardUI != null)
            {
                GetClickedCardUI.CardTranslations?.MoveCard(true, GetDeckPosition(DeckEnum.Disposal), _cardUISettings.GetTimerForCardGoingToDiscardPile, false);
                _soundEvent?.Raise(SoundsNameEnum.DisacrdCard);
            }
            if (_handUI.GetAmountOfCardsInHand == 0)
                yield break;

            CardUI[] cards = _handUI.GetHandCards;

            if (_handUI.GetAmountOfCardsInHand > 0)
            {
                for (int i = _handUI.GetAmountOfCardsInHand - 1; i >= 0; i--)
                {
                    MoveCardToDisposal(ref cards[i]);

                    yield return new WaitForSeconds(_cardUISettings.GetDelayBetweenRemovalOfEachCard);
                }
            }

            yield return null;

        }

        private void MoveCardToDisposal(ref CardUI cards)
        {
            if (cards != null)
                cards.CardTranslations?.MoveCard(true, GetDeckPosition(DeckEnum.Disposal), _cardUISettings.GetTimerForCardGoingToDiscardPile, false);

            _soundEvent?.Raise( SoundsNameEnum.DisacrdCard );
           
            TryRemoveFromHandUI(cards);
        }

        internal CardUI ActivateCard(Cards.Card cardData, Vector2 pos)
        {
            //get one cardUI
            for (int i = 0; i < CardUIArr.Length; i++)
            {
                if (CardUIArr[i] == null)
                    break;
                if (CardUIArr[i].gameObject.activeInHierarchy == false || CardUIArr[i].gameObject.activeSelf == false)
                {
                    
                    CardUIArr[i].gameObject.SetActive(true);
                    CardUIArr[i].CardTranslations?.SetPosition(pos);
                    CardUIArr[i].CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, Time.deltaTime);
                    AssignDataToCardUI(ref CardUIArr[i], ref cardData);

                    if (CardUIArr[i].Inputs != null)
                    CardUIArr[i].Inputs.GetCanvasGroup.blocksRaycasts = true;

                    return CardUIArr[i];

                }
            }

            Debug.LogError("CardUI is NUll");
            return null;

            //turn it on
            //position of turning on
            //add data
        }
        public void AssignDataToCardUI(ref CardUI card, ref Cards.Card cardData)
        {
            card.GFX.SetCardReference(ref cardData, _artSO) ;

        }
        private Vector3 GetDeckPosition(DeckEnum fromDeck)
        {
            switch (fromDeck)
            {
                case DeckEnum.PlayerDeck:
                    return GetDrawDeckPosition.anchoredPosition3D;
                case DeckEnum.Hand:
                    return GetHandMiddlePosition.anchoredPosition3D;
                case DeckEnum.Disposal:
                    return GetDiscardDeckPosition.anchoredPosition3D;
                case DeckEnum.Exhaust:
                    return GetExhaustDeckPosition.anchoredPosition3D;
                default:
                    return Vector3.zero;
            }
        }
        #endregion


        #region Public Methods
        public void InitCardUI()
        {

            if (CardUIArr == null && CardUIArr.Length == 0)
            {
                Debug.LogError("Error in CardUIArr");
                return;
            }
            else
            {
                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                    {
                        Debug.LogError("CardUI is NUll");
                        return;
                    }
                    CardUIArr[i].gameObject.SetActive(false);
                }
            }
        }
        public void DrawCards(Cards.Card cardData, Battles.Deck.DeckEnum fromDeck, int amount = 1)
        {
            for (int j = 0; j < amount; j++)
            {
                if (_handUI.GetAmountOfCardsInHand >= _cardUISettings.GetAmountOfCardsUIInHand)
                {
                    Debug.Log("Tried to draw card but couldnt because hand is full");
                    break;
                }

                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                    {
                        Debug.Log("CardUI is NULL");
                        return;
                    }
                    if (CardUIArr[i].gameObject.activeInHierarchy == false || !CardUIArr[i].gameObject.activeSelf)
                    {
                        ActivateCard(cardData, GetDeckPosition(fromDeck));
                        _soundEvent?.Raise(SoundsNameEnum.DrawCard);
                        AddToHandUI(CardUIArr[i]);
                        break;
                    }
                }
            }

        }
        public void RemoveHands()
        {
            StopCoroutine(RemoveCards());
            StartCoroutine(RemoveCards());
        }
        public override void Init()
        {
            InitCardUI();
            _handUI = new HandUI(ref _cardUISettings.GetAmountOfCardsUIInHand, GetHandMiddlePosition.anchoredPosition, ref _cardUISettings);
          
            //     DrawCards(null, Battles.Deck.DeckEnum.PlayerDeck);
        }
        public void SetCardUI(CardUI cardUI)
        {
            if (cardUI == null)
            {
                Debug.LogError("Error in Set Card UI");
                return;
            }
            //  Debug.Log("Card Was Set");
            InputManager.Instance.AssignObjectFromTouch(this);
            DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Selected, cardUI.GFX.GetCardReference);
            SetClickedCardUI = cardUI;
            _soundEvent?.Raise(SoundsNameEnum.SelectCard);
        }
        public void AddToHandUI(CardUI cache)
        {
            cache?.GFX.GlowCard(false);
            _handUI.Add(ref cache);
        }
        public void TryRemoveFromHandUI(CardUI cache)
        {
          
            if (_zoomedCard != null)
            {
                AddToHandUI(_zoomedCard);
                _zoomedCard.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
                _zoomedCard = null;
            }
            _handUI.TryRemove(ref cache);
        }
        public void OnClickedCardUI(CardUI card)
        {

            if (card.GFX.GetRectTransform.localScale == _cardUISettings.GetCardUIZoomedScale)
            {
                _zoomedCard = null;
                AddToHandUI(card);
                card.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
            }
            else
            {
            

                TryRemoveFromHandUI(card);
                card.CardTranslations?.MoveCard(true, Vector2.zero, _cardUISettings.GetCardMoveToDeckDelay);
                card.CardTranslations?.SetScale(_cardUISettings.GetCardUIZoomedScale, _cardUISettings.GetCardScaleDelay);
                _zoomedCard = card;
            }

            _soundEvent?.Raise(SoundsNameEnum.TapCard);
        }

        public void StartRemoveProcess()
        {
            if (GetClickedCardUI == null)
                return;

            TryRemoveFromHandUI( GetClickedCardUI);
            StartCoroutine(RemoveTransition());


        }

       private void RemoveSelectedCardUI()
        {
            if (GetClickedCardUI == null)
                return;

                 GetClickedCardUI.GFX.SetActive(false);
                SetClickedCardUI=null;
                InputManager.Instance.RemoveObjectFromTouch();
        }
        IEnumerator RemoveTransition()
        {
            CardUI card = GetClickedCardUI;
            SetClickedCardUI = null;
            InputManager.Instance.RemoveObjectFromTouch();

            LeanTween.alpha(card.GFX.GetRectTransform, _cardUISettings.GetAlphaRemovalAmount, _cardUISettings.GetAlphaRemovalTime).setEase(_cardUISettings.GetAlphaLeanTween);
            LeanTween.scale(card.GFX.GetRectTransform, Vector3.one * _cardUISettings.GetScaleSizeForRemoval, _cardUISettings.GetRemovalTimeForRemoval).setEase(_cardUISettings.GetScaleRemovalLeanTweenType);
            yield return new WaitForSeconds(_cardUISettings.GetDelayTillStartMovement) ;
            LeanTween.moveX(card.GFX.GetRectTransform, GetDiscardDeckPosition.anchoredPosition3D.x, _cardUISettings.GetRemovalTransitionXTime).setEase(_cardUISettings.GetMoveOnXLeanTween);
            LeanTween.moveY(card.GFX.GetRectTransform, GetDiscardDeckPosition.anchoredPosition3D.y, _cardUISettings.GetRemovalTransitionYTime).setEase(_cardUISettings.GetMoveOnYLeanTween)
                .setOnComplete(()=> card.GFX.SetActive(false));
     
        }
        #endregion

        #region Touching Cards
        public void OnFirstTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI == null)
                return;
            Debug.Log("Card UI: OnFirstTouch, scaling card");

            TryRemoveFromHandUI(GetClickedCardUI);
        }

        public void OnReleaseTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI == null)
                return;

            if (_isTryingToPlace) // placed on placement Destination
            {
                Debug.Log("Placed On");
                /*
                _handUI.Remove(ref GetClickedCardUI);
                _placementUI.GetCurrentSlot.SetImage(
                    Battles.Deck.DeckManager.Instance.GetCardFromDeck(Battles.Deck.DeckEnum.Placement)[_placementUI.GetCurrentSlot.GetSlotID].GetSetCard.GetCardImage);
            */
                _isTryingToPlace = false;
            }
            else
            {
                AddToHandUI(GetClickedCardUI);

                if (GetClickedCardUI.Inputs != null)
                GetClickedCardUI.Inputs.GetCanvasGroup.blocksRaycasts = true;
                
                GetClickedCardUI.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, Time.deltaTime); 
                SetClickedCardUI = null;
            }


           
        }

        public void OnHoldTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI != null)
            {
                GetClickedCardUI.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
                GetClickedCardUI.CardTranslations?.MoveCard(false, touchPos, _cardUISettings.GetCardFollowDelay);
            }
        }

        #endregion
    }

}
