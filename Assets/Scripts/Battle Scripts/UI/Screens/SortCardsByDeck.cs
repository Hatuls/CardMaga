﻿using Battles.Deck;
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;
namespace Map.UI
{
    public class SortCardsByDeck : SortAbst<Card>
    {
      
        [SerializeField]
        DeckEnum _deck;
        public override IEnumerable<Card> Sort()
        {
            return DeckManager.Instance.GetCardsFromDeck(true, _deck); 
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}