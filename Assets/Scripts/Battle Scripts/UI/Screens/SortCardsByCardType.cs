using Battles;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Map.UI
{
    public class SortCardsByCardType : SortAbst<Card>
    {
        [SerializeField]
        CardUIFilterScreen _filer;
        public void SortRequested()
            => _filer.SortBy(this);

        [SerializeField]
        CardTypeEnum cardTypeEnum;
        public override IEnumerable<Card> Sort()
        {
            var deck = BattleData.Player.CharacterData.CharacterDeck;
            return deck.Where((x) => x.CardSO.CardTypeEnum == cardTypeEnum);
        }
    }
}