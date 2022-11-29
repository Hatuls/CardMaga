using System;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public class BaseSlot<T> : IInventoryObject<T> where T : BaseUIElement , IEquatable<T>
    {
        public event Action<BaseSlot<T>> OnDisposed;
        public event Action<T> OnAddInventoryObject; 
        public event Action<T> OnRemoveInventoryObject; 
        
        private T _inventoryObject;

        public bool IsHaveValue => _inventoryObject != null;

        public T InventoryObject => _inventoryObject;

        public void AssignValue(T InventoryObject)
        {
            if (IsHaveValue)
                return;
            
            _inventoryObject = InventoryObject;
            OnAddInventoryObject?.Invoke(_inventoryObject);
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;
            _inventoryObject.Hide();
            OnRemoveInventoryObject?.Invoke(_inventoryObject);
            _inventoryObject = null;
        }

        public bool Contain(T other)
        {
            if (other == null) return false;
            if (InventoryObject == null) return false;
            return InventoryObject.Equals(other);
        }

        public virtual void Dispose()
        {
            OnDisposed?.Invoke(this);
        }
    }

   public interface IInventoryObject<T> where T : IEquatable<T>
       {
           T InventoryObject { get; }
       } 
}