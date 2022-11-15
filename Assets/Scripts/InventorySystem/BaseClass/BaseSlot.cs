using System;
using CardMaga.UI;

namespace CardMaga.InventorySystem
{
    public abstract class BaseSlot<T> : IUIElement, IInventoryObject , IEquatable<BaseSlot<T>>,IEquatable<int> where T : class
    {
        private T _collectionObject;

        public int InventoryID { get; }

        public bool IsHaveValue => _collectionObject != null;

        public T CollectionObject => _collectionObject;

        public BaseSlot(int inventoryID)
        {
            InventoryID = inventoryID;
        }
        
        public void AssignValue(T collectionObject)
        {
            if (IsHaveValue)
                return;
            
            _collectionObject = collectionObject;
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;

            _collectionObject = null;
        }

        public bool Equals(BaseSlot<T> other)
        {
            if (InventoryID == other.InventoryID)
            {
                return true;
            }

            return false;
        }

        public bool Equals(int inventoryID)
        {
            if (InventoryID == inventoryID)
            {
                return true;
            }

            return false;
        }

        #region UiElement

        public event Action OnInitializable;
        public event Action OnShow;
        public event Action OnHide;
        public virtual void Show()
        {
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            OnHide?.Invoke();
        }

        public virtual void Init()
        {
            OnInitializable?.Invoke();
        }

        #endregion

        public bool Contain(T other)
        {
            return CollectionObject.Equals(other);
        }
    }

    public interface IInventoryObject
    {
        int InventoryID { get; }
    }
}