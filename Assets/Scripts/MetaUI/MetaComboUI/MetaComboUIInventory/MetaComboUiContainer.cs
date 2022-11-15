using CardMaga.MetaUI;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public class MetaComboUiContainer : BaseSlotContainer<MetaComboUI>
    {
        
        public void AddComboToSlot(MetaComboUI metaComboUI)
        {
            if (!TryAddObject(metaComboUI))
                Debug.LogWarning("Failed to add object");
        }
        
        public void RemoveComboFromSlot(MetaComboUI metaComboUI)
        {
            RemoveObject(metaComboUI);
        }
    }
}
