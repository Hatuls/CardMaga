using Battle.UI.CardUIAttributes;
using UnityEngine;
namespace Battle.UI
{

    public class HandUI
    {

        public CardUI CurrentlyHolding { get; private set; }
        CardUISO _cardUISO;
        CardUI[] _handCards;

        Vector2 _middleHandPos;
        public static HandUI Instance;

        public HandUI(CardUI[] handCards, Vector2 middlePos, CardUISO cardUISO)
        {
            Instance = this;
            _handCards = handCards;
            _middleHandPos = middlePos;
            _cardUISO = cardUISO;
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
                SetHandCardPosition(GetHandCardUIFromIndex(i), i);
                var handstate = _handCards[i].CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand);
                handstate.Index = i;
                handstate.HasValue = false;
                _handCards[i].CardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hand);
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
            return card.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand).Index;
        }
        private void SetHandCardPosition(CardUI cardUI, int index)
        {
            Vector2 startPos = _middleHandPos;
            float spaceBetweenCard = _cardUISO.GetSpaceBetweenCards;

            Vector2 destination = startPos + Vector2.right * index * spaceBetweenCard;

            float xMaxDistance = _handCards.Length * spaceBetweenCard / 2;
            float offset = spaceBetweenCard / 2;
            destination += Vector2.left * (xMaxDistance - offset);

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
                if (_handCards[i] != null && _handCards[i].Inputs != null && _handCards[i].Inputs.GetCanvasGroup != null)
                {

                    _handCards[i].Inputs.GetCanvasGroup.blocksRaycasts = !toLock;
                }
                var stateMachine = _handCards[i]?.CardStateMachine;
                var gotoState = toLock ? CardStateMachine.CardUIInput.Locked : CardStateMachine.CardUIInput.Hand;
                stateMachine.MoveToState(gotoState);
            }
        }
        internal void ReplaceCard(CardUI ReplacingCard, CardUI ReplacedCard)
        {
            if (ReplacingCard != null && ReplacedCard != null)
            {
                CurrentlyHolding = ReplacingCard;
                CardUIManager.Instance.AssignDataToCardUI(ReplacingCard, ReplacedCard.GFX.GetCardReference);

                var index = ReplacedCard.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand).Index;
                var replacinghandSTate = ReplacingCard.CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand);
                replacinghandSTate.Index = index;
                replacinghandSTate.HasValue = false;


                int childIndex = ReplacingCard.transform.GetSiblingIndex();
                int replacedIndex = ReplacedCard.transform.GetSiblingIndex();
                ReplacingCard.transform.SetSiblingIndex(replacedIndex);
                ReplacedCard.transform.SetSiblingIndex(childIndex);

                SetHandCardPosition(ReplacingCard, replacedIndex);
                _handCards[index] = ReplacingCard;
            }
        }

        internal void DiscardHand()
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                _handCards[i].CardStateMachine.GetState<HandState>(CardStateMachine.CardUIInput.Hand).HasValue = false;
                _handCards[i].gameObject.SetActive(false);
            }
        }
    }



}