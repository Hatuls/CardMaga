using Cards;

namespace Battles.Deck
{
    public class PlayerDeck : DeckAbst
    {
        BuffIcon _deckIcon;
        public PlayerDeck(Card[] deckCards, BuffIcon DeckIcon) : base(deckCards)
        {
            _deckIcon = DeckIcon;
        }

        public PlayerDeck(int length) : base(length)
        {
        }
        public override void AddCard(Card card)
        {
            base.AddCard(card);
            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public override void ResetDeck()
        {
            SetDeck = DeckManager.Instance.GetSetDeck;
            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
        public override void DiscardCard(in Card card)
        {
            base.DiscardCard(card);
            _deckIcon?.SetAmount(GetAmountOfFilledSlots);
        }
    }
}