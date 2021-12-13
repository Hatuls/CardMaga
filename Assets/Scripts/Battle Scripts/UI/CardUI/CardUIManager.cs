
using UnityEngine;
using Battles.Deck;
using Cards;
using System;
using Battles.UI.CardUIAttributes;
using UnityEngine.Events;

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
        UnityEvent OnPlayerRemoveHand;
        [SerializeField]
        UnityEvent OnDrawCard;
        #endregion

        #region Properties


        public RectTransform GetDrawDeckPosition => _drawDeckPosition;
        public RectTransform GetDiscardDeckPosition => _discardDeckPosition;
        public RectTransform GetExhaustDeckPosition => _exhaustDeckPosition;
        public RectTransform GetHandMiddlePosition => _handMiddlePosition;
        public float GetInputHandLine => GetHandMiddlePosition.position.y + _cardUISettings.LineOfHandTeritory;

        [SerializeField]
        private CardUI _enemyCardUI;
        internal void UpdateHand()
        {
            DrawCards(DeckManager.Instance.GetCardsFromDeck(true, DeckEnum.Hand));
        }

        internal void PlayEnemyCard(Card card)
        {
            if (_enemyCardUI.gameObject.activeSelf == false)
                ActivateEnemyCardUI(true);
            AssignDataToCardUI(_enemyCardUI,card);
    
        }

        public void ActivateEnemyCardUI(bool state)
            => _enemyCardUI.gameObject.SetActive(state);

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

            card.GFX.SetCardReference(cardData);
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
                    var InHandInputState = card.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand);
                    if (InHandInputState.HasValue ==  false)
                    {
                        if (card.gameObject.activeSelf == false)
                            card.gameObject.SetActive(true);

                        InHandInputState.HasValue = true;

                        card.CardAnimator.PlayNoticeAnimation();

                        OnDrawCard?.Invoke();

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
            OnPlayerRemoveHand?.Invoke();
      

        }
        public override void Init()
        {
            _handUI = new HandUI(_handCards, GetHandMiddlePosition.transform.localPosition, _cardUISettings);



            InitCardUI();
        }


   


        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;
   
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

        public void ResetCardUIManager()
        {
            CardUIHandler.Instance._selectedCardUI.gameObject.SetActive(false);
        }
      
    }










    public class CardUIHandler
    {
        public static Action<Vector2, CardUI> OnExecuteCardUI;
        public static CardUIHandler Instance;
      public  CardUI _selectedCardUI;
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

        ~CardUIHandler() { 
            EndTurnButton._OnFinishTurnPress -= OnFinishTurn; 
   
        }

        private void OnFinishTurn()
        {
            CardUITouchedReleased(false, null);
            
        }
        internal void CardUITouched(CardUI cardReference)
        {
            if (cardReference == _selectedCardUI || BattleManager.isGameEnded)
                return;



            DeckManager.Instance.TransferCard(true, DeckEnum.Hand,DeckEnum.Selected, cardReference.GFX.GetCardReference);
            _handUI.ReplaceCard(_selectedCardUI, cardReference);

            _selectedCardUI.gameObject.SetActive(true);
            cardReference.GFX.GlowCard(true);
            GameEventsInvoker.Instance.OnSelectCard?.Invoke();
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
            cardReference?.CardAnimator.ResetAllAnimations();
            card.CardTranslations.SetScale(Vector2.one,0);
            cardReference?.CardTranslations.SetScale(Vector2.one,0);
            _handUI.LockCardsInput(false);
            cardReference?.GFX.GlowCard(false);
            card.gameObject.SetActive(false);
            card.CardStateMachine.MoveToState(CardStateMachine.CardUIInput.None);
            // card.CardTranslations.CancelAllTweens();
       //     _selectedCardUI = null;

        }

        internal void ToZoomCardUI()
        {
            var card = _selectedCardUI;
            if (card == null)
                return;

            GameBattleDescriptionUI.Instance.CheckCardUI(card);
            GameEventsInvoker.Instance.OnZoomCard?.Invoke();
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
            GameBattleDescriptionUI.Instance.CloseCardUIInfo();
            card.GFX.GlowCard(false);

            card.CardAnimator.ScaleAnimation(false);

        }


        internal bool TryExecuteCardUI(CardUI thisCardUI)
        {
            if (BattleManager.isGameEnded)
                return false;

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