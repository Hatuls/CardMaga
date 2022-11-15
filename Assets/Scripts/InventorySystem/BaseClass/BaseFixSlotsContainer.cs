using System;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public abstract class BaseFixSlotsContainer<T> : IUIElement where T : class
    {
        private int _idsCount;
        
        protected BaseSlot<T>[] _slots;

        public BaseFixSlotsContainer(int numberOfSlots)
        {
            _idsCount = 0;
            _slots = new BaseSlot<T>[numberOfSlots];
            
            InitializeSlots(numberOfSlots);
        }

        protected abstract void InitializeSlots(int numberOfSlots);
        
        protected int GenerateInventoryID()
        {
            _idsCount++;
            return _idsCount;
        }

        public bool AddObject(T obj)
        {
            bool isSuccessful = false;
            
            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].IsHaveValue)
                {
                    _slots[i].AssignValue(obj);
                    isSuccessful = true;
                }
            }

            return isSuccessful;
        }

        public void RemoveObject(T obj)
        {
            if (FindBaseSlot(obj, out BaseSlot<T> baseSlot))
            {
                baseSlot.RemoveValue();
            }

            Debug.LogWarning( typeof(T).Name + "Not in the container " + this);
        }
        
        public void RemoveObject(int inventoryID)
        {
            if (FindBaseSlot(inventoryID, out BaseSlot<T> baseSlot))
            {
                baseSlot.RemoveValue();
            }

            Debug.LogWarning( "InventoryID: " + inventoryID + " Not associated with any slot in " + this);
        }

        #region FindCollectionObject

        public bool FindCollectionObject(BaseSlot<T> otherSlot, out T obj)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Equals(otherSlot))
                {
                    obj = _slots[i].CollectionObject;
                    return true;
                }
            }

            obj = null;
            return false;
        }
        
        public bool FindCollectionObject(int inventoryID, out T obj)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Equals(inventoryID))
                {
                    obj = _slots[i].CollectionObject;
                    return true;
                }
            }

            obj = null;
            return false;
        }
        
        public bool FindCollectionObject(T collectionObject, out T obj)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Contain(collectionObject))
                {
                    obj = _slots[i].CollectionObject;
                    return true;
                }
            }

            obj = null;
            return false;
        }

        #endregion
        
        #region FindBaseSlot

        private bool FindBaseSlot(BaseSlot<T> slot, out BaseSlot<T> outSlot)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Equals(slot))
                {
                    outSlot = _slots[i];
                    return true;
                }
            }

            outSlot = null;
            return false;
        }
        
        private bool FindBaseSlot(int inventoryID, out BaseSlot<T> outSlot)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Equals(inventoryID))
                {
                    outSlot = _slots[i];
                    return true;
                }
            }

            outSlot = null;
            return false;
        }
        
        private bool FindBaseSlot(T collectionObject, out BaseSlot<T> outSlot)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Contain(collectionObject))
                {
                    outSlot = _slots[i];
                    return true;
                }
            }

            outSlot = null;
            return false;
        } 

        #endregion
        
        #region UIElement

        public event Action OnInitializable;
        public event Action OnShow;
        public event Action OnHide;
        
        public virtual void Init()
        {
            OnInitializable?.Invoke();
        }

        public virtual void Show()
        {
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            OnHide?.Invoke();
        }

        #endregion
    }
}