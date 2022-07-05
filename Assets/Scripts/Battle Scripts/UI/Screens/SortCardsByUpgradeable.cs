using Cards;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.UI
{
    public class SortCardsByUpgradeable : CardSort
    {
        public override IEnumerable<Card> Sort()
        {
            IReadOnlyCollection<Card> cards = GetCollection();
            IEnumerable<Card> sortedDeck = cards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel - 1);
            return sortedDeck;
        }

    }
}