using Cards;

namespace Battles.Deck
{
    public class PlayerDeck : DeckAbst
    {
        public PlayerDeck(Card[] deckCards) : base(deckCards)
        {
        }

        public PlayerDeck(int length) : base(length)
        {
        }

        public override void ResetDeck()
        {
            SetDeck = DeckManager.Instance.GetSetDeck;
        }
    }
}