using System.Collections.Generic;
using System.Linq;
using CardMaga.MetaData.Collection;

namespace CardMaga.MetaData.AccoutData
{
    public class AccountCollectionCopyHelper
    {
        private MetaDeckData _metaDeckData;
        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;

        public MetaDeckData MetaDeckData => _metaDeckData;

        public CardsCollectionDataHandler CardsCollectionDataHandler => _cardsCollectionDataHandler;

        public ComboCollectionDataHandler ComboCollectionDataHandler => _comboCollectionDataHandler;
        

        public AccountCollectionCopyHelper(AccountDataCollectionHelper accountDataCollectionHelper,MetaDeckData metaDeckData)
        {
            _metaDeckData = metaDeckData.GetCopy();
            _cardsCollectionDataHandler = GetCardCollectionByDeck(metaDeckData.DeckId);
            _comboCollectionDataHandler = GetComboCollectionByDeck(metaDeckData.DeckId);
        }

        private CardsCollectionDataHandler GetCardCollectionByDeck(int deckId) 
        {
            bool Condition(MetaCardInstanceInfo metaCardInstanceInfo)
            {
                return metaCardInstanceInfo.IsInDeck(deckId);
            }
            
            var instanceIDs = new List<int>();

            foreach (var cardData in _cardsCollectionDataHandler.CollectionCardDatas.Values)  
            {
                if (cardData.TryGetMetaCardInstanceInfo(Condition,out MetaCardInstanceInfo[] metaCardInstanceInfos))
                    instanceIDs.AddRange(metaCardInstanceInfos.Select(instanceInfo => instanceInfo.InstanceID));
            }
            
            var output = new CardsCollectionDataHandler(_cardsCollectionDataHandler.GetCollectionCopy());
            
            foreach (var cardData in output.CollectionCardDatas.Values)
            {
                foreach (var instanceID in instanceIDs)
                {
                    cardData.RemoveCardInstance(instanceID);
                }
            }
            
            return output;
        }

        private ComboCollectionDataHandler GetComboCollectionByDeck(int deckId)
        {
            bool Condition(MetaComboInstanceInfo metaComboInstanceInfo)
            {
                return metaComboInstanceInfo.IsInDeck(deckId);
            }
            
            var comboCoreIDs = new List<int>();

            foreach (var collectionComboData in _comboCollectionDataHandler.CollectionComboDatas) 
            {
                if (collectionComboData.TryGetMetaComboInstanceInfo(Condition,out MetaComboInstanceInfo[] metaComboInstanceInfos))
                {
                    comboCoreIDs.Add(collectionComboData.CoreID);
                }   
            }

            var output = new ComboCollectionDataHandler(_comboCollectionDataHandler.GetCollectionCopy());
            
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

            foreach (var metaCollectionCardData in _cardsCollectionDataHandler.CollectionCardDatas.Values)
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