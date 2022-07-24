namespace Battle.Deck
{
    public class Exhaust : BaseDeck {
        public Exhaust(bool isPlayer, int length) : base(isPlayer,length)
        {
        }
        public override void ResetDeck()
        {
            EmptySlots();
        }
    }
}