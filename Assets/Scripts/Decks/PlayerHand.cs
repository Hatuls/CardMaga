﻿using Cards;

namespace Battles.Deck
{
    public class PlayerHand : DeckAbst
    {
        Disposal _disposalDeck;

        public PlayerHand(bool isPlayer,int length , Disposal deck) : base(isPlayer,length)
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
        public override Card GetFirstCard()
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
        public override void AddCard(Card card)
        {
            var deck = GetDeck;
            if (deck == null || deck.Length == 0)
                InitDeck(4);
            for (int i = 0; i < deck.Length; i++)
            {
                if (deck[i] == null)
                {
                    GetDeck[i]= card;
                    break;
                }
            }
                CountCards();
            
        }
        public override void DiscardCard(in Card card)
        {
            if (GetDeck != null && card != null && GetDeck.Length > 0)
            {
                for (int i = 0; i < GetDeck.Length; i++)
                {
                    if (GetDeck[i] != null
                       && GetDeck[i].CardID == card.CardID)
                    {
                        GetDeck[i] = null;
                        CountCards();
                        break;
                    }
                }
            }
        }
    }
}