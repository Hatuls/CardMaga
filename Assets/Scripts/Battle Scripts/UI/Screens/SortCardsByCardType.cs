
using Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.UI
{
    public class SortCardsByCardType :CardSort
    {

        [SerializeField]
        CardTypeEnum cardTypeEnum;
        // Need To be Re-Done
        public override IEnumerable<Card> Sort()
        {
            var deck = GetCollection();
            return deck.Where((x) => x.CardSO.CardTypeEnum == cardTypeEnum);
           
        }

    }
}