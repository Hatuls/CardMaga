using Cards;
using UnityEngine;
namespace Battles.Deck
{
    public class Selected : DeckAbst
    {
        Disposal _disposalDeck;
        PlayerHand _playerHandDeck;


        public static DeckEnum _discardTo;
        public Selected(int length, Disposal deck, PlayerHand hand) : base(length)
        {
            _disposalDeck = deck;
            _playerHandDeck = hand;
        }

        public override void ResetDeck()
        {
            if (_disposalDeck == null || GetDeck == null)
            {
                Debug.LogError("Selected: Deck was not found!");
                return;
            }

            var deck = GetDeck;

            if (deck[0] != null)
            {
                _disposalDeck.AddCard(deck[0]);
                DiscardCard(deck[0]);
            }
        }
      
        public void DiscardCard(in Card card, DeckEnum? discardTo = null)
        {
            if (card == null)
                return;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(DeckManager._placementSize);

            if (GetDeck[0] == null)
                return;

            DeckManager.Instance.TransferCard(
                DeckEnum.Selected,
                discardTo == null ? _discardTo : discardTo.Value,
                card);

            GetDeck[0] = null;
        }
        public override void AddCard(Card card)
        {
            if (card == null)
                return;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(DeckManager._placementSize);


            if (GetDeck[0] == null)
            {
                _discardTo = DeckEnum.Hand;
                GetDeck[0] = card;
                return;
            }

            DiscardCard(GetDeck[0], DeckEnum.Hand);
            AddCard(card);

        }
     
    }
}