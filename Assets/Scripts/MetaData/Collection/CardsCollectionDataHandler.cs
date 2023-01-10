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
    public class CardsCollectionDataHandler
    {
        [SerializeField,ReadOnly] private List<MetaCollectionCardData> _collectionCardDatas;

        public List<MetaCollectionCardData> CollectionCardDatas => _collectionCardDatas;

        public CardsCollectionDataHandler()
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
        }

        public CardsCollectionDataHandler(List<MetaCollectionCardData> cardDatas)
        {
            _collectionCardDatas = cardDatas;
        }

        public CardsCollectionDataHandler(MetaAccountData metaAccountData)
        {
            List<CardInstance> cardDatas = metaAccountData.AccountCards.OrderBy(x => x.CoreID).ToList();

            _collectionCardDatas = new List<MetaCollectionCardData>();

            foreach (var cardInstance in cardDatas)
            {
                bool isAdded = false;
                
                foreach (var collectionCardData in _collectionCardDatas.Where(collectionCardData => collectionCardData.Equals(cardInstance)))
                {
                    collectionCardData.AddCardInstance(new MetaCardInstanceInfo(cardInstance));
                    isAdded = true;
                }

                if (isAdded)
                    continue;
                
                _collectionCardDatas.Add(new MetaCollectionCardData(new MetaCardInstanceInfo(cardInstance)));
            }
            
            MetaCharacterData[] metaCharacterDatas = metaAccountData.CharacterDatas.CharacterDatas;
            
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
        
        public void AddCardInstance(params MetaCardInstanceInfo[] cardInstances)
        {
            foreach (var cardInstance in cardInstances)
            {
                bool isAdded = false;
                
                foreach (var collectionCardData in _collectionCardDatas.Where(collectionCardData => collectionCardData.Equals(cardInstance)))
                {
                    collectionCardData.AddCardInstance(cardInstance);
                    isAdded = true;
                }

                if (isAdded)
                    continue;
                
                _collectionCardDatas.Add(new MetaCollectionCardData(cardInstance));
            }
        }

        public bool TryRemoveCardInstance(int instanceID)
        {
            foreach (var metaCollectionCardData in _collectionCardDatas)
            {
                foreach (var cardInstanceInfo in metaCollectionCardData.CardInstances)
                {
                    if (instanceID != cardInstanceInfo.InstanceID) continue;
                    
                    metaCollectionCardData.RemoveCardInstance(cardInstanceInfo.InstanceID);

                    if (metaCollectionCardData.NumberOfInstance == 0)
                        _collectionCardDatas.Remove(metaCollectionCardData);

                    return true;
                }
            }

            return false;
        }

        public bool TryGetCardCollectionData(Predicate<MetaCollectionCardData> condition,out MetaCollectionCardData[] metaCollectionCardData)
        {
            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();
            
            foreach (var collectionCardData in _collectionCardDatas)
            {
                if (!condition.Invoke(collectionCardData)) continue;

                output.Add(collectionCardData);
            }

            if (output.Count > 0)
            {
                metaCollectionCardData = output.ToArray();
                return true;
            }
            
            metaCollectionCardData = null;
            return false;
        }
        
        public bool TryGetCardInstanceInfo(Predicate<MetaCardInstanceInfo> condition,out MetaCardInstanceInfo[] metaCardInstanceInfo)
        {
            List<MetaCardInstanceInfo> output = new List<MetaCardInstanceInfo>();
            
            foreach (var collectionCardData in _collectionCardDatas)
            {
                foreach (var cardInstance in collectionCardData.CardInstances)
                {
                    if (!condition.Invoke(cardInstance)) continue;
                    
                    output.Add(cardInstance);
                }
            }

            if (output.Count > 0)
            {
                metaCardInstanceInfo = output.ToArray();
                return true;
            }
            
            metaCardInstanceInfo = null;
            return false;
        }

        public void AddDeckAssociate(CardInstance cardInstance, int deckId)
        {
            foreach (var metaCollectionCardData in _collectionCardDatas)
            {
                if (metaCollectionCardData.Equals(cardInstance))
                {
                    if (metaCollectionCardData.FindCardInstance(cardInstance.InstanceID,
                            out MetaCardInstanceInfo metaCardInstanceInfo))
                    {
                        metaCardInstanceInfo.AddToDeck(deckId);
                    }
                }
            }
        }
        
        public void RemoveDeckAssociate(CardInstance cardInstance, int deckId)
        {
            foreach (var metaCollectionCardData in _collectionCardDatas)
            {
                if (metaCollectionCardData.Equals(cardInstance))
                {
                    if (metaCollectionCardData.FindCardInstance(cardInstance.InstanceID,
                            out MetaCardInstanceInfo metaCardInstanceInfo))
                    {
                        metaCardInstanceInfo.RemoveFromDeck(deckId);
                    }
                }
            }
        }

        public List<MetaCollectionCardData> GetCollectionCopy()
        {
            return
                _collectionCardDatas.Select(cardData => cardData.GetCopy())
                    .ToList();
        }
    }
}