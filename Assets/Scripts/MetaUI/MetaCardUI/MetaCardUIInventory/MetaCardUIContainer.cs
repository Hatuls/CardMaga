using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCardUIContainer : BaseSlotContainer<MetaCardUI>
    {
        [SerializeField] private MetaCardUI _metaCardUI;
        public override BaseSlot<MetaCardUI> SlotType => _metaCardUI;
    }
}