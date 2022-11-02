using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI
{
    public class SortByCharacterDeckRemaining : CardSort
    {
        [SerializeField]
        byte deckIndex;
        // need to be redone
        public override IEnumerable<CardMaga.Card.CardData> Sort()
        {

            //var deck = account.AccountCharacters.GetCharacterData(account.AccountCharacters.SelectedCharacter).Decks[deckIndex].Cards;
            //var sortedDeck = account.AccountCards.CardList.Where(c => deck.All((c2) => !c2.Equals(c)));
            //var result = Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(sortedDeck.ToArray());
            //return result;
            throw new System.NotImplementedException();
        }

    }
}