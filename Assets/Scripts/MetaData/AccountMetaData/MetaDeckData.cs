using System;
using System.Collections.Generic;
using Account.GeneralData;
using Battle.Combo;
using CardMaga.Card;

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
        \
        public MetaDeckData(DeckData deckData)
        {
            _id = deckData.Id;
            _deckName = deckData.Name;

            CardCore[] tempCardCore = deckData.Cards;
            int cardLength = tempCardCore.Length;
            
            _cardDatas = new List<MetaCardData>(cardLength);

            for (int i = 0; i < cardLength; i++)
            {
                CardInstanceID instanceID = Factory.GameFactory.Instance.CardFactoryHandler.CreateCardInstance(tempCardCore[i]);
                CardSO cardSo = Factory.GameFactory.Instance.CardFactoryHandler.GetCard(tempCardCore[i].ID);

                _cardDatas[i] = new MetaCardData(instanceID,cardSo);
            }

            ComboCore[] tempComboCores = deckData.Combos;
            int comboLength = tempComboCores.Length;

            _comboDatas = new List<MetaComboData>(comboLength);

            for (int i = 0; i < comboLength; i++)
            {
                ComboSO comboSo = Factory.GameFactory.Instance.ComboFactoryHandler.GetComboSO(tempComboCores[i].ID);//need To check rei______

                _comboDatas[i] = new MetaComboData(tempComboCores[i]);
            }
        }
        
        public bool TryEditDeckName(string name)
        {
            _deckName = name;//add name vaild

            return true;
        }

        public bool TryAddCard(MetaCardData cardData)
        {
            return true;
        }
        
        public void RemoveCard(MetaCardData cardData)
        {
            
        }
        
        public void RemoveCard(int cardIndex)
        {
            
        }
        
        public bool TryAddCombo(MetaComboData comboData)
        {
            return true;
        }
        
        public void RemoveCombo(MetaComboData comboData)
        {
            
        }
        
        public void RemoveCombo(int comboIndex)
        {
            
        }

        public bool Equals(MetaDeckData other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }
    }
}