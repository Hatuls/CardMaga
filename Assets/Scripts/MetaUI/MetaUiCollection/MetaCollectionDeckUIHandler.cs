using System.Collections.Generic;
using MetaUI.MetaCardUI;

namespace CardMaga.UI.MetaUI
{
    public class MetaCollectionDeckUIHandler
    {
        private const int MAX_CARD_IN_DRCK = 8;
        private const int MAX_COMBO_IN_DRCK = 3;
        private MetaCardUISlot[] _cardUISlots;
        private MetaComboUISlot[] _comboUISlots;

        public MetaCardUISlot[] CardUISlots => _cardUISlots;
        public MetaComboUISlot[] ComboUISlots => _comboUISlots;

        public MetaCollectionDeckUIHandler()
        {
            _cardUISlots = new MetaCardUISlot[MAX_CARD_IN_DRCK];
            _comboUISlots = new MetaComboUISlot[MAX_COMBO_IN_DRCK];
        }

        public void AddCardToSlot(MetaCardUI metaCardUI)
        {
            foreach (var slot in _cardUISlots)
            {
                if (!slot.IsHaveValue)
                    slot.AssignValue(metaCardUI);
            }
        }
        
        public void AddCardToSlot(List<MetaCardUI> metaCardUis)
        {
            for (int i = 0; i < _cardUISlots.Length; i++)
            {
                if (!_cardUISlots[i].IsHaveValue)
                    _cardUISlots[i].AssignValue(metaCardUis[i]);
            }
        }

        public void RemoveCardFromSlot(MetaCardUI metaCardUI)
        {
            foreach (var slot in _cardUISlots)
            {
                if (slot.MetaCardUI.Equals(metaCardUI))
                    slot.RemoveCard();
            }
        }
        
        public void AddComboToSlot(MetaComboUI metaComboUI)
        {
            foreach (var slot in _comboUISlots)
            {
                if (!slot.IsHaveValue)
                    slot.AssignValue(metaComboUI);
            }
        }
        
        public void AddComboToSlot(List<MetaComboUI> metaComboUI)
        {
            for (int i = 0; i < _comboUISlots.Length; i++)
            {
                if (!_comboUISlots[i].IsHaveValue)
                    _comboUISlots[i].AssignValue(metaComboUI[i]);
            }
        }

        public void RemoveComboFromSlot(MetaComboUI metaComboUI)
        {
            foreach (var slot in _comboUISlots)
            {
                if (slot.MetaComboUI.Equals(metaComboUI))
                    slot.RemoveCombo();
            }
        }
    }
}