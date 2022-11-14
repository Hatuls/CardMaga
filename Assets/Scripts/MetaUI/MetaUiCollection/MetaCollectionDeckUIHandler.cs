using System.Collections.Generic;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCollectionDeckUIHandler
    {
        private const int MAX_CARD_IN_DRCK = 8;
        private const int MAX_COMBO_IN_DRCK = 3;
        private BaseSlot<MetaCardUI>[] _cardUISlots;
        private BaseSlot<MetaComboUI>[] _comboUISlots;

        public BaseSlot<MetaCardUI>[] CardUISlots => _cardUISlots;
        public BaseSlot<MetaComboUI>[] ComboUISlots => _comboUISlots;

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
                if (slot.CollectionObject.Equals(metaCardUI))
                    slot.RemoveValue();
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
                if (slot.CollectionObject.Equals(metaComboUI))
                    slot.RemoveValue();
            }
        }
    }
}