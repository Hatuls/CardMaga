using System;
using System.Collections.Generic;
using System.Linq;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class AccountDataCollectionHelper
    {
        private MetaAccountData _metaAccountData;

        [SerializeField] private CardsCollectionDataHandler _collectionCardDatasHandler;
        [SerializeField] private ComboCollectionDataHandler _collectionComboDatasHandler;

        public CardsCollectionDataHandler CollectionCardDatasHandler => _collectionCardDatasHandler;
        public ComboCollectionDataHandler CollectionComboDatasHandler => _collectionComboDatasHandler;


        public AccountDataCollectionHelper(MetaAccountData metaAccountData)
        {
            _metaAccountData = metaAccountData;

            _collectionCardDatasHandler = new CardsCollectionDataHandler(_metaAccountData);
            _collectionComboDatasHandler = new ComboCollectionDataHandler(_metaAccountData);
        }

        internal void UpdateCollection()
        {
            _collectionCardDatasHandler = new CardsCollectionDataHandler(_metaAccountData);
            _collectionComboDatasHandler = new ComboCollectionDataHandler(_metaAccountData);
        }

        public AccountCollectionCopyHelper GetCollectionCopy()
        {
            return new AccountCollectionCopyHelper(this);
        }

        public CardsCollectionDataHandler GetCardCollectionByDeck(int deckId) 
        {
            bool Condition(MetaCardInstanceInfo metaCardInstanceInfo)
            {
                return metaCardInstanceInfo.IsInDeck(deckId);
            }
            
            var instanceIDs = new List<int>();

            foreach (var cardData in _collectionCardDatasHandler.CollectionCardDatas.Values)  
            {
                if (cardData.TryGetMetaCardInstanceInfo(Condition,out MetaCardInstanceInfo[] metaCardInstanceInfos))
                    instanceIDs.AddRange(metaCardInstanceInfos.Select(instanceInfo => instanceInfo.InstanceID));
            }
            
            var output = _collectionCardDatasHandler.GetCollectionCopy();
            
            foreach (var cardData in output.CollectionCardDatas.Values)
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
                    comboCoreIDs.Add(collectionComboData.CoreID);
                }   
            }

            var output = _collectionComboDatasHandler.GetCollectionCopy();
            
            foreach (var comboData in output.CollectionComboDatas)
            {
                foreach (var coreID in comboCoreIDs)
                {
                    if (comboData.CoreID == coreID)
                    {
                        comboData.RemoveComboInstance();
                    }
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

            foreach (var metaCollectionCardData in _collectionCardDatasHandler.CollectionCardDatas.Values)
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