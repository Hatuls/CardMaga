using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionDataManager : ISequenceOperation<MetaDataManager>
    {
        [SerializeField] private UnityEvent OnSuccessUpdateDeck;
        [SerializeField] private UnityEvent OnFailedUpdateDeck;
        
        [SerializeField] private MetaDeckUICollectionManager _deckUICollectionManager;
        [SerializeField] private InputFieldHandler _deckName;
        [SerializeField] private MetaCollectionUIManager metaCollectionUIManager;
        private DeckBuilder _deckBuilder;
        private AccountDataAccess _accountDataAccess;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _deckBuilder = data.DeckBuilder;
            
            _deckBuilder.AssingDeckToEdit(_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck);
            _deckName.SetText(_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.DeckName);//all plaster
            
            
            _deckName.OnValueChange += EditDeckName;
            _deckBuilder.OnSuccessCardAdd += metaCollectionUIManager.AddCardUI;
            _deckBuilder.OnSuccessCardRemove += metaCollectionUIManager.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd += metaCollectionUIManager.AddComboUI;
            _deckBuilder.OnSuccessComboRemove += metaCollectionUIManager.RemoveComboUI;
        }

        public int Priority => 1;

        private void EditDeckName(string name)
        {
            if (!_deckBuilder.TryEditDeckName(name))
                Debug.Log("Failed to update deck name");
            
            Debug.Log("Deck new name " + name);
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
            _deckBuilder.OnSuccessCardAdd -= metaCollectionUIManager.AddCardUI;
            _deckBuilder.OnSuccessCardRemove -= metaCollectionUIManager.RemoveCardUI;
            _deckBuilder.OnSuccessComboAdd -= metaCollectionUIManager.AddComboUI;
            _deckBuilder.OnSuccessComboRemove -= metaCollectionUIManager.RemoveComboUI;
        }
    }
}

