using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Map.UI
{
    public class SortCardsByUpgradeable : SortAbst<Card>
    {
            
        public override IEnumerable<Card> Sort()
        {
            var accountCards = Account.AccountManager.Instance.AccountCards.CardList;

            var cards = Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(accountCards.ToArray());

            var sortedDeck = cards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel-1);

            return sortedDeck;
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}