using System;
using UnityEngine;
namespace Battles.UI
{

    public class HandUI
    {
        CardUISO _cardUISO;
        CardUI[] _handCards;
        Vector2 _middleHandPos;
        public static HandUI Instance;

        public HandUI(CardUI[] handCards, Vector2 middlePos,  CardUISO cardUISO)
        {
            Instance = this;
           _handCards = handCards;
            _middleHandPos = middlePos;
            _cardUISO = cardUISO;
            AlignCards();

        }

        public void AlignCards()
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                SetHandCardPosition(GetHandCardUIFromIndex(i), i);
               _handCards[i].Inputs.InHandInputState.Index = i;
               _handCards[i].Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Hand;
                _handCards[i].Inputs.InHandInputState.HasValue = false;
                _handCards[i].gameObject.SetActive(false);
            }
          
        }
        public CardUI GetHandCardUIFromIndex(int index)
        {
            if (index >= 0 && index < _handCards.Length)
                return _handCards[index];
            return null;
        }
        public int GetCardIndex(CardUI card)
        {
            if (card == null)
                return -1;
            return card.Inputs.InHandInputState.Index;
        }
        private void SetHandCardPosition(CardUI cardUI, int index)
        {
            Vector2 startPos = _middleHandPos;
            float spaceBetweenCard = _cardUISO.GetSpaceBetweenCards;

            Vector2 destination = startPos + Vector2.right * index * spaceBetweenCard;

            float xMaxDistance = _handCards.Length * spaceBetweenCard / 2;
            float offset = spaceBetweenCard / 2;
            destination += Vector2.left * (xMaxDistance- offset);

            cardUI.CardTranslations.SetPosition(destination);
        }

        /// <summary>
        /// Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        public void LockCardsInput(bool toLock)
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (_handCards[i]?.Inputs?.CurrentState != CardUIAttributes.CardInputs.CardUIInput.Zoomed 
                    &&_handCards[i]?.Inputs?.CurrentState != CardUIAttributes.CardInputs.CardUIInput.Hold)
                {
                _handCards[i].Inputs.CurrentState = toLock ? CardUIAttributes.CardInputs.CardUIInput.Locked : CardUIAttributes.CardInputs.CardUIInput.Hand;
                _handCards[i].Inputs.GetCanvasGroup.blocksRaycasts = !toLock;
                }
            }
        }

        internal void DiscardHand()
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                _handCards[i].Inputs.InHandInputState.HasValue = false;
                _handCards[i].gameObject.SetActive(false);
            }
        }
    }



}