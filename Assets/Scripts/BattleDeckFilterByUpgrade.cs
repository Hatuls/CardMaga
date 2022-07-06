using Battle;
using UnityEngine;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using Cards;
using CardMaga.UI;

public class BattleDeckFilterByUpgrade : CardSort
{    // Need To be Re-Done
    public override IEnumerable<Card> Sort()
    {
        var deck = GetCollection();
        var sortedDeck = deck.Where(x => x.CardLevel < (x.CardSO.CardsMaxLevel - 1));
        return sortedDeck;
  
    }


}
