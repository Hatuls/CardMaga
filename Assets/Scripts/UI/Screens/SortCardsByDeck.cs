﻿using Battle.Deck;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortCardsByDeck : CardSort
    {
        [SerializeField]
        DeckEnum _deck;
        public override IEnumerable<CardMaga.Card.CardData> Sort()
        {
            return DeckManager.Instance.GetCardsFromDeck(true, _deck);
        }
    }
}