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
        }

        private void InitCollectionDeck()
        {
            List<MetaCollectionCardData> _collectionCard = _accountDataCollectionHelper.CollectionCardDatas;
            
            for (int i = 0; i < _collectionCard.Count; i++)
            {
                _collectionCard[i].OnTryAddCard += _deckBuilder.TryAddCard;
                _collectionCard[i].OnTryRemoveCard += _deckBuilder.TryRemoveCard;
                _deckBuilder.OnSuccessCardAdd += _collectionCard[i].AddCardToDeck;
                _deckBuilder.OnSuccessCardRemove += _collectionCard[i].RemoveCardFromDeck;
            }
        }
    }
}

