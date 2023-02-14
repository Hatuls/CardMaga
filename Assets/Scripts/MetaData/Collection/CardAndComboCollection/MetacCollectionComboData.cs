using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using Unity.Collections;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionComboData : BaseCollectionDataItem , IEquatable<MetaCollectionComboData>,IEquatable<ComboCore>
    {
        public event Action<MetaComboInstanceInfo> OnTryAddItemToCollection; 
        public event Action<ComboCore> OnTryRemoveItemFromCollection;
        public event Action OnSuccessAddOrRemoveFromCollection;
        
        [SerializeField,ReadOnly]
        private ComboCore _comboData;
        [SerializeField,ReadOnly]
        private List<MetaComboInstanceInfo> _comboInstanceInfos;

        public ComboCore ComboData => _comboData;
        public bool IsInAnyDeck => _comboInstanceInfos.Count == 0;
        public override int NumberOfInstance => _comboInstanceInfos.Count;
        public int CoreID => _comboData.CoreID;

        public MetaCollectionComboData(MetaCollectionComboData copyFrom)
        {
            _comboInstanceInfos = copyFrom._comboInstanceInfos;
            _comboData = copyFrom.ComboData;
            _maxInstants = 1;
        }
        
        public MetaCollectionComboData(MetaComboInstanceInfo comboInstance)
        {
            _comboInstanceInfos = new List<MetaComboInstanceInfo>();
            _comboData = comboInstance.ComboInstance.ComboCore;
            _maxInstants = 1; //plastte 23.01.2023
            _comboInstanceInfos.Add(comboInstance);
        }

        public void AddComboToCollection()
        {
            OnTryAddItemToCollection?.Invoke(_comboInstanceInfos[0]);
        }

        public void RemoveComboFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(_comboData);
        }

        public void SuccessAddOrRemoveFromCollection(MetaComboInstanceInfo comboInstance)
        {
            OnSuccessAddOrRemoveFromCollection?.Invoke();
        }

        public void AddComboInstance(MetaComboInstanceInfo metaComboInstanceInfo)
        {
            _comboInstanceInfos.Add(metaComboInstanceInfo);
        }

        public bool RemoveComboInstance(MetaComboInstanceInfo metaComboInstanceInfo)
        {
            if (!_comboInstanceInfos.Contains(metaComboInstanceInfo)) 
                return false; 
            
            _comboInstanceInfos.Remove(metaComboInstanceInfo);
            return true;
        }
        
        public void RemoveComboInstance(int instanceID)
        {
            if (FindComboInstance(instanceID,out MetaComboInstanceInfo comboInstance))
                _comboInstanceInfos.Remove(comboInstance);
        }
        
        public void RemoveComboInstance()
        {
            _comboInstanceInfos.RemoveAt(0);
        }
        
        public bool TryGetMetaComboInstanceInfo(Predicate<MetaComboInstanceInfo> condition ,out MetaComboInstanceInfo[] comboInstance)
        {
            List<MetaComboInstanceInfo> output = _comboInstanceInfos.Where(condition.Invoke).ToList();

            if (output.Count > 0)
            {
                comboInstance = output.ToArray();
                return true;
            }
            
            comboInstance = null;
            return false;
        }
        
        public bool FindComboInstance(int instanceId,out MetaComboInstanceInfo comboInstance)
        {
            foreach (var comboInstanceInfo in _comboInstanceInfos)
            {
                if (comboInstanceInfo.InstanceID != instanceId) continue;
                
                comboInstance = comboInstanceInfo;
                return true;
            }

            comboInstance = null;
            return false;
        }

        public bool Equals(MetaCollectionComboData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CoreID == other.CoreID;
        }

        public bool Equals(ComboCore other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CoreID == other.CoreID;
        }
    }
}