using System;
using CardMaga.UI;

namespace CardMaga.InventorySystem
{
    public class BaseSlot<T> : IInventoryObject<T> where T : BaseUIElement , IEquatable<T>
    {
        
        private T _inventoryObject;

        private bool _isHaveValue;

        //public bool IsHaveValue => _inventoryObject != null; //need this as main
        public bool IsHaveValue => _isHaveValue;

        public T InventoryObject => _inventoryObject;

        public void AssignValue(T InventoryObject)
        {
            if (IsHaveValue)
                return;

            _isHaveValue = true;
            _inventoryObject = InventoryObject;
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;
            
            _inventoryObject.Hide();
            _isHaveValue = false;
            //_inventoryObject = null; plaster!!! need to be able to null the object
        }

        public bool Contain(T other)
        {
            if (!_isHaveValue) return false;//plaster need to null the InventoryObject
            if (other == null) return false;
            //if (InventoryObject == null) return false;
            return InventoryObject.Equals(other);
        }
    }

   public interface IInventoryObject<T> where T : IEquatable<T>
       {
           T InventoryObject { get; }
       } 
}