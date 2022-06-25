using Battles;
using UnityEngine;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using Cards;

public class BattleDeckFilterByUpgrade : SortAbst<Card>
{    // Need To be Re-Done
    public override IEnumerable<Card> Sort()
    {
        //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
        //var sortedDeck =deck.Where(x => x.CardLevel < (x.CardSO.CardsMaxLevel - 1)); 
        //return sortedDeck;
        return null;
    }


    public override void SortRequest()
    {
        _cardEvent?.Invoke(this);
    }
}
