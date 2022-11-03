using Battle.Combo;
using CardMaga.Card;
using Account.GeneralData;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaComboData
    {
        private ComboCore _comboCore;
        private ComboSO _comboSO;


        public MetaComboData(ComboCore comboCore)
        {
            _comboCore = comboCore;
            _comboSO = _comboCore.ComboSO();
        }

        public int ID => _comboCore.ID;
        public int Level => _comboCore.Level;
        public CardTypeData[] ComboSequence => _comboSO.ComboSequence;
        public ComboSO ComboSO => _comboSO;
        public CardSO CraftedCard => _comboSO.CraftedCard;
        public ComboCore ComboCore => _comboCore;
    }
}