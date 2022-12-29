using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionCardData : BaseCollectionDataItem, IEquatable<MetaCollectionCardData>,IEquatable<CardInstance>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove item"; 
        public event Action<string> OnFailedAction;
        public event Action<MetaCollectionCardData> OnTryAddItemToCollection; 
        public event Action<MetaCollectionCardData> OnTryRemoveItemFromCollection;
        public event Action OnSuccessAddOrRemoveFromCollection;

        public int CoreId => _cardInstances[0].CoreID;

        public override  int NumberOfInstance => _cardInstances.Count;

        [SerializeField, ReadOnly] private List<MetaCardInstanceInfo> _cardInstances;
        
        public MetaCollectionCardData(CardInstance cardInstance)
        {
            _cardInstances = new List<MetaCardInstanceInfo>();
            
            _cardInstances.Add(new MetaCardInstanceInfo(cardInstance));
        }

        public void AddCardToCollection()
        {
            OnTryAddItemToCollection?.Invoke(this);
        }

        public void RemoveCardFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(this);
            RemoveItemFromCollection();
        }
        
        public CardInstance GetCardData(int instantsId)
        {
            if (FindCardInstance(instantsId,out MetaCardInstanceInfo metaCardInstanceInfo))
                return metaCardInstanceInfo.GetCardData();

            Debug.LogWarning("Card instancesId was not found");
            return null;
        }

        public bool GetUnassingCard(out CardInstance metaCardData)
        {
            foreach (var cardInstance in _cardInstances)
            {
                if (!cardInstance.InDeck)
                {
                    metaCardData = cardInstance.GetCardData();
                    return true;
                }
            }

            metaCardData = null;
            return false;
        }

        public CardInstance GetCardData()
        {
            return _cardInstances[0].GetCardData();
        }

        public void AddCardInstance(CardInstance cardInstance)
        {
            _cardInstances.Add(new MetaCardInstanceInfo(cardInstance));
        }
        
        public void RemoveCardInstance(int instanceID)
        {
            if (FindCardInstance(instanceID,out MetaCardInstanceInfo cardInstanceInfo))
            {
                _cardInstances.Remove(cardInstanceInfo);
                cardInstanceInfo.Dispose();
            }

            Debug.LogWarning("Card InstanceInfo Was not found");
        }

        public bool FindCardInstance(int instanceId,out MetaCardInstanceInfo cardInstanceInfo)
        {
            foreach (var cardInstance in _cardInstances)
            {
                if (cardInstance.InstanceID != instanceId) continue;
                
                cardInstanceInfo = cardInstance;
                return true;
            }

            cardInstanceInfo = null;
            return false;
        }
        
        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CoreId == other.CoreId;
        }

        public bool Equals(CardInstance other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CoreId == other.CoreID;
        }
    }
}