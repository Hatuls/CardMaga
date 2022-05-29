using Battles.UI.CardUIAttributes;
using UnityEngine;
namespace Battles.UI
{

    public class HandUI
    {

        public CardUI CurrentlyHolding { get; private set; }
        CardUI[] _handCards;

        Vector2 _middleHandPos;
        public static HandUI Instance;

        public HandUI(CardUI[] handCards, Vector2 middlePos)
        {
            Instance = this;
            _handCards = handCards;
            _middleHandPos = middlePos;
            AlignCards();
            CurrentlyHolding = null;
            BattleManager.OnGameEnded += LockCards;
        }
        ~HandUI()
            => BattleManager.OnGameEnded -= LockCards;
        private void LockCards() => LockCardsInput(true);
        public void AlignCards()
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
               
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
            return 1; //need work
        }
        private void SetHandCardPosition(CardUI cardUI, int index)
        {
            
        }

        /// <summary>
        /// Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        public void LockCardsInput(bool toLock)
        {
            
        }
        internal void ReplaceCard(CardUI ReplacingCard, CardUI ReplacedCard)
        {
            
        }

        internal void DiscardHand()
        {
            
        }
    }



}