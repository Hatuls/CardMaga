namespace Battle.Deck
{
    public class Exhaust : BaseDeck {
        public Exhaust( int length) : base(length)
        {
        }
        public override void ResetDeck()
        {
            EmptySlots();
        }
    }
}