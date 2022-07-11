using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Deck;
using Cards;
using DG.Tweening;
using UnityEngine;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;

namespace Battles.UI
{
    public class HandUI : MonoBehaviour , ILockabel
    {
        public  event Action OnCardDrawnAndAlign;
        public event Action OnCardSelect;
        private event Action OnCardAtDicardPosition;

        private TokenMachine _handLockTokenMachine;
        
        [SerializeField,Tooltip("BlaBla")] private TransitionPackSO _drawTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardTransitionPackSo;
        [SerializeField] private TransitionPackSO _reAlignTransitionPackSo;
        [SerializeField] private TransitionPackSO _resetCardPositionPackSO;
        
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;
        
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private SelectCardUI _selectCard;
        [SerializeField] private ZoomCardUI _zoomCard;
        [SerializeField] private TableCardSlot _tableCardSlot;
        
        [SerializeField] private float _delayBetweenCardDrawn;
        
        private WaitForSeconds _waitForCardDrawnDelay;
        
        private Sequence _currentSequence;
        
        public ITokenReciever HandLockTokenReceiver
        {
            get { return _handLockTokenMachine; }
        }
        
        private void Awake()
        {
            BattleManager.OnGameEnded += LockInput;
            DeckManager.OnDrawCards += DrawCardsFromDeck;
            
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _handLockTokenMachine = new TokenMachine(UnLockInput, LockInput);
        }

        private void OnDestroy()
        {
            BattleManager.OnGameEnded -= LockInput;
            DeckManager.OnDrawCards -= DrawCardsFromDeck;

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
            if (_tableCardSlot.ContainCardUIInSlots(cardUI))
            {
                RemoveInputEvents(cardUI.Inputs);
                cardUI.Inputs.OnClick += _zoomCard.SetZoomCard;
                cardUI.Inputs.OnBeginHold += _selectCard.SetSelectCardUI;
                _tableCardSlot.RemoveCardUI(cardUI);
                OnCardSelect?.Invoke();
            }
        }

        public void AddCardUIToHand(CardUI cardUI)
        {
            if (!_tableCardSlot.ContainCardUIInSlots(cardUI))
            {
                _tableCardSlot.AddCardUIToCardSlot(cardUI);
                ResetCard(cardUI);
                Debug.Log("Add " + cardUI.name + " To Hand");
            }
        }
        
        public void ResetCard(CardUI cardUI)
        {
            KillTween();
            cardUI.CardTransitionManager
                .Transition(_tableCardSlot.GetCardSlotsFrom(cardUI)[0].CardPos, _resetCardPositionPackSO)
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
        
        private IEnumerator MoveCardToTheDiscardPosition(params CardUI[] cardUI)
        {
            for (int i = 0; i < _tableCardSlot.CardSlots.Count; i++)
            {
                //_handCards[i].CardTransitionManager.Transition(_discardPos.transform.TransformPoint(_discardPos.rect.center), _discardTransitionPackSo);
                yield return _waitForCardDrawnDelay;
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
            
            if (cardUI.RecieveCardReference().CardInstanceID == CardUI.RecieveCardReference().CardInstanceID)
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
        [SerializeField] private float _spaceBetweenCard;

        [Sirenix.OdinInspector.ShowInInspector,ReadOnly] private List<CardSlot> _cardSlots = new List<CardSlot>();

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

            Vector2 destination = startPos + index * _spaceBetweenCard * Vector2.right;

            float xMaxDistance = _cardSlots.Count * _spaceBetweenCard / 2;
            float offset = _spaceBetweenCard / 2;
            destination += Vector2.left * (xMaxDistance - offset);
            return destination;
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