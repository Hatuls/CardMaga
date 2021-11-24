using Battles;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Map.UI
{
    public class ShowAllCardsInMenuScreen : SortAbst<Card>
    {
        [SerializeField]
        CardUIFilterScreen _filer;

        public void SortRequest() => _filer.SortBy(this);
        public override IEnumerable<Card> Sort()
        {
          return BattleData.Player.CharacterData.CharacterDeck;
        }
    }
}