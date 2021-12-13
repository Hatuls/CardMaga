using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Map.UI
{
    public class SortByCharacterDeckRemaining : SortAbst<Card>
    {
        [SerializeField]
        byte deckIndex;
    
        public override IEnumerable<Card> Sort()
        {
            var account = Account.AccountManager.Instance;
            var deck = account.AccountCharacters.GetCharacterData(account.AccountCharacters.SelectedCharacter).Decks[deckIndex].Cards;

            var sortedDeck = account.AccountCards.CardList.Where(c => deck.All((c2) => !c2.Equals(c)));
            var result = Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(sortedDeck.ToArray());
            return result;
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}