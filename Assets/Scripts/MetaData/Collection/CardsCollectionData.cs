using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class CardsCollectionData
    {
        [SerializeField,ReadOnly] private List<MetaCollectionCardData> _collectionCardDatas;

        public CardsCollectionData(List<MetaCollectionCardData> cardDatas)
        {
            _collectionCardDatas = cardDatas;
        }

        public CardsCollectionData(MetaAccountData metaAccountData)
        {
            List<CardInstance> cardDatas = metaAccountData.AccountCards.OrderBy(x => x.CoreID).ToList();

            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();

            foreach (var cardInstance in cardDatas)
            {
                bool isAdded = false;
                
                foreach (var collectionCardData in output.Where(collectionCardData => collectionCardData.Equals(cardInstance)))
                {
                    collectionCardData.AddCardInstance(cardInstance);
                    isAdded = true;
                }

                if (isAdded)
                    continue;
                
                output.Add(new MetaCollectionCardData(cardInstance));
            }
            
            SortCardByDeck(metaAccountData.CharacterDatas.CharacterDatas);

            _collectionCardDatas = output;
        }
        
        private void SortCardByDeck(MetaCharacterData[] metaCharacterDatas)
        {
            foreach (var characterData in metaCharacterDatas)
            {
                foreach (var deckData in characterData.Decks)       
                {
                    foreach (var cardData in deckData.Cards)
                    {
                        foreach (var collectionCardData in _collectionCardDatas)
                        {
                            if (collectionCardData.FindCardInstance(cardData.InstanceID,out MetaCardInstanceInfo metaCardInstanceInfo))
                            {
                                metaCardInstanceInfo.AddToDeck(deckData.DeckId);
                            }
                        }
                    }
                }
            }
        }

        public MetaCollectionCardData GetCardCollectionData(int coreID)
        {
            return _collectionCardDatas.FirstOrDefault(cardData => cardData.CoreId == coreID);
        }

        public bool TryGetCardInstance(int instanceID,out CardInstance cardInstance)
        {
            foreach (var cardData in _collectionCardDatas)      
            {
                foreach (var instance in cardData.CardInstances)
                {
                    if (instance.InstanceID == instanceID)
                    {
                        cardInstance = instance.GetCardData();
                        return true;
                    }
                }
            }

            cardInstance = null;
            return false;
        }

        public MetaCardInstanceInfo GetCardInstanceInfo(int instanceID)
        {
            foreach (var cardData in _collectionCardDatas)
            {
                if (cardData.FindCardInstance(instanceID, out MetaCardInstanceInfo metaCardInstanceInfo))
                    return metaCardInstanceInfo;
            }

            return null;
        }

        public CardsCollectionData GetCollectionByDeck(int deckID)
        {
            var output = new CardsCollectionData(GetCollectionCopy());

            foreach (var cardData in _collectionCardDatas)  
            {
                if (cardData.IsCardInDeck(deckID,out List<MetaCardInstanceInfo> metaCardInstanceInfos))
                {
                    foreach (var cardInstance in metaCardInstanceInfos)
                    {
                        cardInstance
                    }
                }
            }
        }

        private List<MetaCollectionCardData> GetCollectionCopy()
        {
            return
                _collectionCardDatas.Select(cardData => new MetaCollectionCardData(cardData.GetCardInstanceData()))
                    .ToList();
        }
    }
}