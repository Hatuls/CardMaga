﻿using CardMaga.Card;

namespace Battle.Deck
{
    public class PlayerHand : BaseDeck
    {
        Discard _disposalDeck;

        public PlayerHand(int length , Discard deck) : base(length)
        {
            _disposalDeck = deck;
        }

        public override void ResetDeck()
        {
            var deck = GetDeck;
            for (int i = deck.Length - 1; i >= 0; i--)
            {
                if (deck[i] != null)
                {
                    _disposalDeck.AddCard(deck[i]);
                    DiscardCard(deck[i]);
                }
            }
            CountCards();
        }
        public override CardData GetFirstCard()
        {
            var deck = GetDeck;
            if (deck != null && deck.Length > 0)
            {
                for (int i = 0; i < deck.Length; i++)
                {
                    if (deck[i] != null)
                        return deck[i];
                }
            }
            return null;
        }
        public override bool AddCard(CardData card)
        {
            bool added = false;
            var deck = GetDeck;
            if (deck == null || deck.Length == 0)
                InitDeck(4);
            for (int i = 0; i < deck.Length; i++)
            {
                if (deck[i] == null)
                {
                    GetDeck[i]= card;
                    added = true;
                    break;
                }
            }
                CountCards();
            return added;
        }
        public override bool DiscardCard(in CardData card)
        {
            bool found = false;
            if (GetDeck != null && card != null && GetDeck.Length > 0)
            {
                for (int i = 0; i < GetDeck.Length; i++)
                {
                    if (GetDeck[i] != null
                       && GetDeck[i].CardInstanceID == card.CardInstanceID)
                    {
                        found = true;
                        GetDeck[i] = null;
                        CountCards();
                        break;
                    }
                }
            }
            return found;
        }
    }
}