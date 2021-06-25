namespace Battles.Deck
{
    public class Disposal : DeckAbst
    {
        PlayerDeck _playerDeck;
        public Disposal(int length, PlayerDeck deck) : base(length)
        {
            _playerDeck = deck;
        }

        public override void ResetDeck()
        {
            var disposalDeck = GetDeck;
            for (int i = disposalDeck.Length - 1; i >= 0; i--)
            {
                if (disposalDeck[i] != null)
                {
                    _playerDeck.AddCard(disposalDeck[i]);
                    DiscardCard(disposalDeck[i]);
                }
            }
        }
    }
}