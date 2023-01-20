using System;
using CardMaga.UI;

namespace CardMaga.InventorySystem
{
    public class BaseSlot<T> : IInventoryObject<T> where T : BaseUIElement , IEquatable<T>
    {
        private T _inventoryObject;
        
        public bool IsHaveValue => _inventoryObject != null; //need this as main

        public T InventoryObject => _inventoryObject;

        public void AssignValue(T inventoryObject)
        {
            if (IsHaveValue)
                return;

            _inventoryObject = inventoryObject;
            _inventoryObject.Show();
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;
            
            _inventoryObject.Hide(); 
            _inventoryObject = null;
        }

        public bool Contain(T other)
        {
            if (!IsHaveValue) return false;//plaster need to null the inventoryObject
            if (other == null) return false;
            return _inventoryObject.Equals(other);
        }
    }

   public interface IInventoryObject<T> where T : IEquatable<T>
       {
           T InventoryObject { get; }
       } 
}