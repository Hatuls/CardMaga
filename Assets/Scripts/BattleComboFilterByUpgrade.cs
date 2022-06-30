using Battle;
using UnityEngine;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using Battle.Combo;

public class BattleComboFilterByUpgrade : SortAbst<Combo>
{
    // Need To be Re-Done
    public override IEnumerable<Combo> Sort()
    {
        //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe;
        //var sortedDeck = deck.Where(x => x.Level < (x.ComboSO().CraftedCard.CardsMaxLevel - 1));
        //return sortedDeck;
        return null;
    }


    public override void SortRequest()
    {
        _comboEvent?.Invoke(this);
    }
}