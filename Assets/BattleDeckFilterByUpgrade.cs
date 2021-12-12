using Battles;
using UnityEngine;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using Cards;

public class BattleDeckFilterByUpgrade : SortAbst<Card>
{
    [SerializeField]
    public override IEnumerable<Card> Sort()
    {
        var deck = BattleData.Player.CharacterData.CharacterDeck;
        var sortedDeck =deck.Where(x => x.CardLevel < (x.CardSO.CardsMaxLevel - 1)); 
        return sortedDeck;
    }


    public override void SortRequest()
    {
        _cardEvent?.Invoke(this);
    }
}
