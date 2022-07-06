using System;
using System.Collections;
using System.Collections.Generic;
using Battles.Deck;
using Cards;
using UnityEngine;
using ReiTools.TokenMachine;

namespace Battles.UI
{
    public class HandUI : MonoBehaviour , ILockabel
    {
        public  event Action OnCardDrawnAndAlign;
        private event Action OnCardAtDicardPosition;

        private TokenMachine _handLockTokenMachine;
        
        [SerializeField] private BaseState _battleInput;

        [SerializeField] private TransitionPackSO _drawTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardTransitionPackSo;
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;
        
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private float _spaceBetweenCard;

        [SerializeField] private SelectCardUI _selectCard;

        [SerializeField] private float _delayBetweenCardDrawn;
        
        [SerializeField] private Canvas canvasTest;
    
        private List<CardSlot> _cardSlots;

        private List<CardUI> _handCards;

        private WaitForSeconds _waitForCardDrawnDelay;
        
        private void Awake()
        {
            _handCards = new List<CardUI>();
            BattleManager.OnGameEnded += LockInput;
            DeckManager.OnDrawCards += DrawCards;
            _selectCard.OnDisposeCard += AddCardUIToHand;
            _cardSlots = new List<CardSlot>();
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _handLockTokenMachine = new TokenMachine(UnLockInput, LockInput);
        }

        private void OnDestroy()
        {
            BattleManager.OnGameEnded -= LockInput;
            DeckManager.OnDrawCards -= DrawCards;
            _selectCard.OnDisposeCard -= AddCardUIToHand;
            
            for (int i = 0; i < _handCards.Count; i++)
            {
                RemoveInputEvents(_handCards[i].Inputs);
            }
        }

        public ITokenReciever HandLockTokenReceiver
        {
            get { return _handLockTokenMachine; }
        }

        public void AlignCards()
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _cardSlots.Add( new CardSlot(CalculateCardPosition(i),_handCards[i]));
                _handCards[i].SetCardHandPos(CalculateCardPosition(i));
            }
        }

        public void DrawCards(params Card[] cards)
        {
            _handCards.AddRange(_cardUIManager.GetCardsUI(cards));
            
            AddCardToTheInputState(_handCards.ToArray());//need Work!!!
            
            AlignCards();
            SetCardAtDrawPos(_handCards.ToArray());

            for (int i = 0; i < _handCards.Count; i++)
            {
                AddInputEvents(_handCards[i].Inputs);
            }

            Debug.Log("Card Drawn");
            StartCoroutine(MoveCardsToHandPos(_cardSlots,OnCardDrawnAndAlign));
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
            if (_handCards.Contains(cardUI))
            {
                _handCards.Remove(cardUI);
                _selectCard.SetSelectCardUI(cardUI);
            }
        }

        private void AddCardUIToHand(CardUI cardUI)
        {
            if (!_handCards.Contains(cardUI))
            {
                _handCards.Add(cardUI);
                Debug.Log("Add " + cardUI.name + " To Hand");
            }
        }

        private void AddCardToTheInputState(CardUI[] cardsUI)
        {
            for (int i = 0; i < cardsUI.Length; i++)
            {
                
            }
        }

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].CardTransitionManager.SetPosition(_drawPos);
                cards[i].CardTransitionManager.SetScale(0.1f);
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

        [ContextMenu("Discard")]
        private void Discard()
        {
            StartCoroutine(MoveCardToTheDiscardPosition());
        }
        private IEnumerator MoveCardToTheDiscardPosition()
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _handCards[i].CardTransitionManager.Transition(_discardPos.transform.TransformPoint(_discardPos.rect.center), _discardTransitionPackSo);
                yield return _waitForCardDrawnDelay;
            }

            // for (int i = 0; i < _handCards.Count; i++)
            // {
            //     _handCards[i].gameObject.SetActive(false);
            // }
        }

        public CardUI GetHandCardUIFromIndex(int index)
        {
            
            if (index >= 0 && index < _handCards.Count)
                return _handCards[index];
            return null;
        }
        public int GetCardIndex(CardUI card)
        {
            if (card == null)
                return -1;
            return 1; //need work
        }

        /// <summary>
        /// Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        private void LockCardsInput(bool toLock)
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _handCards[i].Inputs.ForceChangeState(toLock);
            }
        }

        private Vector2 CalculateCardPosition(int index)
        {
            Vector2 startPos = _middleHandPos.position;

            Vector2 destination = startPos + Vector2.right * index * _spaceBetweenCard;

            float xMaxDistance = _handCards.Count * _spaceBetweenCard / 2;
            float offset = _spaceBetweenCard / 2;
            destination += Vector2.left * (xMaxDistance - offset);
            return destination;
        }

        public void LockInput()
        {
            LockCardsInput(false);
        }

        public void UnLockInput()
        {
            LockCardsInput(true);
        }
    }

    public class CardSlot
    {
        public Vector2 CardPos { get; set; }

        public bool IsHaveValue { get; set; }

        public CardUI CardUI { get; set; }
        
        public CardSlot(Vector2 pos,CardUI cardUI)
        {
            CardUI = cardUI;
            CardPos = pos;
            IsHaveValue = true;
        }
    }

}