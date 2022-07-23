using Battle;
using Battle.Deck;
using Battle.Turns;
using Battle.UI;
using CardMaga.Card;
using CardMaga.Input;
using CardMaga.UI.Card;
using DG.Tweening;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.UI
{
    public class HandUI : MonoBehaviour, ILockabel , IGetCardsUI
    {
        public static event Action OnCardDrawnAndAlign;
        public static event Action OnDiscardAllCards;
        public static event Action<CardUI> OnCardSelect; 
        public static event Action<CardUI> OnCardReturnToHand;
        public static event Action<IReadOnlyList<CardUI>> OnCardsAddToHand; //when new cards are added to the hand, passing the cards
        public static event Action<IReadOnlyList<CardUI>> OnCardsExecuteGetCards;//when card execute, and passes all the cards that are in the hand

        [SerializeField] [Tooltip("BlaBla")] private TransitionPackSO _drawTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardTransitionPackSo;
        [SerializeField] private TransitionPackSO _reAlignTransitionPackSo;
        [SerializeField] private TransitionPackSO _resetCardPositionPackSO;
        [SerializeField] private TransitionPackSO _dicardExecutePositionPackSO;

        [SerializeField] private BaseStateMachine _battleInputStateMachine;
        [SerializeField] private StateIdentificationSO _lockState;

        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;


        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private FollowCardUI _followCard;
        [SerializeField] private ZoomCardUI _zoomCard;
        [SerializeField] private TableCardSlot _tableCardSlot;

        [SerializeField] private float _delayBetweenCardDrawn;
        [SerializeField] private float _delayBetweenCardDiscard;

        private Sequence _currentSequence;

        private TokenMachine _handLockTokenMachine;
        private bool _isCardSelected;
        private WaitForSeconds _waitForCardDiscardDelay;

        private WaitForSeconds _waitForCardDrawnDelay;

        public ITokenReciever HandLockTokenReceiver => _handLockTokenMachine;

        public bool IsCardSelect
        {
            get => _isCardSelected;
        }

        public IReadOnlyList<CardUI> CardsUI
        {
            get => _tableCardSlot.GetCardUIsFromTable();
        }

        private void Awake()
        {
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _waitForCardDiscardDelay = new WaitForSeconds(_delayBetweenCardDiscard);
            _handLockTokenMachine = new TokenMachine(UnLockInput, LockInput);

            EndPlayerTurn.OnPlayerEndTurn += ForceDiscardCards;
            BattleManager.OnGameEnded += ForceDiscardCards;
            DeckManager.OnDrawCards += DrawCardsFromDeck;
            FollowCardUI.OnCardExecute += DiscardCard;
            _isCardSelected = false;
        }

        private void OnDestroy()
        {
            BattleManager.OnGameEnded -= ForceDiscardCards;
            DeckManager.OnDrawCards -= DrawCardsFromDeck;
            FollowCardUI.OnCardExecute -= DiscardCard;
            EndPlayerTurn.OnPlayerEndTurn -= ForceDiscardCards;

            for (var i = 0; i < _tableCardSlot.CardSlots.Count; i++)
            {
                var currentSlot = _tableCardSlot.CardSlots[i];

                if (currentSlot.IsHaveValue) RemoveInputEvents(currentSlot.CardUI.Inputs);
            }
        }

        public void LockInput()
        {
            LockCardsInput(false);
        }

        public void UnLockInput()
        {
            LockCardsInput(true);
        }

        private void DrawCardsFromDeck(params CardData[] cards)
        {
            _zoomCard.ForceReleaseCard();
            _followCard.ForceReleaseCard();
            
            CardUI[] _handCards;

            _handCards = _cardUIManager.GetCardsUI(cards);
            OnCardsAddToHand?.Invoke(_handCards);
            AddCards(_handCards);
            SetCardAtDrawPos(_handCards);
            ReAlignCardUI(_tableCardSlot.GetCardSlotsExceptFrom(_handCards));
            StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(_handCards), OnCardDrawnAndAlign));
        }

        private void AddCards(params CardUI[] cards)
        {
            _tableCardSlot.AddCardUIToCardSlot(cards);

            for (var i = 0; i < cards.Length; i++)
            {
                AddInputEvents(cards[i].Inputs);
            }
        }

        private void AddInputEvents(CardUIInputHandler cardUIInput)
        {
            cardUIInput.OnPointDown += RemoveCardUIFromHand;
        }

        private void RemoveInputEvents(CardUIInputHandler cardUIInput)
        {
            cardUIInput.OnPointDown -= RemoveCardUIFromHand;
        }

        private void RemoveCardUIFromHand(CardUI cardUI)
        {
            if (_isCardSelected)
                return;

            if (_tableCardSlot.ContainCardUIInSlots(cardUI, out CardSlot cardSlot))
            {
                RemoveInputEvents(cardUI.Inputs);
                cardUI.transform.SetAsLastSibling();
                cardUI.Inputs.OnClick += _zoomCard.SetZoomCard;
                cardUI.Inputs.OnBeginHold += _followCard.SetSelectCardUI;
                _tableCardSlot.RemoveCardUI(cardUI);
                _isCardSelected = true;
                OnCardSelect?.Invoke(cardUI);
            }
        }

        public void ReturnCardUIToHand(CardUI cardUI)
        {
            if (cardUI != null && !_tableCardSlot.ContainCardUIInSlots(cardUI, out CardSlot cardSlot))
            {
                _tableCardSlot.AddCardUIToCardSlot(cardUI);
                ResetCard(cardUI);
                OnCardReturnToHand?.Invoke(cardUI);
                _isCardSelected = false;
            }
        }

        public void ForceReturnCardUIToHand(CardUI cardUI)
        {
            if (cardUI == null)
                return;

            _tableCardSlot.AddCardUIToCardSlot(cardUI);
            DeckManager.Instance.TransferCard(true, DeckEnum.Selected, DeckEnum.Hand, cardUI.CardData); 
            _isCardSelected = false;
        }

        private void DiscardCard(CardUI cardUI)
        {
            if (cardUI == null)
                return;

            cardUI.CardVisuals.SetExecutedCardVisuals();
            RemoveInputEvents(cardUI.Inputs);
            MoveCardToDiscardAfterExecute(cardUI);
            _isCardSelected = false; 
            OnCardsExecuteGetCards?.Invoke(_tableCardSlot.GetCardUIsFromTable());
            //OnCardReturnToHand?.Invoke(cardUI);
        }

        private void ResetCard(CardUI cardUI)
        {
            KillTween();
            Sequence temp = cardUI.CardTransitionManager
                .Transition(_tableCardSlot.GetCardSlotFrom(cardUI).CardPos, _resetCardPositionPackSO)
                .OnComplete(() => AddInputEvents(cardUI.Inputs));
        }

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].CardTransitionManager.SetPosition(_drawPos);
                cards[i].CardTransitionManager.SetScale(0.1f);
            }
        }

        private void ReAlignCardUI(IReadOnlyList<CardSlot> cardSlots)
        {
            for (var i = 0; i < cardSlots.Count; i++)
                cardSlots[i].CardUI.CardTransitionManager.Transition(cardSlots[i].CardPos, _reAlignTransitionPackSo);
        }

        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots, Action onComplete = null)
        {
            for (var i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].CardUI.Init();
                cardSlots[i].CardUI.transform.SetAsLastSibling();
                cardSlots[i].CardUI.CardTransitionManager.Transition(cardSlots[i].CardPos, _drawTransitionPackSo);
                yield return _waitForCardDrawnDelay;
            }

            onComplete?.Invoke();
        }

        private void DiscardCards(params CardUI[] cardUI)
        {
            StartCoroutine(MoveCardToTheDiscardPosition(cardUI));
        }

        private void ForceDiscardCards()
        {
            _zoomCard.ForceReleaseCard();
            _followCard.ForceReleaseCard();
            _battleInputStateMachine.ForceChangeState(_lockState);
            
            CardUI[] tempCardUis = _tableCardSlot.GetCardUIsFromTable();
            
            for (int i = 0; i < tempCardUis.Length; i++)
            {
                RemoveInputEvents(tempCardUis[i].Inputs); 
            }
            
            _tableCardSlot.RemoveAllCardUI();
            //OnDiscardAllCards?.Invoke();
            DiscardCards(tempCardUis);
        }

        private IEnumerator MoveCardToTheDiscardPosition(params CardUI[] cardUI)
        {
            for (var i = 0; i < cardUI.Length; i++)
            {
                cardUI[i].CardTransitionManager.Transition(_discardPos, _discardTransitionPackSo,cardUI[i].Dispose);
                yield return _waitForCardDiscardDelay;
            }
        }

        private void MoveCardToDiscardAfterExecute(CardUI cardUI)
        {
            cardUI.CardTransitionManager.Transition(_discardPos, _discardTransitionPackSo, cardUI.Dispose);
        }

        /// <summary>
        ///     Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        private void LockCardsInput(bool toLock)
        {
            for (var i = 0; i < _tableCardSlot.CardSlots.Count; i++)
            {
                var currentSlot = _tableCardSlot.CardSlots[i];
                if (currentSlot.IsHaveValue) currentSlot.CardUI.Inputs.ForceChangeState(toLock);
            }
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }
    }

    [Serializable]
    public class CardSlot
    {
        [SerializeField,ReadOnly] private CardUI _cardUI;
        private Vector2 _cardPos;

        public Vector2 CardPos
        {
            get => _cardPos;
            set => _cardPos = value;
        }

        public bool IsHaveValue => !ReferenceEquals(CardUI, null) || _cardUI != null;

        public CardUI CardUI
        {
            get => _cardUI;
            private set => _cardUI = value;
        }

        public void AssignCardUI(CardUI cardUI)
        {
            CardUI = cardUI;
        }

        public void RemoveCardUI()
        {
            CardUI = null;
        }

        public bool IsContainCardUI(CardUI cardUI)
        {
            if (ReferenceEquals(cardUI, null) || !IsHaveValue || cardUI == null)
                return false;

            if (cardUI.CardData.CardInstanceID == _cardUI.CardData.CardInstanceID) 
                return true;

            return false;
        }
    }

    [Serializable]
    public class TableCardSlot
    {
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] [Range(0, 25)] private float _cardPaddingPrecentage;
        [SerializeField] [Range(-10, 10)] private float _cardCenterOffSetPrecentage;
        [SerializeField] [ReadOnly] private List<CardSlot> _cardSlots = new List<CardSlot>();

        public IReadOnlyList<CardSlot> CardSlots => _cardSlots;


        private void AlignCardsSlots()
        {
            for (var i = 0; i < _cardSlots.Count; i++) _cardSlots[i].CardPos = CalculateCardPosition(i);
        }

        private Vector2 CalculateCardPosition(int index)
        {
            Vector2 startPos = _middleHandPos.position;

            float screenWidth = Screen.width;
            var screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100);
            var spaceBetweenCard = (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
            var offSetFromCenter = screenWidth / 2 * (_cardCenterOffSetPrecentage / 100);

            var destination = startPos + index * spaceBetweenCard * Vector2.right;

            var xMaxDistance = _cardSlots.Count * spaceBetweenCard / 2;
            var offset = spaceBetweenCard / 2 + offSetFromCenter;
            destination += Vector2.left * (xMaxDistance - offset);

            destination += Vector2.right * _cardCenterOffSetPrecentage;
            return destination;
        }

        private float CalculateSpaceBetweenCards()
        {
            float screenWidth = Screen.width;

            var screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100);

            return (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
        }

        public void AddCardUIToCardSlot(params CardUI[] cardUI)
        {
            var isNoMoreSpace = _cardSlots.Count == 0;

            for (var i = 0; i < cardUI.Length; i++) //loop over all the received CardUIs and Assign to  cardslot 
            {
                if (!isNoMoreSpace && !ContainCardUIInSlots(cardUI[i], out CardSlot cardSlot)) 
                    for (var j = 0; j < _cardSlots.Count; j++) //loop over all the cardslot and check if there is a available slot
                    {
                        if (!_cardSlots[j].IsHaveValue)
                        {
                            _cardSlots[j].AssignCardUI(cardUI[i]);
                            break;
                        }

                        //if (j == _cardSlots.Count - 1) // did not find a empty cardslot 
                            //isNoMoreSpace = true;
                    }

                if (isNoMoreSpace)
                {
                    AddCardSlot(cardUI[i]); //create a new cardslot and assign a cardui to it
                }
                    
            }

            AlignCardsSlots();
        }

        // public CardUI RemoveCardUIFromCardSlot(CardSlot cardSlot)
        // {
        //     
        // }

        public bool RemoveCardUI(CardUI cardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    _cardSlots[i].RemoveCardUI();
                    return true;
                }

            return false;
        }

        public void RemoveAllCardUI()
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    _cardSlots[i].RemoveCardUI();
        }

        public bool ContainCardUIInSlots(CardUI cardUI, out CardSlot cardSlot)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    cardSlot = _cardSlots[i];
                    return true;
                }
            }
            
            cardSlot = null;
            return false;
        }

        public IReadOnlyList<CardSlot> GetCardSlotsFrom(params CardUI[] cardUIs)
        {
            var cardSlots = new List<CardSlot>();

            for (var i = 0; i < cardUIs.Length; i++)
                for (var j = 0; j < _cardSlots.Count; j++)
                    if (_cardSlots[j].IsContainCardUI(cardUIs[i]))
                    {
                        cardSlots.Add(_cardSlots[j]);
                        break;
                    }

            return cardSlots;
        }

        public CardUI[] GetCardUIsFromTable()
        {
            var tempCardUI = new List<CardUI>();

            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    tempCardUI.Add(_cardSlots[i].CardUI);

            return tempCardUI.ToArray();
        }

        public CardSlot GetCardSlotFrom(CardUI cardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(cardUI))
                    return _cardSlots[i];

            Debug.LogError(cardUI.name + " is not found");
            return null;
        }


        /// <summary>
        ///     Get a List Of cardSlots Except the cardSlots that contain the params of cardUI
        /// </summary>
        /// <param name="cardUIs">The cardUI that will not be include</param>
        /// <returns></returns>
        public IReadOnlyList<CardSlot> GetCardSlotsExceptFrom(params CardUI[] cardUIs)
        {
            var cardSlots = new List<CardSlot>();
            var cardUIList = cardUIs.ToList(); //A list of cards we want to ignore and not send back

            var isFound = false;

            for (var i = 0; i < _cardSlots.Count; i++) //loop over all the cardslots and check if they contain the specific cardui
            {
                isFound = false;

                for (var j = 0; j < cardUIList.Count; j++)
                    if (_cardSlots[i].IsContainCardUI(cardUIList[j]))
                    {
                        isFound = true; //if the cardui was found we will update this variable to true
                        cardUIList.RemoveAt(
                            j); //If we found the card we will not have to keep checking if it is in another slot
                        break;
                    }

                if (!isFound) //If the card is not found it means that we did not ask to ignore it and we will add it to the list
                    cardSlots.Add(_cardSlots[i]);
            }

            return cardSlots;
        }

        public void AdjustSize(int size)
        {
        }

        private void AddCardSlot(CardUI cardUI)
        {
            var cardSlot = new CardSlot();
            _cardSlots.Add(cardSlot);
            cardSlot.AssignCardUI(cardUI);
        }

        private void RemoveCardSlot(CardSlot cardSlot)
        {
        }

        public void ClearCardSlot()
        {
        }
    }

    public interface IGetCardsUI
    {
        IReadOnlyList<CardUI> CardsUI { get; }
    }
}