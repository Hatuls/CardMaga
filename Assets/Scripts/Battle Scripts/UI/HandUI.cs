using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Deck;
using Battles.Turns;
using Cards;
using DG.Tweening;
using UnityEngine;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;

namespace Battles.UI
{
    public class HandUI : MonoBehaviour , ILockabel
    {
        public static event Action OnCardDrawnAndAlign;
        public static event Action OnCardSelect;
        public static event Action OnCardReturnToHand;
        

        private TokenMachine _handLockTokenMachine;
        
        [SerializeField,Tooltip("BlaBla")] private TransitionPackSO _drawTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardTransitionPackSo;
        [SerializeField] private TransitionPackSO _reAlignTransitionPackSo;
        [SerializeField] private TransitionPackSO _resetCardPositionPackSO;
        
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;
        

        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private FollowCardUI _followCard;
        [SerializeField] private ZoomCardUI _zoomCard;
        [SerializeField] private TableCardSlot _tableCardSlot;
        
        [SerializeField] private float _delayBetweenCardDrawn;
        [SerializeField] private float _delayBetweenCardDiscard;

        private bool _isCardSelected = false;
        
        private WaitForSeconds _waitForCardDrawnDelay;
        private WaitForSeconds _waitForCardDiscardDelay;
        
        private Sequence _currentSequence;
        
        public ITokenReciever HandLockTokenReceiver
        {
            get { return _handLockTokenMachine; }
        }
        
        private void Awake()
        {
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _waitForCardDiscardDelay = new WaitForSeconds(_delayBetweenCardDiscard);
            _handLockTokenMachine = new TokenMachine(UnLockInput, LockInput);

            EndPlayerTurn.OnPlayerEndTurn += ForceDiscardCards;
            BattleManager.OnGameEnded += LockInput;
            DeckManager.OnDrawCards += DrawCardsFromDeck;
            _followCard.OnCardExecut += OnCardExecute;
        }

        private void OnDestroy()
        {
            BattleManager.OnGameEnded -= LockInput;
            DeckManager.OnDrawCards -= DrawCardsFromDeck;
            _followCard.OnCardExecut -= OnCardExecute;

            for (int i = 0; i < _tableCardSlot.CardSlots.Count; i++)
            {
                CardSlot currentSlot = _tableCardSlot.CardSlots[i];
                
                if (currentSlot.IsHaveValue)
                {
                    RemoveInputEvents(currentSlot.CardUI.Inputs);
                }
            }
        }

