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

        //[SerializeField,ReadOnly] private List<MetaCollectionCardData> _collectionCardDatas;
        private Dictionary<int, MetaCollectionCardData> _collectionCardDatas;

        public Dictionary<int, MetaCollectionCardData> CollectionCardDatas => _collectionCardDatas;

        public CardsCollectionDataHandler()
        {
            _collectionCardDatas = new Dictionary<int, MetaCollectionCardData>();
        }

        public CardsCollectionDataHandler(List<MetaCollectionCardData> cardDatas)
        {
            _collectionCardDatas = new Dictionary<int, MetaCollectionCardData>();

            foreach (var cardData in cardDatas)
            {
                _collectionCardDatas.Add(cardData.CardCoreID,cardData);
            }
        }

        public CardsCollectionDataHandler(MetaAccountData metaAccountData)
        {
            List<CardInstance> cardDatas = metaAccountData.AccountCards.OrderBy(x => x.CoreID).ToList();

            _collectionCardDatas = new Dictionary<int, MetaCollectionCardData>();

            foreach (var cardData in cardDatas)
            {
                if (_collectionCardDatas.ContainsKey(cardData.CoreID))
                {
                    _collectionCardDatas[cardData.CoreID].AddCardInstance(new MetaCardInstanceInfo(cardData));
                }
                else
                {
                    _collectionCardDatas.Add(cardData.CoreID,new MetaCollectionCardData(new MetaCardInstanceInfo(cardData)));
                }
            }

            MetaCharacterData[] metaCharacterDatas = metaAccountData.CharacterDatas.CharacterDatas;

            foreach (var characterData in metaCharacterDatas)
            {
                foreach (var deckData in characterData.Decks)
                {
                    foreach (var cardData in deckData.Cards)
                    {
                        if (!_collectionCardDatas.ContainsKey(cardData.CoreID)) continue;
                        
                        if(_collectionCardDatas[cardData.CoreID].FindCardInstance(cardData.InstanceID,out var metaCardInstanceInfo))
                        {
                            metaCardInstanceInfo.AddToDeck(deckData.DeckId);
                        }
                    }
                }
            }
        }

        public void CleanCollection()
        {
            var output = new List<MetaCollectionCardData>();
            
            foreach (var value in _collectionCardDatas.Values)
            {
                if (value.NumberOfInstance == 0)
                {
                    output.Add(value);
                }
            }

            for (int i = 0; i < output.Count; i++)
            {
                _collectionCardDatas.Remove(output[i].CardCoreID);
                output.Remove(output[i]);
            }
        }
        
        public void AddCardInstance(params MetaCardInstanceInfo[] cardInstances)
        {
            foreach (var cardInstance in cardInstances)
            {
                if (_collectionCardDatas.ContainsKey(cardInstance.CoreID))
                {
                    _collectionCardDatas[cardInstance.CoreID].AddCardInstance(cardInstance);
                }
                else
                {
                    _collectionCardDatas.Add(cardInstance.CoreID,new MetaCollectionCardData(cardInstance));
                }
            }
        }

        public bool TryRemoveCardInstance(int instanceID,bool toRemoveCardCollectionData)
        {
            foreach (var metaCollectionCardData in _collectionCardDatas.Values)
            {
                if (metaCollectionCardData.NumberOfInstance == 0 && toRemoveCardCollectionData)
                {
                    _collectionCardDatas.Remove(metaCollectionCardData.CardCoreID);
                    break;
                }
                
                foreach (var cardInstanceInfo in metaCollectionCardData.CardInstances)
                {
                    if (instanceID != cardInstanceInfo.InstanceID) continue;
                    
                    metaCollectionCardData.RemoveCardInstance(cardInstanceInfo.InstanceID);

                    if (toRemoveCardCollectionData)
                    {
                        if (metaCollectionCardData.NumberOfInstance == 0)
                            _collectionCardDatas.Remove(metaCollectionCardData.CardCoreID);
                    }

                    return true;
                }
            }

            return false;
        }

        // public bool TryGetCardCollectionData(Predicate<MetaCollectionCardData> condition,out MetaCollectionCardData[] metaCollectionCardData)
        // {
        //     List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();
        //     
        //     foreach (var collectionCardData in _collectionCardDatas)
        //     {
        //         if (!condition.Invoke(collectionCardData)) continue;
        //
        //         output.Add(collectionCardData);
        //     }
        //
        //     if (output.Count > 0)
        //     {
        //         metaCollectionCardData = output.ToArray();
        //         return true;
        //     }
        //     
        //     metaCollectionCardData = null;
        //     return false;
        // }
        
        public bool TryGetCardInstanceInfo(Predicate<MetaCardInstanceInfo> condition,out MetaCardInstanceInfo[] metaCardInstanceInfo)
        {
            List<MetaCardInstanceInfo> output = new List<MetaCardInstanceInfo>();

            foreach (var metaCollectionData in _collectionCardDatas.Values)
            {
                foreach (var cardInstanceInfo in metaCollectionData.CardInstances)
                {
                    if (condition.Invoke(cardInstanceInfo))
                    {
                        output.Add(cardInstanceInfo);
                    }
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
            if (!_collectionCardDatas.ContainsKey(cardInstance.CoreID)) return;
            
            if (_collectionCardDatas[cardInstance.CoreID].FindCardInstance(cardInstance.InstanceID,
                    out MetaCardInstanceInfo metaCardInstanceInfo))
            {
                metaCardInstanceInfo.AddToDeck(deckId);
            }
        }
        
        public void RemoveDeckAssociate(CardInstance cardInstance, int deckId)
        {
            if (!_collectionCardDatas.ContainsKey(cardInstance.CoreID)) return;
            
            if (_collectionCardDatas[cardInstance.CoreID].FindCardInstance(cardInstance.InstanceID,
                    out MetaCardInstanceInfo metaCardInstanceInfo))
            {
                metaCardInstanceInfo.RemoveFromDeck(deckId);
            }
        }

        public List<MetaCollectionCardData> GetCollectionCopy()
        {
            return
                _collectionCardDatas.Values.Select(cardData => cardData.GetCopy())
                    .ToList();
        }
    }
}