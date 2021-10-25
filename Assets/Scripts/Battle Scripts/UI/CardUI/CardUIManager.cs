
using UnityEngine;
using Battles.Deck;
using Cards;
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
       // private CardUITransition[] _transitions;


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

        internal void UpdateHand()
        {
            DrawCards(DeckManager.Instance.GetCardsFromDeck(true, DeckEnum.Hand));
        }


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

        public void DrawCards(Card[] cardData)
        {
            for (int i = 0; i < cardData.Length; i++)
            {
                var card = _handUI.GetHandCardUIFromIndex(i);

                if (cardData[i] == null)
                {
                    string cardsDrawn = "";
                    for (int j = 0; j < cardData.Length; j++)
                    {
                        if (cardData[j] != null)
                            cardsDrawn += cardData[j].ToString() + " " + cardData[j].CardSO.CardName + "\n";
                        else
                            cardsDrawn += "Card at index " + i + " Is null!\n";
                    }
                    Debug.LogError($"Drawn Card is null!!\n {cardsDrawn} ,\n");
                  //  card.gameObject.SetActive(false);
                    continue;
                }

                if (card != null)
                {
                    var InHandInputState = card.Inputs.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand);
                    if (InHandInputState.HasValue ==  false)
                    {
                        if (card.gameObject.activeSelf == false)
                            card.gameObject.SetActive(true);

                        InHandInputState.HasValue = true;

                        card.CardAnimator.PlayNoticeAnimation();

                     //  var cardRefenrec = card.GFX.GetCardReference;
                     //  if (cardRefenrec ==null|| cardData[i].CardSO != cardRefenrec.CardSO && cardRefenrec.CardLevel != cardData[i].CardLevel)
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

            //_transitions = new CardUITransition[4]
            //{
            //    new DrawCardUIHandler(this,_handUI,_soundEvent,_cardUISettings),
            //    new RemoveCardAfterACtivated(this, _cardUISettings, _soundEvent),
            //    new DiscardHandHandler(this,_cardUISettings,_handUI,_soundEvent),
            //    new CraftCardUIHandler(_handUI ,this, _cardUISettings,_soundEvent)
            //};


            InitCardUI();
        }


        /// <summary>
        /// Return a Transition Class For The Card UI
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns>DrawCardUIHandler, RemoveCardAfterActivated, DiscardHandHandler, CraftCardUIHandler</returns>
        //private T GetCardUIHandler<T>() where T : CardUITransition
        //{
        //    for (int i = 0; i < _transitions.Length; i++)
        //    {
        //        if (_transitions[i].GetType() == typeof(T))
        //        {
        //            return _transitions[i] as T;
        //        }
        //    }
        //    return null;
        //}


        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;
     //       card.Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Locked;
        //    var handler = GetCardUIHandler<RemoveCardAfterACtivated>();
        //    StartCoroutine(handler.MoveCardsUI(new CardUI[1] { card }, GetDeckPosition(DeckEnum.Disposal), card.transform.localPosition));

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
            EndTurnButton._OnFinishTurnPress += OnFinishTurn;
        }

        ~CardUIHandler() => EndTurnButton._OnFinishTurnPress -= OnFinishTurn;
        private void OnFinishTurn()
        {
            CardUITouchedReleased(false, null);
            
        }
        internal void CardUITouched(CardUI cardReference)
        {
            if (cardReference == _selectedCardUI)
                return;



            DeckManager.Instance.TransferCard(true, DeckEnum.Hand,DeckEnum.Selected, cardReference.GFX.GetCardReference);
            _handUI.ReplaceCard(_selectedCardUI, cardReference);

            _selectedCardUI.gameObject.SetActive(true);
            cardReference.GFX.GlowCard(true);

            _selectedCardUI = cardReference;
            _handUI.LockCardsInput(true);


        }
        internal void CardUITouchedReleased(bool ExecuteSucceded,CardUI cardReference)
        {
         
            var card = _selectedCardUI;
            if (card == null)
                return;

            card?.GFX.GlowCard(false);

                if (ExecuteSucceded==false)
                    DeckManager.Instance.TransferCard(true, DeckEnum.Selected, DeckEnum.Hand, _selectedCardUI.GFX.GetCardReference);

            InputManager.Instance.RemoveObjectFromTouch();
            card.CardAnimator.ResetAllAnimations();    
            _handUI.LockCardsInput(false);
            cardReference?.GFX.GlowCard(false);
            card.gameObject.SetActive(false);
            card.Inputs.CardStateMachine.MoveToState(CardStateMachine.CardUIInput.None);
            // card.CardTranslations.CancelAllTweens();
       //     _selectedCardUI = null;

        }

        internal void ToZoomCardUI()
        {
            var card = _selectedCardUI;
            if (card == null)
                return;
            //     card.Inputs.GetCanvasGroup.blocksRaycasts = false;
            // card.CardTranslations?.SetPosition( Vector2.zero);

           // card.CardTranslations?.MoveCard(false, Vector2.zero, _cardUISettings.GetCardScaleDelay);
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

            card.CardAnimator.ScaleAnimation(false);

        }


        internal bool TryExecuteCardUI(CardUI thisCardUI)
        {
            var card = DeckManager.Instance.GetCardFromDeck(true, 0, DeckEnum.Selected);
            if (card == null)
            {
                OnFinishTurn();
                Debug.LogError("CardUIHandler - Selected Card  is null !");

            }
            bool succeded = CardExecutionManager.Instance.TryExecuteCard(true, card);
            if (succeded )
            {
                //              _OriginalCard.Inputs.InHandInputState.HasValue = false;
                OnExecuteCardUI?.Invoke(_selectedCardUI.transform.localPosition, _OriginalCard);
                int index = _handUI.GetCardIndex(_selectedCardUI);
             }
            return succeded;
           
        }

    }

















    //public abstract class CardUITransition
    //{
    //    protected CardUIManager _cardUIManager;
    //    protected SoundsEvent _soundEvent;
    //    protected CardUISO _cardSettings;
      
    //    public CardUITransition(CardUIManager cuim,CardUISO so, SoundsEvent soundEvent)
    //    {
    //        _cardUIManager = cuim;
    //        _soundEvent = soundEvent;
    //        _cardSettings = so;
    //    }
    //    public abstract IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos);

    //}

}