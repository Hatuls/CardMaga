using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.Card;
using Factory;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaDeckData : IEquatable<MetaDeckData>
    {
        #region Events
        
        public event Action<MetaCardData> OnCardAdd;
        public event Action<string> OnCardAddFaild;
        public event Action<MetaCardData> OnCardRemove; 
        public event Action<MetaComboData> OnComboAdd;
        public event Action<string> OnComboAddFaild; 
        public event Action<MetaComboData> OnComboRemove; 

        #endregion

        #region Fields

        private int _id;
        private string _deckName;
        private List<MetaCardData> _cardDatas;
        private List<MetaComboData> _comboDatas;

        #endregion

        #region Prop

        public int Id  => _id; 
        public string DeckName  => _deckName;
        public List<MetaCardData> Cards => _cardDatas; 
        public List<MetaComboData> Combos => _comboDatas;

        #endregion
        
        public MetaDeckData(DeckData deckData)
        {
            _id = deckData.Id;
            _deckName = deckData.Name;

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
        
        public bool TryEditDeckName(string name)
        {
            _deckName = name;//add name vaild

            return true;
        }

        public void AddCard(MetaCardData cardData)
        {
            _cardDatas.Add(cardData);
        }
        
        public void RemoveCard(MetaCardData cardData)
        {
            _cardDatas.Remove(cardData);
        }
        
        public void RemoveCard(int cardIndex)//instanceID
        {
            
        }
        
        public void AddCombo(MetaComboData comboData)
        {
            _comboDatas.Add(comboData);
        }
        
        public void RemoveCombo(MetaComboData comboData)
        {
            
        }
        
        public void RemoveCombo(int comboIndex)
        {
            
        }

        private bool FindMetaCardData(int cardCoreId, out MetaCardData metaCardData)
        {
            for (int i = 0; i < _cardDatas.Count; i++)
            {
                if (_cardDatas[i].BattleCardData.CardInstance.ID == cardCoreId)
                {
                    metaCardData = _cardDatas[i];
                    return true;
                }
            }

            metaCardData = null;
            return false;
        }

        public bool Equals(MetaDeckData other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }
    }
}