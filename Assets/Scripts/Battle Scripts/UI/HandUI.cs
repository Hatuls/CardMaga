using System;
using System.Collections.Generic;
using Battles.Deck;
using Battles.UI.CardUIAttributes;
using Cards;
using UnityEngine;
namespace Battles.UI
{

    public class HandUI : MonoBehaviour
    {
        [SerializeField] private SelectCardUI _selectCard;
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private float _spaceBetweenCard;

        private List<CardUI> _handCards;

        private void Awake()
        {
            _handCards = new List<CardUI>();
            BattleManager.OnGameEnded += LockCards;
            DeckManager.OnDrawCards += DrawCards;
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
               SetHandCardPosition(_handCards[i],i);
               _handCards[i].GFX.SetActive(true);
            }
        }

        public void DrawCards(params Card[] cards)
        {
            _handCards.AddRange(_cardUIManager.GetCardsUI(cards));
            AlignCards();
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
        
        private void SetHandCardPosition(CardUI cardUI, int index)
        {
            Vector2 startPos = _middleHandPos.position;

            Vector2 destination = startPos + Vector2.right * index * _spaceBetweenCard;

            float xMaxDistance = _handCards.Count * _spaceBetweenCard / 2;
            float offset = _spaceBetweenCard / 2;
            destination += Vector2.left * (xMaxDistance - offset);

            cardUI.CardLocoMotionUI.Transition(destination);
        }
    }



}