using Cards;
using UnityEngine;
namespace Battles.Deck
{
    public class Selected : BaseDeck
    {
        Disposal _disposalDeck;
        PlayerHand _playerHandDeck;


        public static DeckEnum _discardTo;
        public Selected(bool isPlayer, int length, Disposal deck, PlayerHand hand) : base(isPlayer,length)
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
                DeckManager.Instance.AddCardToDeck(
                    isPlayer,
                    deck[0],
                    deck[0].IsExhausted ? DeckEnum.Exhaust : DeckEnum.Disposal
                    );

                DiscardCard(deck[0]);
            }
        }
      
        public bool DiscardCard(in Card card, DeckEnum? discardTo = null)
        {

            if (card == null)
                return false;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(DeckManager._placementSize);
  
            if (GetDeck[0] == null)
                return true;

            DeckEnum destination = (discardTo == null) ?
                ( (card.IsExhausted) ? DeckEnum.Exhaust : DeckEnum.Disposal)
                : discardTo.Value;

            DeckManager.Instance.TransferCard(
                isPlayer,
                DeckEnum.Selected,
                destination,
                card);

            GetDeck[0] = null;

            return true;
        }
        public override bool AddCard(Card card)
        {
            if (card == null)
                return false;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(DeckManager._placementSize);


            if (GetDeck[0] == null)
            {
                _discardTo = DeckEnum.Hand;
                GetDeck[0] = card;
            }

           // DiscardCard(GetDeck[0], DeckEnum.Hand);
            //AddCard(card);
         return true;
        }
     
    }
}