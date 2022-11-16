using CardMaga.InventorySystem;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCardUIContainer : BaseSlotContainer<MetaCardUI>
    {
        [SerializeField] private MetaCardUI _metaCardUI;
        public override BaseSlot<MetaCardUI> SlotType => _metaCardUI;
    }
}