using Cards;
using System;
using System.Collections.Generic;

namespace Battles.Deck
{
    public class PlayerDeck : DeckAbst
    {
        BuffIcon _deckIcon;
        public PlayerDeck(bool isPlayer, Card[] deckCards, BuffIcon DeckIcon) : base(isPlayer, deckCards)
        {
            _deckIcon = DeckIcon;
        }
        public override void AddCard(Card card)
        {
            base.AddCard(card);

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);
          
        }


        public override Card GetFirstCard()
        {
            var card = base.GetFirstCard();
            if (card ==  null)
            {
                OrderDeck();
                card = base.GetFirstCard();
            }
            return card;
        }
        public override void ResetDeck()
        {
            if (isPlayer)
                SetDeck = Managers.PlayerManager.Instance.Deck;
            else
                SetDeck = EnemyManager.Instance.Deck;

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);


            Shuffle();
            OrderDeck();

            CountCards();
        }
        public override void DiscardCard(in Card card)
        {
            base.DiscardCard(card);

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public void Shuffle()
        {
            if(GetDeck == null)
                return;

            int deckLength = GetDeck.Length;

            if (deckLength == 0)
                return;

            Card[] tempArray = new Card[deckLength];
            List<Card> tempList = new List<Card>(deckLength);
            for (int i = 0; i < deckLength; i++)
            {
                tempList.Add(GetDeck[i]);
            }
            Random random = new Random();
            for (int i = 0; i < deckLength; i++)
            {
                int indexNum = random.Next(0, tempList.Count);
                tempArray[i] = tempList[indexNum];
                tempList.RemoveAt(indexNum);
            }
            for (int i = 0; i < deckLength; i++)
            {
                GetDeck[i] = tempArray[i];
            }
        }
    }
}