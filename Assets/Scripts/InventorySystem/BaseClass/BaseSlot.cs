using System;
using CardMaga.UI;

namespace CardMaga.InventorySystem
{
    public abstract class BaseSlot<T> : BaseUIElement, IInventoryObject<T> where T : BaseUIElement
    {
        public event Action<IInventoryObject<T>> OnAddInventoryObject; 
        public event Action<IInventoryObject<T>> OnRemoveInventoryObject; 
        
        private T _inventoryObject;

        public bool IsHaveValue => _inventoryObject != null;

        public T InventoryObject => _inventoryObject;

        public void AssignValue(T InventoryObject)
        {
            if (IsHaveValue)
                return;
            
            _inventoryObject = InventoryObject;
            OnAddInventoryObject?.Invoke(this);
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;

            _inventoryObject = null;
            OnRemoveInventoryObject?.Invoke(this);
        }

        public bool Contain(T other)
        {
            if (other == null) return false;
            if (InventoryObject == null) return false;
            return InventoryObject.Equals(other);
        }
    }

   public interface IInventoryObject<T>
       {
           T InventoryObject { get; }
       } 
}