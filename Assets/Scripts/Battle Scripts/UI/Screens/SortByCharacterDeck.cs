using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Map.UI
{
    public class SortByCharacterDeck : SortAbst<Card>
    {
        [SerializeField]
        int deckIndex;
        [SerializeField]
        SortEvent _event;
        public override IEnumerable<Card> Sort()
        {
            var account = Account.AccountManager.Instance;
            var deck = account.AccountCharacters.GetCharacterData(CharacterEnum.Chiara).Decks[deckIndex].Cards;
            var result = Factory.GameFactory.Instance.CardFactoryHandler.CreateDeck(deck.ToArray());
            return result;

        }

        public override void SortRequest()
        {
            _event?.Invoke(this);
        }
    }
}