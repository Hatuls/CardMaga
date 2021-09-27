
using UnityEngine;

using System.Collections;
using Battles.Deck;
using Cards;
using Unity.Events;
using System;
using Battles.UI.CardUIAttributes;

namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>
    {
        #region Field
        [Tooltip("List of cards that UI Know About")]
        [SerializeField] CardUI _cardsHandler;
        [SerializeField] CardUI[] _handCards;

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

  
        [SerializeField] RectTransform _craftingBtnPosition;



        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _draggableLocation;

        [Tooltip("Card UI Settings")]
        [SerializeField] CardUISO _cardUISettings;

        private CardUIHandler _cardUIHandler;
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


        public RectTransform GetDrawDeckPosition => _drawDeckPosition;
        public RectTransform GetDiscardDeckPosition => _discardDeckPosition;
        public RectTransform GetExhaustDeckPosition => _exhaustDeckPosition;
        public RectTransform GetHandMiddlePosition => _handMiddlePosition;
        public float GetInputHandLine => GetHandMiddlePosition.position.y + _cardUISettings.LineOfHandTeritory;


        #endregion

        #region MonoBehaiviour callbacks


        #endregion

        #region Private Methods

        internal void CraftCardUI(Card addedCard, DeckEnum toDeck)
        {
            if (toDeck == DeckEnum.Hand)
             AssignDataToCardUI(_handUI.CurrentlyHolding, addedCard);

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
            _cardUIHandler = new CardUIHandler(_handUI, _cardsHandler, _cardUISettings);
        }

        public void DrawCards(Card[] cardData, DeckEnum fromDeck)
        {
            for (int i = 0; i < cardData.Length; i++)
            {
                var card = _handUI.GetHandCardUIFromIndex(i);
                if (card != null && cardData[i] != null)
                {
                    var InHandInputState = card.Inputs.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand);
                    if (InHandInputState.HasValue ==  false)
                    {

                    card.gameObject.SetActive(true);
                    InHandInputState.HasValue = true;
                    AssignDataToCardUI(card, cardData[i]);

                    }
                }
            }

        }


        public void RemoveHands()
        {
            _handUI.DiscardHand();

          //  var removal = GetCardUIHandler<DiscardHandHandler>();
          //  StartCoroutine(removal.MoveCardsUI(array, GetDeckPosition(DeckEnum.Disposal), GetDeckPosition(DeckEnum.Hand)));

        }
        public override void Init()
        {
            _handUI = new HandUI(_handCards, GetHandMiddlePosition.transform.localPosition, _cardUISettings);

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
     //       card.Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Locked;
            var handler = GetCardUIHandler<RemoveCardAfterACtivated>();
            StartCoroutine(handler.MoveCardsUI(new CardUI[1] { card }, GetDeckPosition(DeckEnum.Disposal), card.transform.localPosition));

        }



        public void LockHandCards(bool value)
      => _handUI.LockCardsInput(value);
        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (_cardUISettings.ToDrawGizmos)
            {
                Gizmos.color = Color.green;
                float screenWidth = (float)Screen.width;
                float fromX = _handMiddlePosition.position.x - screenWidth / 2;
                float fromY = GetInputHandLine;

                Vector3 from = new Vector3(fromX, fromY);
                Vector3 rightLine = new Vector3(from.x + (screenWidth), from.y);
                Gizmos.DrawLine(from, rightLine);
            }
        }


        #endregion


      
    }










    public class CardUIHandler
    {
        public static Action<Vector2, CardUI> OnExecuteCardUI;
        public static CardUIHandler Instance;
        CardUI _selectedCardUI;
        CardUISO _cardUISettings;
        CardUI _OriginalCard;
        HandUI _handUI;


        public CardUIHandler(HandUI hand, CardUI firstCardUI, CardUISO cardUISettings)
        {
            Instance = this;
            _handUI = hand;
            _selectedCardUI = firstCardUI;
            this._cardUISettings = cardUISettings;
            _selectedCardUI.gameObject.SetActive(false);
        }

      
        internal void CardUITouched(CardUI cardReference)
        {
     
            _handUI.ReplaceCard(_selectedCardUI, cardReference);
            _handUI.LockCardsInput(true);
            _selectedCardUI.Inputs.CardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hand);
            _selectedCardUI.gameObject.SetActive(true);
            _selectedCardUI = cardReference;
        }
        internal void CardUITouchedReleased(CardUI cardReference)
        {

            var card = _selectedCardUI;
            if (card == null)
                return;
          //    card.startState =CardInputs.CardUIInput.Zoomed;
            InputManager.Instance.RemoveObjectFromTouch();
            card.CardAnimator.ResetAllAnimations();    
            _handUI.LockCardsInput(false);
            card.gameObject.SetActive(false);
           // card.CardTranslations.CancelAllTweens();

        }

        internal void ToZoomCardUI()
        {
            var card = _selectedCardUI;
            if (card == null)
                return;
            //     card.Inputs.GetCanvasGroup.blocksRaycasts = false;
            // card.CardTranslations?.SetPosition( Vector2.zero);
            card.CardTranslations?.MoveCard(true, Vector2.zero, _cardUISettings.GetCardScaleDelay);
            card.CardAnimator.ScaleAnimation(true);
            card.CardTranslations?.SetRotation(0, _cardUISettings.RotationTimer);
            card.GFX.GlowCard(true);
        }

        internal void ToUnZoomCardUI(in Vector2 location)
        {
            var card = _selectedCardUI;
            if (card == null)
                return;

            card.GFX.GlowCard(false);

            //card.CardTranslations?.MoveCard(
            //    true,
            //  location,
            //    _cardUISettings.GetCardMoveToDeckDelay,
            //    null);

            card.CardAnimator.ScaleAnimation(false);
      //      card.Inputs.GetCanvasGroup.blocksRaycasts = true;
        }

        internal void DragCard(CardUI thisCardUI, in Vector2 touchPos)
        {
            thisCardUI.CardAnimator.ScaleAnimation(false);
            thisCardUI.CardTranslations.MoveCard(false, touchPos, _cardUISettings.GetCardFollowDelay);
        }

        internal void TryExecuteCardUI(CardUI thisCardUI)
        {
            if (CardExecutionManager.Instance.TryExecuteCard(_selectedCardUI))
            {
  //              _OriginalCard.Inputs.InHandInputState.HasValue = false;
                OnExecuteCardUI?.Invoke(_selectedCardUI.transform.localPosition,_OriginalCard);
                int index = _handUI.GetCardIndex(_selectedCardUI);
                DeckManager.Instance.TransferCard(true, DeckEnum.Hand, _selectedCardUI.GFX.GetCardReference.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Disposal, _selectedCardUI.GFX.GetCardReference);

         
            }
     
        }

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

            //for (int i = 0; i < cards.Length; i++)
            //{ 
            //    if (cards[i] == null)
            //        continue;
            //    var translation = cards[i].CardTranslations;

            //    cards[i].GFX.SetAlpha(_cardSettings.GetAlphaRemovalAmount, _cardSettings.GetAlphaRemovalTime,_cardSettings.GetAlphaLeanTween);
            //    translation.SetScale(Vector3.one * _cardSettings.GetScaleSizeForRemoval, _cardSettings.GetRemovalTimeForRemoval, _cardSettings.GetScaleRemovalLeanTweenType);
            //     yield return new WaitForSeconds(_cardSettings.GetDelayTillStartMovement);
            //    translation.MoveCardX(destination.x, _cardSettings.GetRemovalTransitionXTime, null, _cardSettings.GetMoveOnXLeanTween);
            //    translation.MoveCardY(destination.y, _cardSettings.GetRemovalTransitionYTime, false, _cardSettings.GetMoveOnYLeanTween);
            //}
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
            LeanTween.moveX(cardRect, destination.x, _cardSettings.CraftingToHandTransitionXTime).setEase(_cardSettings.CraftingToHandMoveOnXLeanTweenType);

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
            LeanTween.moveX(cardUIRect, destination.x, _cardSettings.DrawTransitionXTime).setEase(_cardSettings.DrawMoveOnXLeanTweenType);
        }
        
    }
}