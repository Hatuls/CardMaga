using System;
using System.Collections.Generic;
using Account.GeneralData;
using Factory;
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
        [SerializeField,ReadOnly] private List<ComboCore> _comboDatas;

        #endregion

        #region Prop

        public int DeckId  => _deckId; 
        public string DeckName  => _deckName;
        public int DeckIndex { get; }
        public bool IsNewDeck => _isNewDeck;

        public List<CardInstance> Cards => _cardDatas; 
        public List<ComboCore> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData,List<CardInstance> allCards,int deckIndex,bool isNewDeck)
        {
            _deckId = deckData.Id;
            _deckName = deckData.Name;
            _isNewDeck = isNewDeck;
            DeckIndex = deckIndex;

            if (isNewDeck)
            {
                _cardDatas = new List<CardInstance>();
                _comboDatas = new List<ComboCore>();
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

            ComboCore[] tempComboCores = deckData.Combos;
            int comboLength = tempComboCores.Length;

            _comboDatas = new List<ComboCore>(comboLength);

            GameFactory.ComboFactory comboFactory = GameFactory.Instance.ComboFactoryHandler; 

            for (int i = 0; i < comboLength; i++)
            {
                _comboDatas.Add(tempComboCores[i]);
            }
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

        public void AddCombo(ComboCore comboData)
        {
            _comboDatas.Add(comboData);
        }
        
        public void RemoveCombo(ComboCore comboData)
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
        
        public bool FindComboData(int comboCoreId, out ComboCore metaComboData)
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