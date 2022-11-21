﻿using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaUI.CollectionUI;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnSuccessUpdateDeck;
        [SerializeField] private UnityEvent OnFailedUpdateDeck;

        [SerializeField] private InputFieldHandler _deckName;
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

            _deckName.OnValueChange += EditDeckName;
            _deckBuilder.OnSuccessCardAdd += _metaUICollectionManager.AddCardUI;
            _deckBuilder.OnSuccessCardRemove += _metaUICollectionManager.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += _metaUICollectionManager.AddComboUI;
            _deckBuilder.OnSuccessComboRemove += _metaUICollectionManager.RemoveComboUI;
        }

        private void EditDeckName(string name)
        {
            if (_deckBuilder.TryEditDeckName(name))
            {
                Debug.Log("Deck new name " + name);
            }

            Debug.Log("Failed to update deck name");
        }

        public void ExitDeckEditing()
        {
            if (_deckBuilder.TryApplyDeck(out MetaDeckData metaDeckData))
            {
                TokenMachine tokenMachine = new TokenMachine(SuccessUpdateDeck);
                _accountDataAccess.UpdateDeck(metaDeckData,tokenMachine);
            }
            else
                OnFailedUpdateDeck?.Invoke();
        }

        private void SuccessUpdateDeck()
        {
            OnSuccessUpdateDeck?.Invoke();
        }

        private void OnDestroy()
        {
            _deckName.OnValueChange -= EditDeckName;
            _deckBuilder.OnSuccessCardAdd -= _metaUICollectionManager.AddCardUI;
            _deckBuilder.OnSuccessCardRemove -= _metaUICollectionManager.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd -= _metaUICollectionManager.AddComboUI;
            _deckBuilder.OnSuccessComboRemove -= _metaUICollectionManager.RemoveComboUI;
        }
    }
}

