using CardMaga.Meta.AccountMetaData;
using UnityEngine;

namespace CardMaga.MetaData.DeckBuilder
{
    public class MetaDeckEditingHandler : MonoBehaviour
    {
        [SerializeField] private AccountDataAccess _accountData;
        private DeckBuilder _deckBuilder;

        private void Start()
        {
            _deckBuilder = new DeckBuilder();
            _deckBuilder.AssingDeckToEdit(_accountData.AccountData.CharacterDatas.CharacterData.Decks[0]);
        }
    }
}