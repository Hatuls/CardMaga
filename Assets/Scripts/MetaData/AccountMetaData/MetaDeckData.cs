using System;
using System.Collections.Generic;
using Account.GeneralData;
using Factory;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaDeckData : IEquatable<MetaDeckData>
    {
        #region Fields

        private int _deckId;
        private string _deckName;
        private bool _isNewDeck;
        private List<CardInstance> _cardDatas;
        private List<ComboCore> _comboDatas;

        #endregion

        #region Prop

        public int DeckId  => _deckId; 
        public string DeckName  => _deckName;
        public int DeckIndex { get; }
        public bool IsNewDeck => _isNewDeck;

        public List<CardInstance> Cards => _cardDatas; 
        public List<ComboCore> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData,int deckIndex,bool isNewDeck)
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
            
            GameFactory.CardFactory cardFactory = GameFactory.Instance.CardFactoryHandler;

            CoreID[] tempCardCore = deckData.Cards;
            int cardLength = tempCardCore.Length;
            
            _cardDatas = new List<CardInstance>(cardLength);

            for (int i = 0; i < cardLength; i++)
            {
                _cardDatas.Add(cardFactory.CreateCardInstance(tempCardCore[i]));//need To remove carddata
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