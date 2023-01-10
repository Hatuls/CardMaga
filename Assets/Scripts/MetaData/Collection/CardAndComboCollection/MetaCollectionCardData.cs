using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionCardData : BaseCollectionDataItem, IEquatable<MetaCollectionCardData>,IEquatable<CardInstance>, IEquatable<MetaCardInstanceInfo>
    { 
        public event Action<CardInstance> OnTryAddItemToCollection; 
        public event Action<CardCore> OnTryRemoveItemFromCollection;
        public event Action OnSuccessAddOrRemoveFromCollection;

#if UNITY_EDITOR
        [SerializeField, ReadOnly] private string _cardName;
#endif
        [SerializeField, ReadOnly] private List<MetaCardInstanceInfo> _cardInstances;
        private CardCore _cardCore;

        public int CardCoreID => _cardCore.CoreID;
        public List<MetaCardInstanceInfo> CardInstances => _cardInstances;
        public override  int NumberOfInstance => _cardInstances.Count;
        
        public MetaCollectionCardData(MetaCardInstanceInfo cardInstance)
        {
            _cardCore = cardInstance.CardInstance.GetCardCore();//need to change

            _cardName = cardInstance.CardInstance.CardSO.CardName;
            
            _cardInstances = new List<MetaCardInstanceInfo>();
            
            AddCardInstance(cardInstance);
        }

        public MetaCollectionCardData(List<MetaCardInstanceInfo> instanceInfos)
        {
            _cardCore = instanceInfos[0].CardInstance.GetCardCore();
            _cardName = instanceInfos[0].CardInstance.CardSO.CardName;
            _cardInstances = instanceInfos;
            _maxInstants = _cardInstances.Count;
        }

        public void AddCardToCollection()
        {
            OnTryAddItemToCollection?.Invoke(GetCardInstanceData());
        }

        public void RemoveCardFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(_cardCore);
        }

        public void SuccessAddOrRemoveFromCollection(CardInstance cardInstance)
        {
            OnSuccessAddOrRemoveFromCollection?.Invoke();
        }
        
        public MetaCardInstanceInfo GetCardInstanceData(int instantsId)
        {
            if (FindCardInstance(instantsId,out MetaCardInstanceInfo metaCardInstanceInfo))
                return metaCardInstanceInfo;

            Debug.LogWarning("Card instancesId was not found");
            return null;
        }

        public bool TryGetMetaCardInstanceInfo(Predicate<MetaCardInstanceInfo> condition ,out MetaCardInstanceInfo[] cardInstances)
        {
            List<MetaCardInstanceInfo> output = _cardInstances.Where(condition.Invoke).ToList();

            if (output.Count > 0)
            {
                cardInstances = output.ToArray();
                return true;
            }
            
            cardInstances = null;
            return false;
        }

        public CardInstance GetCardInstanceData()
        {
            if (_cardInstances[0] == null)
                return null;
            
            return _cardInstances[0].CardInstance;
        }
        
        private List<MetaCardInstanceInfo> GetCardInstanceInfoDataCopy()
        {
            List<MetaCardInstanceInfo> output = new List<MetaCardInstanceInfo>(_cardInstances.Count);
            
            output.AddRange(_cardInstances.Select(instanceInfo => new MetaCardInstanceInfo(instanceInfo.CardInstance,instanceInfo.AssociateDeck)));

            return output;
        }

        public MetaCollectionCardData GetCopy()
        {
            List<MetaCardInstanceInfo> cache = new List<MetaCardInstanceInfo>(_cardInstances.Count);
            
            cache.AddRange(_cardInstances.Select(instanceInfo => new MetaCardInstanceInfo(instanceInfo.CardInstance,instanceInfo.AssociateDeck)));
            
             return new MetaCollectionCardData(cache);
        }

        public void AddCardInstance(MetaCardInstanceInfo cardInstance)
        {
            _cardInstances.Add(cardInstance);

            if (_maxInstants < _cardInstances.Count)
                _maxInstants = _cardInstances.Count;
        }
        
        public void RemoveCardInstance(int instanceID)
        {
            if (FindCardInstance(instanceID,out MetaCardInstanceInfo cardInstanceInfo))
            {
                _cardInstances.Remove(cardInstanceInfo);
                //cardInstanceInfo.Dispose();
            }
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
            return CardCoreID == other.CardCoreID;
        }

        public bool Equals(CardInstance other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardCoreID == other.CoreID;
        }

        public bool Equals(MetaCardInstanceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardCoreID == other.CoreID;
        }
    }
}