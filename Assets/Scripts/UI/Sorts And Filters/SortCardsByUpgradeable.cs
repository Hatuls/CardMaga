using Cards;
using System.Collections.Generic;
using System.Linq;
namespace CardMaga.UI
{
    public class SortCardsByUpgradeable : CardSort
    {
        public override IEnumerable<CardMaga.Card.BattleCardData> Sort()
        {
            IReadOnlyCollection<CardMaga.Card.BattleCardData> cards = GetCollection();
            IEnumerable<CardMaga.Card.BattleCardData> sortedDeck = cards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel - 1);
            return sortedDeck;
        }

    }
}