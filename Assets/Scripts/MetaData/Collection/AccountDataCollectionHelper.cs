using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class AccountDataCollectionHelper : ISequenceOperation<MetaDataManager>
    {
        private AccountDataAccess _accountDataAccess;

        [SerializeField] private CardsCollectionDataHandler _collectionCardDatasHandler;
        [SerializeField] private ComboCollectionDataHandler _collectionComboDatasHandler;

        public CardsCollectionDataHandler CollectionCardDatasHandler => _collectionCardDatasHandler;

        public ComboCollectionDataHandler CollectionComboDatasHandler => _collectionComboDatasHandler;

        public List<MetaCollectionCardData> AllCollectionCardDatas => _collectionCardDatasHandler.CollectionCardDatas;
        public List<MetaCollectionComboData> AllCollectionComboDatas => _collectionComboDatasHandler.CollectionComboDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;

            _collectionCardDatasHandler = new CardsCollectionDataHandler(_accountDataAccess.AccountData);
            _collectionComboDatasHandler = new ComboCollectionDataHandler(_accountDataAccess.AccountData);
        }

        public CardsCollectionDataHandler GetCardCollectionByDeck(int deckId)
        {
            bool Condition(MetaCardInstanceInfo metaCardInstanceInfo)
            {
                return metaCardInstanceInfo.IsInDeck(deckId);
            }
            
            var instanceIDs = new List<int>();

            foreach (var cardData in _collectionCardDatasHandler.CollectionCardDatas)  
            {
                if (cardData.TryGetMetaCardInstanceInfo(Condition,out MetaCardInstanceInfo[] metaCardInstanceInfos))
                    instanceIDs.AddRange(metaCardInstanceInfos.Select(instanceInfo => instanceInfo.InstanceID));
            }
            
            var output = new CardsCollectionDataHandler(_collectionCardDatasHandler.GetCollectionCopy());
            
            foreach (var cardData in output.CollectionCardDatas)
            {
                foreach (var instanceID in instanceIDs)
                {
                    cardData.RemoveCardInstance(instanceID);
                }
            }

            return output;
        }
        
        public ComboCollectionDataHandler GetComboCollectionByDeck(int deckId)
        {
            bool Condition(MetaComboInstanceInfo metaComboInstanceInfo)
            {
                return metaComboInstanceInfo.IsInDeck(deckId);
            }
            
            var comboCoreIDs = new List<int>();

            foreach (var collectionComboData in _collectionComboDatasHandler.CollectionComboDatas) 
            {
                if (collectionComboData.TryGetMetaComboInstanceInfo(Condition,out MetaComboInstanceInfo[] metaComboInstanceInfos))
                {
                    comboCoreIDs.AddRange(metaComboInstanceInfos.Select(instanceInfo => instanceInfo.InstanceID));
                }   
            }

            var output = new ComboCollectionDataHandler(_collectionComboDatasHandler.GetCollectionCopy());
            
            foreach (var cardData in output.CollectionComboDatas)
            {
                foreach (var instanceID in comboCoreIDs)
                {
                    cardData.RemoveComboInstance(instanceID);
                }
            }

            return output;
        }

        public CardsCollectionDataHandler GetAllUnAssingeCard()
        {
            CardsCollectionDataHandler output = new CardsCollectionDataHandler();
            
            bool Condition(MetaCardInstanceInfo metaCardInstanceInfo)
            {
                return !metaCardInstanceInfo.InDeck; 
            }

            foreach (var metaCollectionCardData in _collectionCardDatasHandler.CollectionCardDatas)
            {
                if (metaCollectionCardData.TryGetMetaCardInstanceInfo(Condition, out MetaCardInstanceInfo[] metaCardInstanceInfos))
                {
                    output.AddCardInstance(metaCardInstanceInfos);
                }
            }

            return output;
        }
    }
}