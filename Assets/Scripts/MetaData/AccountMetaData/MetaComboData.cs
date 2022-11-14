using System;
using Battle.Combo;
using CardMaga.Card;
using Account.GeneralData;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaComboData : IEquatable<MetaComboData>
    {
        private ComboCore _comboCore;
        private ComboSO _comboSO;
        private BattleComboData _battleComboData;

        public BattleComboData BattleComboData => _battleComboData;
        
        public MetaComboData(ComboCore comboCore)
        {
            _comboCore = comboCore;
            _comboSO = _comboCore.ComboSO();
            _battleComboData = new BattleComboData(_comboSO,_comboCore.Level);
        }

        public int ID => _comboCore.ID;
        public int Level => _comboCore.Level;
        public CardTypeData[] ComboSequence => _comboSO.ComboSequence;
        public ComboSO ComboSO => _comboSO;
        public CardSO CraftedCard => _comboSO.CraftedCard;
        public ComboCore ComboCore => _comboCore;
        
        public bool Equals(MetaComboData other)
        {
            if (other == null)
                return false;
            
            return _comboCore.ID == other.ID;
        }
        
        public bool Equals(int comboCoreId)
        {
            return _comboCore.ID == comboCoreId;
        }
    }
}