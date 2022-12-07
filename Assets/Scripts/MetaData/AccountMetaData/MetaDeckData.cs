using System;
using System.Collections.Generic;
using Account.GeneralData;
using Factory;
using Sirenix.OdinInspector.Editor;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaDeckData : IEquatable<MetaDeckData>
    {
        #region Fields

        private int _deckId;
        private string _deckName;
        private bool _isNewDeck;
        private List<MetaCardData> _cardDatas;
        private List<MetaComboData> _comboDatas;

        #endregion

        #region Prop

        public int DeckId  => _deckId; 
        public string DeckName  => _deckName;
        public int DeckIndex { get; }
        public bool IsNewDeck => _isNewDeck;

        public List<MetaCardData> Cards => _cardDatas; 
        public List<MetaComboData> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData,int deckIndex,bool isNewDeck)
        {
            _deckId = deckData.Id;
            _deckName = deckData.Name;
            _isNewDeck = isNewDeck;
            DeckIndex = deckIndex;

            if (isNewDeck)
            {
                _cardDatas = new List<MetaCardData>();
                _comboDatas = new List<MetaComboData>();
                return;
            }
            
            GameFactory.CardFactory cardFactory = GameFactory.Instance.CardFactoryHandler;

            CardCore[] tempCardCore = cardFactory.CreateCardCores(deckData.Cards);
            int cardLength = tempCardCore.Length;
            
            _cardDatas = new List<MetaCardData>(cardLength);

            for (int i = 0; i < cardLength; i++)
            {
                _cardDatas.Add(cardFactory.GetMetaCardData(tempCardCore[i]));//need To remove carddata
            }

            ComboCore[] tempComboCores = deckData.Combos;
            int comboLength = tempComboCores.Length;

            _comboDatas = new List<MetaComboData>(comboLength);

            GameFactory.ComboFactory comboFactory = GameFactory.Instance.ComboFactoryHandler; 

            for (int i = 0; i < comboLength; i++)
            {
                _comboDatas.Add(comboFactory.GetMetaComboData(tempComboCores[i]));
            }
        }

        public void UpdateDeck(MetaDeckData metaDeckData)
        {
            _cardDatas = metaDeckData._cardDatas;
            _comboDatas = metaDeckData._comboDatas;
            _deckName = metaDeckData._deckName;
        }
        
        public void UpdateDeckName(string name)
        {
            _deckName = name;
        }

        public void AddCard(MetaCardData cardData)
        {
            _cardDatas.Add(cardData);
        }
        
        public void RemoveCard(MetaCardData cardData)
        {
            _cardDatas.Remove(cardData);
        }

        public void AddCombo(MetaComboData comboData)
        {
            _comboDatas.Add(comboData);
        }
        
        public void RemoveCombo(MetaComboData comboData)
        {
            _comboDatas.Remove(comboData);
        }

        public bool FindMetaCardData(int cardCoreId, out MetaCardData metaCardData)
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
        
        public bool FindMetaComboData(int comboCoreId, out MetaComboData metaComboData)
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