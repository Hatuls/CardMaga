using Battle.Combo;
using CardMaga.UI;
using System.Collections.Generic;
using System.Linq;

public class BattleComboFilterByUpgrade : ComboSort
{
    public override IEnumerable<BattleComboData> Sort()
    {
        var deck = GetCollection();
        var sortedDeck = deck.Where(x => x.Level < (x.ComboSO.CraftedCard.CardsMaxLevel - 1));
        return sortedDeck;
    }
}