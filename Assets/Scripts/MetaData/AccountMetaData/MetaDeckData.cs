using System.Collections.Generic;
using Account.GeneralData;
using Battle.Combo;
using CardMaga.Card;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaDeckData
    {
        private int _id;
        private string _deckName;
        private List<MetaCardData> _cardDatas;
        private List<MetaComboData> _comboDatas;
        
        public int Id  => _id; 
        public string DeckName  => _deckName;
        public List<MetaCardData> Cards { get => _cardDatas; set => _cardDatas = value; }
        public List<MetaComboData> Combos { get => _comboDatas; set => _comboDatas = value; }

        public bool TryEditDeckName(string name)
        {
            _deckName = name;//add name vaild

            return true;
        }

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
    }
}