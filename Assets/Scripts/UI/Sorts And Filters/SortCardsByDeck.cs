using Battle.Deck;
using CardMaga.Battle;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortCardsByDeck : CardSort
    {

        [SerializeField]
        DeckEnum _deck;
        public override IEnumerable<CardMaga.Card.BattleCardData> Sort()
        {
            return BattleManager.Instance.PlayersManager.GetCharacter(true).DeckHandler.GetCardsFromDeck(_deck);
        }
    }
}