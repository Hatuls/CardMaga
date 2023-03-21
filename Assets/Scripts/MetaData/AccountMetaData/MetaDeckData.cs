using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using Factory;
using Rei.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class MetaDeckData : IEquatable<MetaDeckData>
    {
        #region Fields

        [SerializeField,ReadOnly] private int _deckId;
        [SerializeField,ReadOnly] private string _deckName;
        private bool _isNewDeck;
        [SerializeField,ReadOnly] private List<MetaCardInstanceInfo> _cardDatas;
        [SerializeField,ReadOnly] private List<MetaComboInstanceInfo> _comboDatas;

        #endregion

        #region Prop

        public int DeckId  => _deckId; 
        public string DeckName  => _deckName;
        public bool IsNewDeck => _isNewDeck;

        public List<MetaCardInstanceInfo> Cards => _cardDatas; 
        public List<MetaComboInstanceInfo> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData,List<MetaCardInstanceInfo> allCards,List<MetaComboInstanceInfo> allCombo,bool isNewDeck)
        {
            _deckId = deckData.Id;
            _deckName = deckData.Name;
            _isNewDeck = isNewDeck;
            
            if (isNewDeck)
            {
                _cardDatas = new List<MetaCardInstanceInfo>();
                _comboDatas = new List<MetaComboInstanceInfo>();
                return;
            }
            
            CoreID[] cardCore = deckData.Cards;
            
            _cardDatas = new List<MetaCardInstanceInfo>(cardCore.Length);

            foreach (var coreID in cardCore)
            {
                foreach (var cardInstance in allCards)
                {
                    if (coreID.ID == cardInstance.CoreID)
                    {
                        if (_cardDatas.Contains(cardInstance))
                            continue;

                        AddCard(cardInstance);
                        break;
                    }
                }
            }

            ComboCore[] comboCores = deckData.Combos;
            
            _comboDatas = new List<MetaComboInstanceInfo>(comboCores.Length);

            foreach (var coreID in comboCores)
            {
                foreach (var comboInstance in allCombo)  
                {
                    if (coreID.CoreID == comboInstance.CoreID)
                    {
                        if (_comboDatas.Contains(comboInstance))
                            continue;
                        AddCombo(comboInstance);
                    }
                }
            }
        }

        public MetaDeckData(int deckId,string deckName,List<MetaCardInstanceInfo> cardDatas,List<MetaComboInstanceInfo> comboDatas,bool isNewDeck)
        {
            _deckId = deckId;
            _deckName = deckName;

            _cardDatas = cardDatas;
            _comboDatas = comboDatas;
            _isNewDeck = isNewDeck;
        }
        
        public MetaDeckData GetCopy()
        {
            return new MetaDeckData(_deckId, _deckName, _cardDatas.Copy(), _comboDatas.Copy(),_isNewDeck);
        }

        public void UpdateDeck(MetaDeckData metaDeckData)
        {
            foreach (var instanceInfo in metaDeckData.Cards)
            {
                if(_cardDatas.Contains(instanceInfo)) continue;
                
                AddCard(instanceInfo);
            }
            
            foreach (var instanceInfo in metaDeckData.Combos)
            {
                if(_comboDatas.Contains(instanceInfo)) continue;
                
                AddCombo(instanceInfo);
            }
            
            _deckName = metaDeckData.DeckName;
        }
        
        public void UpdateDeckName(string name)
        {
            _deckName = name;
        }

        public void AddCard(MetaCardInstanceInfo cardData)
        {
            _cardDatas.Add(cardData);
            cardData.AddToDeck(DeckId);
        }
        
        public void RemoveCard(MetaCardInstanceInfo cardData)
        {
            _cardDatas.Remove(cardData);
            cardData.RemoveFromDeck(DeckId);
        }

        public void AddCombo(MetaComboInstanceInfo comboData)
        {
            _comboDatas.Add(comboData);
            comboData.AddDeckReference(DeckId);
        }
        
        public void RemoveCombo(MetaComboInstanceInfo comboData)
        {
            _comboDatas.Remove(comboData);
            comboData.RemoveDeckReference(DeckId);
        }

        public bool FindCardData(int cardCoreId, out MetaCardInstanceInfo metaCardData)
        {
            for (int i = 0; i < _cardDatas.Count; i++)
            {
                if (_cardDatas[i].Equals(cardCoreId))
                {
                    metaCardData = _cardDatas[i];
                    return true;
                }
            }

            metaCardData = null;
            return false;
        }
        
        public bool FindComboData(int comboCoreId, out MetaComboInstanceInfo metaComboData)
        {
            for (int i = 0; i < _comboDatas.Count; i++)
            {
                if (_comboDatas[i].Equals(comboCoreId))
                {
                    metaComboData = _comboDatas[i];
                    return true;
                }
            }

            metaComboData = null;
            return false;
        }

        public void RegisterDeck()
        {
            _isNewDeck = false;
        }

        public bool Equals(MetaDeckData other)
        {
            if (other == null)
                return false;
            return DeckId == other.DeckId;
        }
    }
}