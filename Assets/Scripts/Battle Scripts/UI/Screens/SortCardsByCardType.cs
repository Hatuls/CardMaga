using Battle;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.UI
{
    public class SortCardsByCardType : SortAbst<Card>
    {

        [SerializeField]
        CardTypeEnum cardTypeEnum;
        // Need To be Re-Done
        public override IEnumerable<Card> Sort()
        {
            //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
            //return deck.Where((x) => x.CardSO.CardTypeEnum == cardTypeEnum);
            return null;
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}