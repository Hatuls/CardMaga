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
        bool toUseAccoundCards;
        [SerializeField]
        MetaCardUIFilterScreen filterHandler;
        
        public int? ID { get; set; }

        // need to be re done
        public override IEnumerable<Card> Sort()
        {
            //var accountCards = toUseAccoundCards ? Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(Account.AccountManager.Instance.AccountCards.CardList.ToArray()) :
            //    Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;

            //var sortedDeck = accountCards.Where(x => x.CardLevel < x.CardSO.CardsMaxLevel - 1);

            //if (ID == null)
            //    return sortedDeck;

            //return sortedDeck.Where(x => x.CardInstanceID != ID);
            return null;
        }

        public override void SortRequest()
        {
            filterHandler.SortBy(this);
        }
    }
}