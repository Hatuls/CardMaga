using System;
using System.Collections.Generic;
using Account.GeneralData;
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
        [SerializeField,ReadOnly] private List<CardInstance> _cardDatas;
        [SerializeField,ReadOnly] private List<ComboInstance> _comboDatas;

        #endregion

        #region Prop

        public int DeckId  => _deckId; 
        public string DeckName  => _deckName;
        public bool IsNewDeck => _isNewDeck;

        public List<CardInstance> Cards => _cardDatas; 
        public List<ComboInstance> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData,List<CardInstance> allCards,List<ComboInstance> allCombo,bool isNewDeck)
        {
            _deckId = deckData.Id;
            _deckName = deckData.Name;
            _isNewDeck = isNewDeck;
            
            if (isNewDeck)
            {
                _cardDatas = new List<CardInstance>();
                _comboDatas = new List<ComboInstance>();
                return;
            }
            
            CoreID[] cardCore = deckData.Cards;
            
            _cardDatas = new List<CardInstance>(cardCore.Length);

            foreach (var coreID in cardCore)
            {
                foreach (var cardInstance in allCards)  
                {
                    if (coreID.ID == cardInstance.CoreID)
                    {
                        if (_cardDatas.Contains(cardInstance))
                            continue;
                        _cardDatas.Add(cardInstance);
                    }
                }
            }
            
            ComboCore[] comboCores = deckData.Combos;
            
            _comboDatas = new List<ComboInstance>(comboCores.Length);

            foreach (var coreID in comboCores)
            {
                foreach (var comboInstance in allCombo)  
                {
                    if (coreID.CoreID == comboInstance.CoreID)
                    {
                        if (_comboDatas.Contains(comboInstance))
                            continue;
                        _comboDatas.Add(comboInstance);
                    }
                }
            }
        }

        public MetaDeckData(int deckId,string deckName,List<CardInstance> cardDatas,List<ComboInstance> comboDatas,bool isNewDeck)
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
            _cardDatas = metaDeckData.Cards;
            _comboDatas = metaDeckData.Combos;
            _deckName = metaDeckData.DeckName;
        }
        
        public void UpdateDeckName(string name)
        {
            _deckName = name;
        }

        public void AddCard(CardInstance cardData)
        {
            _cardDatas.Add(cardData);
        }
        
        public void RemoveCard(CardInstance cardData)
        {
            _cardDatas.Remove(cardData);
        }

        public void AddCombo(ComboInstance comboData)
        {
            _comboDatas.Add(comboData);
        }
        
        public void RemoveCombo(ComboInstance comboData)
        {
            _comboDatas.Remove(comboData);
        }

        public bool FindCardData(int cardCoreId, out CardInstance metaCardData)
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
        
        public bool FindComboData(int comboCoreId, out ComboInstance metaComboData)
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