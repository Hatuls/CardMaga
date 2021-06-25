namespace Battles.Deck
{
    public class PlayerHand : DeckAbst
    {
        Disposal _disposalDeck;

        public PlayerHand(int length , Disposal deck) : base(length)
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
        }

    }
}