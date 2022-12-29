using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class DeckEditingDataManager : ISequenceOperation<MetaDataManager>, IDisposable
    {
        public event Action OnSuccessUpdateDeck;
        public event Action OnFailedUpdateDeck;
        
        private DeckBuilder _deckBuilder;
        private AccountDataAccess _accountDataAccess;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _deckBuilder = data.DeckBuilder;
        }

        public int Priority => 2;

        public void AssingDeckDataToEdit()
        {
            _deckBuilder.AssingDeckToEdit(_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck);
        }

        public bool ExitDeckEditing()
        {
            if (_deckBuilder.TryApplyDeck(out MetaDeckData metaDeckData))
            {
                TokenMachine tokenMachine = new TokenMachine(SuccessUpdateDeck);
                _accountDataAccess.UpdateDeck(metaDeckData,tokenMachine);
                DiscardDeck();
                return true;
            }
            
            OnFailedUpdateDeck?.Invoke();
            return false;
        }

        private void SuccessUpdateDeck()
        {
            OnSuccessUpdateDeck?.Invoke();
        }

        public void DiscardDeck()
        {
            _deckBuilder.DisposeDeck();
        }

        public void Dispose()
        {
        }
    }
}

