using Battles.UI;
using Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rei.Utilities
{
    public class SortByNotSelected : SortAbst<Card>
    {
        [SerializeField]

        MetaCardUIFilterScreen filterHandler;

        public ushort? ID { get; set; }


        public override IEnumerable<Card> Sort()
        {
            var accountCards = Account.AccountManager.Instance.AccountCards.CardList;

            var cards = Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(accountCards.ToArray());

            var sortedDeck = cards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel - 1);

            if (ID == null)
                return sortedDeck;

            return sortedDeck.Where(x => x.CardInstanceID != ID);
        }

        public override void SortRequest()
        {
            filterHandler.SortBy(this);
        }
    }
}