        public void DrawCardsFromDeck(params Card[] cards)
        {
            CardUI[] _handCards;
             
            _handCards = _cardUIManager.GetCardsUI(cards);
            
            AddCards(_handCards);
            SetCardAtDrawPos(_handCards);
            ReAlignCardUI(_tableCardSlot.GetCardSlotsExceptFrom(_handCards));
            StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(_handCards),OnCardDrawnAndAlign));
        }

        private void AddCards(params CardUI[] cards)
        {
            _tableCardSlot.AddCardUIToCardSlot(cards);
             
            for (int i = 0; i < cards.Length; i++)
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

            if (_tableCardSlot.ContainCardUIInSlots(cardUI))
            {
                cardUI.transform.SetAsLastSibling();
                _tableCardSlot.RemoveCardUI(cardUI);
                OnCardSelect?.Invoke();
                _isCardSelected = true;
                DeckManager.Instance.TransferCard(true,DeckEnum.Hand,DeckEnum.Selected,cardUI.CardData);
                RemoveInputEvents(cardUI.Inputs);
                cardUI.Inputs.OnClick += _zoomCard.SetZoomCard;
                cardUI.Inputs.OnBeginHold += _followCard.SetSelectCardUI;
            }
        }

        public void ReturnCardUIToHand(CardUI cardUI)
        {
            if (!_tableCardSlot.ContainCardUIInSlots(cardUI))
            {
                _tableCardSlot.AddCardUIToCardSlot(cardUI);
                DeckManager.Instance.TransferCard(true,DeckEnum.Selected,DeckEnum.Hand,cardUI.CardData);
                ResetCard(cardUI);
                OnCardReturnToHand?.Invoke();
                _isCardSelected = false;
                Debug.Log("Add " + cardUI.name + " To Hand");
            }
        }

        public void ForceReturnCardUIToHand(CardUI cardUI)
        {
            if (!_tableCardSlot.ContainCardUIInSlots(cardUI))
            {
                _tableCardSlot.AddCardUIToCardSlot(cardUI);
                DeckManager.Instance.TransferCard(true,DeckEnum.Selected,DeckEnum.Hand,cardUI.RecieveCardReference());
                OnCardReturnToHand?.Invoke();
                _isCardSelected = false;
            }
        }

        private void OnCardExecute(CardUI cardUI)
        {
            DiscardCards(cardUI);
            _isCardSelected = false;
            OnCardReturnToHand?.Invoke();
        }
        
        public void ResetCard(CardUI cardUI)
        {
            KillTween();
            cardUI.CardTransitionManager
                .Transition(_tableCardSlot.GetCardSlotFrom(cardUI).CardPos, _resetCardPositionPackSO)
                .OnComplete(() => AddInputEvents(cardUI.Inputs));
        }

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].CardTransitionManager.SetPosition(_drawPos);
                cards[i].CardTransitionManager.SetScale(0.1f);
            }           
        }

        private void ReAlignCardUI(IReadOnlyList<CardSlot> cardSlots)
        {
            for (int i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].CardUI.CardTransitionManager.Transition(cardSlots[i].CardPos,_reAlignTransitionPackSo);
            }
        }
        
        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots,Action onComplete = null)
        {
            for (int i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].CardUI.Init();
                cardSlots[i].CardUI.CardTransitionManager.Transition(cardSlots[i].CardPos,_drawTransitionPackSo);
                yield return _waitForCardDrawnDelay;
            }
            onComplete?.Invoke();
        }

        public void DiscardCards(params CardUI[] cardUI)
        {
            StartCoroutine(MoveCardToTheDiscardPosition(cardUI));
        }
        
        public void ForceDiscardCards()
        {
            _followCard.ForceReleaseCard();
            StartCoroutine(MoveCardToTheDiscardPosition(_tableCardSlot.GetCardUIsFromTable()));
            _tableCardSlot.RemoveAllCardUI();
        }

        private IEnumerator MoveCardToTheDiscardPosition(params CardUI[] cardUI)
        {
            for (int i = 0; i < cardUI.Length; i++)
            {
                cardUI[i].CardTransitionManager.Transition(_discardPos, _discardTransitionPackSo);
                yield return _waitForCardDiscardDelay;
            }

            for (int i = 0; i < cardUI.Length; i++)
            {
                cardUI[i].Dispose();
            }
        }
        
        /// <summary>
        /// Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        private void LockCardsInput(bool toLock)
        {
            for (int i = 0; i < _tableCardSlot.CardSlots.Count; i++)
            {
                CardSlot currentSlot = _tableCardSlot.CardSlots[i];
                if (currentSlot.IsHaveValue)
                {
                    currentSlot.CardUI.Inputs.ForceChangeState(toLock);
                }
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
        
        private void KillTween()
        {
            if (_currentSequence != null)
            {
                _currentSequence.Kill();
            }
        }
    }
    
    [Serializable]
    public class CardSlot
    {
        private Vector2 _cardPos;
        [SerializeField] private CardUI _cardUI;

        public Vector2 CardPos
        {
            get => _cardPos;
            set => _cardPos = value;
        }

        public bool IsHaveValue
        {
            get { return !ReferenceEquals(CardUI, null); }
        }

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
            if (ReferenceEquals(cardUI, null) || !IsHaveValue)
                return false;
            
            if (cardUI.CardData.CardInstanceID == CardUI.CardData.CardInstanceID)
            {
                return true;
            }

            return false;
        }
    }
    
    [Serializable]
    public class TableCardSlot
    {
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField,Range(0,25)] private float _cardPaddingPrecentage;
        [SerializeField,Range(-10,10)] private float _cardCenterOffSetPrecentage;
        [ShowInInspector,ReadOnly] private List<CardSlot> _cardSlots = new List<CardSlot>();

        public IReadOnlyList<CardSlot> CardSlots
        {
            get { return _cardSlots; }
        }
        
        
        private void AlignCardsSlots()
        {
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                _cardSlots[i].CardPos = CalculateCardPosition(i);
            }
        }
        
        private Vector2 CalculateCardPosition(int index)
        {
            Vector2 startPos = _middleHandPos.position;
            
            float screenWidth = Screen.width;
            float screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100);
            float spaceBetweenCard = (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
            float offSetFromCenter = (screenWidth / 2) * (_cardCenterOffSetPrecentage / 100);
            
            Vector2 destination = startPos + index * spaceBetweenCard * Vector2.right;

            float xMaxDistance = _cardSlots.Count * spaceBetweenCard / 2;
            float offset = spaceBetweenCard / 2 + offSetFromCenter;
            destination += Vector2.left * (xMaxDistance - offset);

            destination += Vector2.right * _cardCenterOffSetPrecentage;
            return destination;
        }

        private float CalculateSpaceBetweenCards()
        {
            float screenWidth = Screen.width;

            float screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100); 
            
            return (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
        }

        public void AddCardUIToCardSlot(params CardUI[] cardUI)
        {
            bool isNoMoreSpace = _cardSlots.Count == 0;
            
            for (int i = 0; i < cardUI.Length; i++) //loop over all the received CardUIs and Assign to  cardslot 
            {
                if (!isNoMoreSpace)
                {
                    for (int j = 0; j < _cardSlots.Count; j++) //loop over all the cardslot and check if there is a available slot
                    {
                        if (!_cardSlots[j].IsHaveValue)
                        {
                            _cardSlots[j].AssignCardUI(cardUI[i]);
                            break;
                        }

                        if (j == _cardSlots.Count - 1) // did not find a empty cardslot 
                        {
                            isNoMoreSpace = true;
                        }
                    }
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
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    _cardSlots[i].RemoveCardUI();
                    return true;
                }
            }

            return false;
        }
        
        public void RemoveAllCardUI()
        {
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsHaveValue)
                {
                    _cardSlots[i].RemoveCardUI();
                }
            }
        }
        
        public bool ContainCardUIInSlots(CardUI cardUI)
        {
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    return true;
                }
            }

            return false;
        }
        
        public void RemoveAllCardUIFromCardSlots()
        {
            
        }

        public IReadOnlyList<CardSlot> GetCardSlotsFrom(params CardUI[] cardUIs)
        {
            List<CardSlot> cardSlots = new List<CardSlot>();

            for (int i = 0; i < cardUIs.Length; i++)
            {
                for (int j = 0; j < _cardSlots.Count; j++)
                {
                    if (_cardSlots[j].IsContainCardUI(cardUIs[i]))
                    {
                        cardSlots.Add(_cardSlots[j]);
                        break;
                    }
                }
            }

            return cardSlots;
        }

        public CardUI[] GetCardUIsFromTable()
        {
            List<CardUI> tempCardUI = new List<CardUI>();

            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsHaveValue)
                {
                    tempCardUI.Add(_cardSlots[i].CardUI);
                }
            }

            return tempCardUI.ToArray();
        }
        
        public CardSlot GetCardSlotFrom(CardUI cardUI)
        {
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    return _cardSlots[i];
                }
            }

            Debug.LogError(cardUI.name + " is not found");
            return null;
        }
        
        
        /// <summary>
        /// Get a List Of cardSlots Except the cardSlots that contain the params of cardUI
        /// </summary>
        /// <param name="cardUIs">The cardUI that will not be include</param>
        /// <returns></returns>
        public IReadOnlyList<CardSlot> GetCardSlotsExceptFrom(params CardUI[] cardUIs)
        {
            List<CardSlot> cardSlots = new List<CardSlot>();
            List<CardUI> cardUIList = cardUIs.ToList();//A list of cards we want to ignore and not send back

            bool isFound = false;

            for (int i = 0; i < _cardSlots.Count; i++) //loop over all the cardslots and check if they contain the specific cardui
            {
                isFound = false;
                
                for (int j = 0; j < cardUIList.Count; j++)
                {
                    if (_cardSlots[i].IsContainCardUI(cardUIList[j]))
                    {
                        isFound = true;//if the cardui was found we will update this variable to true
                        cardUIList.RemoveAt(j);//If we found the card we will not have to keep checking if it is in another slot
                        break;
                    }
                }

                if (!isFound)//If the card is not found it means that we did not ask to ignore it and we will add it to the list
                {
                    cardSlots.Add(_cardSlots[i]);
                }
            }

            return cardSlots;
        }

        public void AdjustSize(int size)
        {
            
        }

        private void AddCardSlot(CardUI cardUI)
        {
            CardSlot cardSlot = new CardSlot();
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


}