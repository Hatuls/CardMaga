using Cards;
using UnityEngine;
namespace Battles.Deck
{
    public class Placements : DeckAbst
    {
        Disposal _disposalDeck;
        PlayerHand _playerHandDeck;
        public Placements(int length, Disposal deck, PlayerHand hand) : base(length)
        {
            _disposalDeck = deck;
            _playerHandDeck = hand;
        }

        public override void ResetDeck()
        {
            /*
             * run on cards each card each that is not null move it to disposal deck
             */
            if (_disposalDeck == null || GetDeck == null)
            {
                Debug.LogError("Placements: Deck was not found!");
                return;
            }

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
        public void DiscardCard(in Card card, int index)
        {
            if (index < 0 || index >= GetDeck.Length || card == null)
                return;

            GetDeck[index] = null;

            CountCards();
            Relics.RelicManager.Instance.DetectRelics();


        }
        public void AddCard(Card card, int index)
        {
            if (card == null)
                return;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(4);


            if (index >= 0 && index < GetDeck.Length)
            {
                if (GetDeck[index] != null)
                    _playerHandDeck.AddCard(GetDeck[index]);

                GetDeck[index] = card;
            }

            CountCards();
            Relics.RelicManager.Instance.DetectRelics();

        }
    }
}