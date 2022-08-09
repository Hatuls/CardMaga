using Cards;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.UI
{
    public class SortCardsByUpgradeable : CardSort
    {
        public override IEnumerable<CardMaga.Card.CardData> Sort()
        {
            IReadOnlyCollection<CardMaga.Card.CardData> cards = GetCollection();
            IEnumerable<CardMaga.Card.CardData> sortedDeck = cards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel - 1);
            return sortedDeck;
        }

    }
}