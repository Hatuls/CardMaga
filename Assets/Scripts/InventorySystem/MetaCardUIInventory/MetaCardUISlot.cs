using CardMaga.InventorySystem;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCardUISlot : BaseSlot<MetaCardUI>
    {
        public MetaCardUISlot(int inventoryID) : base(inventoryID)
        {
        }

        public override void Hide()
        {
            base.Hide();
            CollectionObject.Hide();
        }
        
        
    }
}