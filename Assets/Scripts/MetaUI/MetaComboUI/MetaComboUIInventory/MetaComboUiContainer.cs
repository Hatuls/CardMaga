using CardMaga.MetaUI;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public class MetaComboUiContainer : BaseSlotContainer<MetaComboUI>
    {
        [SerializeField] private MetaComboUI _metaComboUI;
        public override BaseSlot<MetaComboUI> SlotType => _metaComboUI;
    }
}
