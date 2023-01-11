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
    public class ComboCollectionDataHandler
    {
        [SerializeField,ReadOnly] private List<MetaCollectionComboData> _collectionComboDatas;

        public List<MetaCollectionComboData> CollectionComboDatas => _collectionComboDatas;

        public ComboCollectionDataHandler()
        {
        }
        
        public ComboCollectionDataHandler(List<MetaCollectionComboData> collectionComboDatas)
        {
            _collectionComboDatas = collectionComboDatas;
        }

        public ComboCollectionDataHandler(MetaAccountData metaAccountData)
        {
            List<ComboInstance> comboCores = metaAccountData.AccountCombos;
            _collectionComboDatas = new List<MetaCollectionComboData>();
            
            _collectionComboDatas.AddRange(comboCores.Select(comboData => new MetaCollectionComboData(comboData)));
            
            
            MetaCharacterData[] metaCharacterDatas = metaAccountData.CharacterDatas.CharacterDatas;

            foreach (var characterData in metaCharacterDatas)
            { 
                foreach (var deckData in characterData.Decks) 
                { 
                    foreach (var comboData in deckData.Combos)
                    { 
                        foreach (var collectionCardData in _collectionComboDatas) 
                        {
                            if (collectionCardData.FindComboInstance(comboData.InstanceID,out MetaComboInstanceInfo metaComboInstanceInfo))
                            { 
                                metaComboInstanceInfo.AddDeckReference(deckData.DeckId);
                            }
                        }
                    }
                }
                
            }
        }
        
        public void AddComboCollection(ComboInstance comboInstance)
        {
            foreach (var collectionComboData in _collectionComboDatas)
            {
                if (collectionComboData.CoreID != comboInstance.CoreID) continue;
                
                collectionComboData.AddComboInstance(new MetaComboInstanceInfo(comboInstance));
                return;
            }
            
            _collectionComboDatas.Add(new MetaCollectionComboData(comboInstance));
        }

        public bool TryRemoveComboCollection(int comboCoreId)
        {
            MetaCollectionComboData cache = null;
            
            foreach (var collectionComboData in _collectionComboDatas)
            {
                if (collectionComboData.CoreID == comboCoreId)
                {
                    cache = collectionComboData;
                } 
            }

            if (cache == null) return false;
            
            _collectionComboDatas.Remove(cache);
            return true;
        }
        
        public bool TryGetComboCollectionData(Predicate<MetaCollectionComboData> condition,out MetaCollectionComboData[] metaCollectionComboData)
        {
            List<MetaCollectionComboData> output = new List<MetaCollectionComboData>();

            foreach (var collectionComboData in _collectionComboDatas)
            {
                if (!condition.Invoke(collectionComboData)) continue;
                
                output.Add(collectionComboData); 
            }

            if (output.Count > 0)
            {
                metaCollectionComboData = output.ToArray();
                return true;
            }
            
            metaCollectionComboData = null;
            return false;
        }

        public List<MetaCollectionComboData> GetCollectionCopy()
        {
            return
                _collectionComboDatas.Select(comboData => new MetaCollectionComboData(new ComboInstance(comboData.ComboData)))
                    .ToList();
        }
    }
}