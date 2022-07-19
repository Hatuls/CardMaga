using Battle;
using UnityEngine;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using CardMaga.UI;
using CardMaga.Card;

public class BattleDeckFilterByUpgrade : CardSort
{    // Need To be Re-Done
    public override IEnumerable<CardData> Sort()
    {
        var deck = GetCollection();
        var sortedDeck = deck.Where(x => x.CardLevel < (x.CardSO.CardsMaxLevel - 1));
        return sortedDeck;
  
    }


}
