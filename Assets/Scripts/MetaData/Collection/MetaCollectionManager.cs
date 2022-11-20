using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionManager : MonoBehaviour
    {
        [SerializeField] private AccountDataAccess _accountDataAccess;
        [SerializeField] private MetaUICollectionManager _metaUICollectionManager;
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private DeckBuilder _deckBuilder;

        private void Start()
        {
            _accountDataCollectionHelper = new AccountDataCollectionHelper(_accountDataAccess);
            _metaUICollectionManager.Init(_accountDataCollectionHelper,_accountDataAccess.AccountData);
            _deckBuilder = new DeckBuilder(_accountDataCollectionHelper);
            _deckBuilder.AssingDeckToEdit(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0]);

            _deckBuilder.OnSuccessCardAdd += _metaUICollectionManager.AddCardUI;
            _deckBuilder.OnSuccessCardRemove += _metaUICollectionManager.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += _metaUICollectionManager.AddComboUI;
            _deckBuilder.OnSuccessComboRemove += _metaUICollectionManager.RemoveComboUI;
        }
    }
}

