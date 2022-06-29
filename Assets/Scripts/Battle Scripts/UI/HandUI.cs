using System;
using System.Collections;
using System.Collections.Generic;
using Battles.Deck;
using Cards;
using UnityEngine;

namespace Battles.UI
{
    public class HandUI : MonoBehaviour
    {
        public static event Action OnCardDrawnAndAlign;
        [SerializeField] private CardTransitions _transitions;
        [SerializeField] private BattleInputDefaultState _battleInput;
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private float _spaceBetweenCard;

        private SelectCardUI _selectCard;

        [SerializeField] private float _delayBetweenCardDrawn;
        
        [SerializeField] private Canvas canvasTest;
    
        private List<CardSlot> _cardSlots;

        private List<CardUI> _handCards;

        private WaitForSeconds _waitForCardDrawnDelay;
        
        private void Awake()
        {
            _handCards = new List<CardUI>();
            BattleManager.OnGameEnded += LockCards;
            DeckManager.OnDrawCards += DrawCards;
            _cardSlots = new List<CardSlot>();
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _selectCard = new SelectCardUI();
        }

        private void OnDestroy()
        {
            BattleManager.OnGameEnded -= LockCards;
            DeckManager.OnDrawCards -= DrawCards;
        }

        public void LockCards() => LockCardsInput(true);
        
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
            AddINputEvents();
            StartCoroutine(MoveCardsToHandPos(_cardSlots,OnCardDrawnAndAlign));
            InputReciever.OnTouchDetectd += GetMousePos;
        }

        private void AddINputEvents()
        {
            for (int i = 0; i < _handCards.Count; i++)
            {
                _handCards[i].Inputs.OnPointDown += SetSelectCard;
                _handCards[i].Inputs.OnEndHold += RelaseCard;
            }
        }

        private void AddCardToTheInputState(CardUI[] cardsUI)
        {
            for (int i = 0; i < cardsUI.Length; i++)
            {
                _battleInput.AddTouchableItem(cardsUI[i].GetComponent<CardUIInputHandler>());
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
                cardSlots[i].CardUI.CardTransitionManager.Transition(cardSlots[i].CardPos,_transitions.TransitionPackSos[1]);
                yield return _waitForCardDrawnDelay;
            }
            onComplete?.Invoke();
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
        
        internal void ReplaceCard(CardUI ReplacingCard, CardUI ReplacedCard)
        {
            
        }

        internal void DiscardHand()
        {
            
        }

        private void FollowHand()
        {
            
        }

        private void RelaseCard(CardUI cardUI)
        {
            if (_selectCard.Card == cardUI)
            {
                _selectCard.Card.CardTransitionManager.Move(_selectCard.Card.HandPos,_transitions.TransitionPackSos[0]);
            }
        }

        private void GetMousePos(Vector2 mousePos)
        {
            Debug.Log(mousePos);
            _selectCard.Card.CardTransitionManager.Move(mousePos,_transitions.TransitionPackSos[0]);
        }

        private void SetSelectCard(CardUI cardUI)
        {
            _selectCard.SetSelectCardUI(cardUI);
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

public static class RectTransformHelper
{
    public static Vector2 GetLocalPosition(this RectTransform rectTransform)
        => rectTransform.transform.localPosition;
}