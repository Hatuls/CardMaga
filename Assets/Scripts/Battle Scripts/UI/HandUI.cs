using System;
using System.Collections;
using System.Collections.Generic;
using Battles.Deck;
using Battles.UI.CardUIAttributes;
using Cards;
using FMOD;
using UnityEngine;
using UnityEngine.XR.WSA.Sharing;
using Debug = UnityEngine.Debug;

namespace Battles.UI
{

    public class HandUI : MonoBehaviour
    {
        [SerializeField] private SelectCardUI _selectCard;
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private float _spaceBetweenCard;
        [SerializeField] private LocoMotionSO _drawTransition;
        [SerializeField] private float _delayBetweenCardDrawn;
    
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
            }
        }

        public void DrawCards(params Card[] cards)
        {
            _handCards.AddRange(_cardUIManager.GetCardsUI(cards));
            AlignCards();
            SetCardAtDrawPos(_handCards.ToArray());
            
            StartCoroutine(MoveCardsToHandPos(_cardSlots));
        }

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].CardLocoMotionUI.Transition(_drawPos.transform.position);
            }           
            Debug.Log("LocalPos: " + _drawPos.transform.localPosition);
            Debug.Log("Pos" + _drawPos.transform.position);
            Debug.Log("RectPos: " + _drawPos.rect.position);
            Debug.Log("AnchrPos: " + _drawPos.anchoredPosition);
            Debug.Log("CamPos: " + Camera.main.ScreenToWorldPoint(_drawPos.transform.position));
            Debug.Log("CamLocalPos: " + Camera.main.ScreenToWorldPoint(_drawPos.transform.localPosition));
            Debug.Log("CamRectPos: " + Camera.main.ScreenToWorldPoint(_drawPos.rect.position));
            Debug.Log("CamAnchrPos: " + Camera.main.ScreenToWorldPoint(_drawPos.anchoredPosition));
        }
        
        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots,Action onComplete = null)
        {
            for (int i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].CardUI.Init();
                cardSlots[i].CardUI.CardLocoMotionUI.Transition(cardSlots[i].CardPos,_drawTransition);
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
        
        private Vector2 CalculateCardPosition(int index)
        {
            Vector2 startPos = _middleHandPos.GetLocalPosition();

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