﻿using Cards;
using Rei.Utilities;
using System.Collections.Generic;

using UnityEngine;
namespace CardMaga.UI
{
    public class SortByCharacterDeck : SortAbst<Card>
    {
        [SerializeField]
        int deckIndex;
        // Need To be Re-Done
        public override IEnumerable<Card> Sort()
        {
            // var account = Account.AccountManager.Instance.AccountCharacters;
            // var currentDeck = account.GetCharacterData(account.SelectedCharacter).GetDeckAt(deckIndex);
            //int i = currentDeck.Cards.Length;
            // return Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(currentDeck);
            return null;
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}