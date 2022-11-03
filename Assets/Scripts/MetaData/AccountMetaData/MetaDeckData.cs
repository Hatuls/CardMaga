using System.Collections.Generic;
using Battle.Deck;

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

        public MetaDeckData(BaseDeck baseDeck)
        {
            
        }
    }
}