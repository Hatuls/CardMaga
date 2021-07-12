using Cards;

namespace Battles.Deck
{
    public class Disposal : DeckAbst
    {
        PlayerDeck _playerDeck;
        BuffIcon _disposalIcon;
        public Disposal(int length, PlayerDeck deck, BuffIcon icon) : base(length)
        {
            _playerDeck = deck;
            _disposalIcon = icon;
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
        public override void AddCard(Card card)
        {
            base.AddCard(card);
            _disposalIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public override void DiscardCard(in Card card)
        {
            base.DiscardCard(card);
            _disposalIcon?.SetAmount(GetAmountOfFilledSlots);
        }
    }
}