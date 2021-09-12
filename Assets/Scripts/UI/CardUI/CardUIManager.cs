
using UnityEngine;

using System.Collections;
using Battles.Deck;
using Cards;
using Unity.Events;
using System;

namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>
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

        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _craftingBtnPosition;



        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _draggableLocation;

        [Tooltip("Card UI Settings")]
        [SerializeField] CardUISO _cardUISettings;

        private HandUI _handUI;
        private CardUI _zoomedCard;
        private CardUI _holdingCardUI;
        private CardUITransition[] _transitions;


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
        public float GetInputHandLine => GetHandMiddlePosition.position.y + _cardUISettings.LineOfHandTeritory;


        #endregion

        #region MonoBehaiviour callbacks


        #endregion

        #region Private Methods
        public CardUI ActivateCard(Card card)
            => ActivateCard(card, _craftingBtnPosition.localPosition);


        internal void CraftCardUI(Card addedCard, DeckEnum toDeck)
        {
            var cardui = ActivateCard(addedCard, _craftingBtnPosition.localPosition);

            var handler = GetCardUIHandler<CraftCardUIHandler>();

            StartCoroutine(
                handler.MoveCardsUI(
                new CardUI[1] { cardui },
                GetDeckPosition(toDeck),
                _craftingBtnPosition.localPosition)
                );

        }

        public CardUI ActivateCard(Card cardData, Vector2 pos)
        {
            if (cardData != null)
            {
                //get one cardUI
                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                        break;
                    if (CardUIArr[i].gameObject.activeInHierarchy == false || CardUIArr[i].gameObject.activeSelf == false)
                    {

                        CardUIArr[i].CardTranslations?.SetScale(_cardUISettings.StartScaleSize * Vector3.one, Time.deltaTime);
                        CardUIArr[i].gameObject.SetActive(true);
                        CardUIArr[i].CardTranslations?.SetPosition(pos);
                        CardUIArr[i].CardTranslations.SetRotation(Vector3.zero);
                        AssignDataToCardUI(CardUIArr[i], cardData);

                        if (CardUIArr[i].Inputs != null)
                            CardUIArr[i].Inputs.GetCanvasGroup.blocksRaycasts = true;

                        return CardUIArr[i];
                    }
                }
            }
            Debug.LogError("CardUI is NUll");
            return null;

            //turn it on
            //position of turning on
            //add data
        }
        public void AssignDataToCardUI(CardUI card, Cards.Card cardData)
        {
            card.GFX.SetCardReference(cardData, _artSO);
        }
        private Vector3 GetDeckPosition(DeckEnum fromDeck)
        {
            switch (fromDeck)
            {
                case DeckEnum.PlayerDeck:
                    return GetDrawDeckPosition.localPosition;
                case DeckEnum.Hand:
                    return GetHandMiddlePosition.localPosition;
                case DeckEnum.Disposal:
                    return GetDiscardDeckPosition.localPosition;
                case DeckEnum.Exhaust:
                    return GetExhaustDeckPosition.localPosition;
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
                    CardUIArr[i].GFX.SetActive(false);
                }
            }
        }
        // remove
        public void DrawCards(Card[] cardData, DeckEnum fromDeck)
        {
            Vector2 deckPos = GetDeckPosition(fromDeck);
            CardUI[] cards = new CardUI[cardData.Length];
            for (int j = 0; j < cardData.Length; j++)
            {
                if (_handUI.GetAmountOfCardsInHand >= _cardUISettings.GetAmountOfCardsUIInHand)
                {
                    Debug.Log("Tried to draw card but couldnt because hand is full");
                    break;
                }
                if (cardData[j] != null)
                {
                    cards[j] = ActivateCard(cardData[j], deckPos);
                }
            }
            StartCoroutine(GetCardUIHandler<DrawCardUIHandler>().MoveCardsUI(cards, GetDeckPosition(DeckEnum.Hand), deckPos));
        }


        public void RemoveHands()
        {
            if (_zoomedCard != null)
            {
                _zoomedCard?.GFX.GlowCard(false);
                _handUI.Add(_zoomedCard);
                _zoomedCard = null;
            }
            if (_holdingCardUI != null)
            {
                _holdingCardUI?.GFX.GlowCard(false);
                _handUI.Add(_holdingCardUI);
                _holdingCardUI = null;
            }
            CardUI[] array = _handUI.GetHandCards;
            var removal = GetCardUIHandler<DiscardHandHandler>();
            StartCoroutine(removal.MoveCardsUI(array, GetDeckPosition(DeckEnum.Disposal), GetDeckPosition(DeckEnum.Hand)));

        }
        public override void Init()
        {
            _handUI = new HandUI(ref _cardUISettings.GetAmountOfCardsUIInHand, GetHandMiddlePosition.anchoredPosition, _cardUISettings);

            _transitions = new CardUITransition[4]
            {
                new DrawCardUIHandler(this,_handUI,_soundEvent,_cardUISettings),
                new RemoveCardAfterACtivated(this, _cardUISettings, _soundEvent),
                new DiscardHandHandler(this,_cardUISettings,_handUI,_soundEvent),
                new CraftCardUIHandler(_handUI ,this, _cardUISettings,_soundEvent)
            };
            
            
            InitCardUI();
        }


        /// <summary>
        /// Return a Transition Class For The Card UI
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns>DrawCardUIHandler, RemoveCardAfterActivated, DiscardHandHandler, CraftCardUIHandler</returns>
        private T GetCardUIHandler<T>() where T : CardUITransition
        {
            for (int i = 0; i < _transitions.Length; i++)
            {
                if (_transitions[i].GetType() == typeof(T))
                {
                    return _transitions[i] as T;
                }
            }
            return null;
        }

        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;

            _holdingCardUI = null;
            _zoomedCard = null;
            _handUI.AlignCards();

            card.Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Locked;
            var handler = GetCardUIHandler<RemoveCardAfterACtivated>();
            StartCoroutine(handler.MoveCardsUI(new CardUI[1] { card }, GetDeckPosition(DeckEnum.Disposal), card.transform.localPosition)) ;
            Debug.Log($"<a>Hand Amount {_handUI.GetAmountOfCardsInHand}</a>");
        }

  

          public void LockHandCards(bool value)
        => _handUI.LockCardsInput(value);
        #endregion

        #region Touching Cards

        #region Holding Card
        public void SelectCardUI(CardUI card)
        {
            if (card == _holdingCardUI)
                return;

            if (_holdingCardUI != null)
            {
                RemoveCardUI();
            }
    
            _holdingCardUI = card;

            if (_holdingCardUI != null)
            {
                _zoomedCard = null;
                AssignCardUI();
            }
        }
        private void AssignCardUI()
        {
            _handUI.TryRemove(_holdingCardUI);

            DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Selected, _holdingCardUI.GFX.GetCardReference);
            _holdingCardUI.CardTranslations.CancelAllTweens();
            _holdingCardUI.CardTranslations?.SetRotation(0, _cardUISettings.RotationTimer);
            _holdingCardUI.CardTranslations.MoveCard(true, _draggableLocation.localPosition, 0.3f);
            _holdingCardUI.GFX.GlowCard(false);
            _holdingCardUI.CardAnimator.ScaleAnimation(false);
            _soundEvent?.Raise(SoundsNameEnum.TapCard);
        }
      
        private void RemoveCardUI()
        {
            _holdingCardUI.GFX.GlowCard(false);
            DeckManager.Instance.TransferCard(DeckEnum.Selected, DeckEnum.Hand, _holdingCardUI.GFX.GetCardReference);
            LockHandCards(false);
            _handUI.Add(_holdingCardUI);
        }

        #endregion

        #region Zoom Behaviour
        public void ZoomCard(CardUI card)
        {
            if (card == _zoomedCard)
                return;

            if (_zoomedCard != null)  
                ResetZoom();
            
            _zoomedCard = card;

            if (_zoomedCard != null)
            {
                _holdingCardUI = null;
                ZoomInCard();
            }
        }
        private void ZoomInCard()
        {
            _zoomedCard._currentState = CardUIAttributes.CardInputs.CardUIInput.Zoomed;
            LockHandCards(true);
            _zoomedCard.transform.SetSiblingIndex(_handUI.GetAmountOfCardsInHand);
            _zoomedCard.Inputs.GetCanvasGroup.blocksRaycasts = false;
            _zoomedCard.CardTranslations?.MoveCard(true, Vector2.zero, _cardUISettings.GetCardMoveToDeckDelay);
            _zoomedCard.CardAnimator.ScaleAnimation(true);
            _zoomedCard.CardTranslations?.SetRotation(0, _cardUISettings.RotationTimer);
            _zoomedCard.GFX.GlowCard(true);
            _soundEvent?.Raise(SoundsNameEnum.TapCard);
        }
        private void ResetZoom()
        {
            //return to start position
            _zoomedCard.Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Hand;
            LockHandCards(false);
            _zoomedCard.GFX.GlowCard(false);
            _zoomedCard.CardAnimator.ScaleAnimation(false);

            _handUI.AlignCards();
        }

        #endregion
        #endregion

        #region Gizmos
        
        private void OnDrawGizmos()
        {
            if (_cardUISettings.ToDrawGizmos)
            {
                float screenWidth = (float)Screen.width;
                float fromX = _handMiddlePosition.position.x - screenWidth / 2;
                float fromY = _handMiddlePosition.position.y + _cardUISettings.LineOfHandTeritory;
                Vector3 from = new Vector3(fromX, fromY);
                Vector3 rightLine = new Vector3(from.x+(screenWidth) , from.y);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(from, rightLine);
            }
        }


        #endregion
    }


    public abstract class CardUITransition
    {
        protected CardUIManager _cardUIManager;
        protected SoundsEvent _soundEvent;
        protected CardUISO _cardSettings;
      
        public CardUITransition(CardUIManager cuim,CardUISO so, SoundsEvent soundEvent)
        {
            _cardUIManager = cuim;
            _soundEvent = soundEvent;
            _cardSettings = so;
        }
        public abstract IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos);

    }

    public class DiscardHandHandler : CardUITransition
    {
        HandUI _handUI;
        public DiscardHandHandler(CardUIManager cuim, CardUISO so, HandUI hands, SoundsEvent soundEvent) : base(cuim, so, soundEvent)
        {
            _handUI = hands;
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            if (cards == null || cards.Length == 0)
                yield break;


            for (int i = cards.Length - 1; i >= 0; i--)
            {
                if (cards[i] == null)
                    continue;
                _cardUIManager.StartCoroutine(DiscardCard(cards[i], destination));

                yield return new WaitForSeconds(_cardSettings.DelayBetweenCardsIsDiscarded);
                _handUI.TryRemove(cards[i]);
            }

        }

        IEnumerator DiscardCard(CardUI card, Vector2 destination)
        {

            _soundEvent?.Raise(SoundsNameEnum.DisacrdCard);
            card.GFX?.SetAlpha(_cardSettings.EndDiscardAlphaAmount, _cardSettings.AlphaDiscardTime, _cardSettings.AlphaDiscardTweenType);
            var translation = card.CardTranslations;
            if (translation!= null)
            {
                translation.SetScale(Vector3.one * _cardSettings.DiscardEndScaleSize, _cardSettings.ScaleDiscardTime, _cardSettings.ScaleDiscardTweenType);

            yield return new WaitForSeconds(_cardSettings.DelayBetweenVisualAndMoving);
                translation.MoveCardX(destination.x, _cardSettings.DiscardTransitionXTime, null,_cardSettings.DiscardMoveOnYLeanTweenType);
                translation.MoveCardY(destination.y, _cardSettings.DiscardTransitionYTime,false,_cardSettings.DiscardMoveOnYLeanTweenType);

            }
       
        }
    }

    public class RemoveCardAfterACtivated : CardUITransition
    {
        public RemoveCardAfterACtivated(CardUIManager _manager, CardUISO so, SoundsEvent soundEvent) : base(_manager, so, soundEvent)
        {
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            if (cards == null || cards.Length == 0)
                yield break;

            InputManager.Instance.TouchableObject = null;
            for (int i = 0; i < cards.Length; i++)
            { 
                if (cards[i] == null)
                    continue;
                var translation = cards[i].CardTranslations;

                cards[i].GFX.SetAlpha(_cardSettings.GetAlphaRemovalAmount, _cardSettings.GetAlphaRemovalTime,_cardSettings.GetAlphaLeanTween);
                translation.SetScale(Vector3.one * _cardSettings.GetScaleSizeForRemoval, _cardSettings.GetRemovalTimeForRemoval, _cardSettings.GetScaleRemovalLeanTweenType);
                 yield return new WaitForSeconds(_cardSettings.GetDelayTillStartMovement);
                translation.MoveCardX(destination.x, _cardSettings.GetRemovalTransitionXTime, null, _cardSettings.GetMoveOnXLeanTween);
                translation.MoveCardY(destination.y, _cardSettings.GetRemovalTransitionYTime, false, _cardSettings.GetMoveOnYLeanTween);
            }
        }

    }


    public class CraftCardUIHandler : CardUITransition
    {
        HandUI _hand;
        public CraftCardUIHandler(HandUI hand, CardUIManager cuim, CardUISO so, SoundsEvent soundEvent) : base(cuim, so, soundEvent)
        {
            _hand = hand;
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            if (cards == null || cards.Length == 0)
                yield break;



            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null)
                    continue;

                RectTransform cardUIRect = cards[i].GFX.GetRectTransform;
                //DeckEnum goToDeck = cards[i].GFX.GetCardReference.CardSO.GoToDeckAfterCrafting;
                //switch (goToDeck)
                //{
                //    case DeckEnum.PlayerDeck:
                //        MoveCardUIToPlayerDeck(cards[i], cardUIRect, destination);
                //        break;
                //    case DeckEnum.Hand:
                        MoveCardUIToHandDeck(cards[i], cardUIRect, destination, _hand);
                //        break;
                //    case DeckEnum.Disposal:
                //        MoveCardUIToDisposalDeck(cards[i], cardUIRect, destination);

                //        break;

                //    default:
                //        throw new Exception("Crafted Card Destination Was Not Valid Check Destination Deck!");
                    
                //}

                yield return null;
            }
        }

        //private void MoveCardUIToPlayerDeck(CardUI cardUI, RectTransform cardRect, in Vector2 destination)
        //{
        //    cardUI.GFX.GlowCard(false);
        //    // set their starting scale
        //    //play sound
        //    _soundEvent?.Raise(SoundsNameEnum.DrawCard);


        //    cardUI.CardTranslations?.MoveCardX(destination.x, _cardSettings.CraftingToDrawPileTransitionXTime, null, _cardSettings.CraftingToDrawPileMoveOnXLeanTweenType);
        //    cardUI.CardTranslations?.MoveCardY(destination.y, _cardSettings.CraftingToDrawPileTransitionYTime, true, _cardSettings.CraftingToDrawPileMoveOnYLeanTweenType);

        //}

        private void MoveCardUIToHandDeck(CardUI cardUI, RectTransform cardRect, in Vector2 destination,HandUI _hand)
        {
            cardUI.GFX.GlowCard(true);
            // set their starting scale
            cardUI.GFX.SetAlpha(1, _cardSettings.AlphaDrawTime, _cardSettings.AlphaDrawTweenType);
            //play sound
            _soundEvent?.Raise(SoundsNameEnum.DrawCard);

            cardUI.CardTranslations?.MoveCardY(destination.y, _cardSettings.CraftingToHandTransitionYTime, null, _cardSettings.CraftingToHandMoveOnYLeanTweenType);
            LeanTween.moveX(cardRect, destination.x, _cardSettings.CraftingToHandTransitionXTime).setEase(_cardSettings.CraftingToHandMoveOnXLeanTweenType)
                .setOnComplete(() => _hand.Add(cardUI));

        }

        //private void MoveCardUIToDisposalDeck(CardUI cardUI, RectTransform cardRect, in Vector2 destination)
        //{

        //    // disable glow
        //    cardUI.GFX.GlowCard(false);
        //    // set their starting scale
        //    //play sound
        //    _soundEvent?.Raise(SoundsNameEnum.DisacrdCard);

        //    // Move cards and then add it to hand
        //    cardUI.CardTranslations?.MoveCardY(destination.y, _cardSettings.CraftingToDiscardTransitionYTime, null, _cardSettings.CraftingToDiscardMoveOnYLeanTweenType);
        //    cardUI.CardTranslations?.MoveCardX(destination.x, _cardSettings.CraftingToDiscardTransitionXTime, true, _cardSettings.CraftingToDiscardMoveOnXLeanTweenType);
        //}
    }



    public class DrawCardUIHandler : CardUITransition
    {
        HandUI _hand;
        public DrawCardUIHandler(CardUIManager _manager,HandUI hand,SoundsEvent _soundEvent, CardUISO so) : base(_manager,so, _soundEvent)
        {
            _hand = hand;
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos)
        {
            // check if its valid array
            if (cards == null || cards.Length == 0)
                yield break;

            
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null)
                    continue;
                MoveCardUI(cards[i], destination);

                yield return new WaitForSeconds(_cardSettings.DelayTillDrawNextCard);

            }
        }

        private void MoveCardUI(CardUI card, Vector2 destination)
        {
            // cache rectTransform
            RectTransform cardUIRect = card.GFX.GetRectTransform;
            // disable glow
            card.GFX.GlowCard(false);
            // set their starting scale
            cardUIRect.localScale = Vector3.one * _cardSettings.StartScaleSize;

            //play sound
            _soundEvent?.Raise(SoundsNameEnum.DrawCard);

            //set their alpha
            card.GFX.SetAlpha(
                _cardSettings.StartAlphaAmount,
                0.001f, LeanTweenType.notUsed,
                () => 
                { card.GFX.SetAlpha(1, _cardSettings.AlphaDrawTime, _cardSettings.AlphaDrawTweenType); }
                ); 
            //LeanTween.alpha(cardUIRect, _cardSettings.StartAlphaAmount, 0.001f).setOnComplete(() =>
            //LeanTween.alpha(cardUIRect, 1, _cardSettings.AlphaDrawTime).setEase(_cardSettings.AlphaDrawTweenType));

            // set 
            card.CardTranslations?.SetScale(Vector3.one, _cardSettings.ScaleDrawTime);
            // Move cards and then add it to hand
            card.CardTranslations?.MoveCardY(destination.y, _cardSettings.DrawTransitionYTime, null, _cardSettings.DrawMoveOnYLeanTweenType);
          //  card.CardTranslations?.MoveCardX(destination.x, _cardSettings.DrawTransitionXTime, null, _cardSettings.DrawMoveOnXLeanTweenType);
            LeanTween.moveX(cardUIRect, destination.x, _cardSettings.DrawTransitionXTime).setEase(_cardSettings.DrawMoveOnXLeanTweenType)
                .setOnComplete(() => _hand.Add(card));
        }
        
    }
}