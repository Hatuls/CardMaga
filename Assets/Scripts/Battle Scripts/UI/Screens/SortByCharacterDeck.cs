using Cards;
using Rei.Utilities;
using System.Collections.Generic;

using UnityEngine;
namespace CardMaga.UI
{
    public class SortByCharacterDeck : CardSort
    {
        [SerializeField]
        int deckIndex;
        // Need To be Re-Done
        public override IEnumerable<Card> Sort()
        {
            var deck = GetCollection();
            // var account = Account.AccountManager.Instance.AccountCharacters;
            // var currentDeck = account.GetCharacterData(account.SelectedCharacter).GetDeckAt(deckIndex);
            //int i = currentDeck.Cards.Length;
            // return Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(currentDeck);
            throw new System.NotImplementedException();
        }
    }
}