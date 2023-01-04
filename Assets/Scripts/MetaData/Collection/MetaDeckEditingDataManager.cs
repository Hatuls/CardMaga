using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaDeckEditingDataManager : ISequenceOperation<MetaDataManager>, IDisposable
    {
        public event Action OnSuccessUpdateDeck;
        public event Action OnFailedUpdateDeck;
        
        private DeckBuilder _deckBuilder;
        private AccountDataAccess _accountDataAccess;
        private MetaDeckData _metaDeckData;
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private List<MetaCollectionCardData> _metaCollectionCardDatas;

        public MetaDeckData MetaDeckData => _metaDeckData;

        private List<MetaCollectionComboData> _metaCollectionComboDatas;

        public List<MetaCollectionCardData> MetaCollectionCardDatas => _metaCollectionCardDatas;

        public List<MetaCollectionComboData> MetaCollectionComboDatas => _metaCollectionComboDatas;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _deckBuilder = data.DeckBuilder;
            _accountDataCollectionHelper = data.AccountDataCollectionHelper;
        }

        public int Priority => 2;

        public void AssingDeckDataToEdit()
        {
            _metaDeckData = _accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck;

            _metaCollectionCardDatas = _accountDataCollectionHelper.SetCardCollection(_metaDeckData.DeckId);
            _metaCollectionComboDatas = _accountDataCollectionHelper.SetComboCollection(_metaDeckData.DeckId);
            
            _deckBuilder.AssingDeckToEdit(_metaDeckData);
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

