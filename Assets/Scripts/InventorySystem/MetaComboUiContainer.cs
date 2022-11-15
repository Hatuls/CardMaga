using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;

namespace InventorySystem
{
    public class MetaComboUiContainer : BaseFixSlotsContainer<MetaComboUI>
    {
        public MetaComboUiContainer(List<MetaComboUI> comboUis,int numberOfSlots) : base(numberOfSlots)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].AssignValue(comboUis[i]);
            }
        }
        
        protected override void InitializeSlots(int numberOfSlots)
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                _slots[i] = new MetaComboUISlot(GenerateInventoryID());
            }
        }
    }
}