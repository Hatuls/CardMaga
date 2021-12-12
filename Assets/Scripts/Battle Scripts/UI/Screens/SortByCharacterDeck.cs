using Cards;
using Rei.Utilities;
using System.Collections.Generic;

using UnityEngine;
namespace Map.UI
{
    public class SortByCharacterDeck : SortAbst<Card>
    {
        [SerializeField]
        int deckIndex;

        public override IEnumerable<Card> Sort()
        {
            var account = Account.AccountManager.Instance.AccountCharacters;
            var currentDeck = account.GetCharacterData(account.SelectedCharacter).GetDeckAt(deckIndex);
            return Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(currentDeck);
        }

        public override void SortRequest()
        {
            _cardEvent?.Invoke(this);
        }
    }
}