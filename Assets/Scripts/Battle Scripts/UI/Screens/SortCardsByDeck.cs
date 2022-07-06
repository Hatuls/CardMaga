using Battle.Deck;
using Cards;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortCardsByDeck : CardSort
    {
        [SerializeField]
        DeckEnum _deck;
        public override IEnumerable<Card> Sort()
        {
            return DeckManager.Instance.GetCardsFromDeck(true, _deck);
        }
    }
}