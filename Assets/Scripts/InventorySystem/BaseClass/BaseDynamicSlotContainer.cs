using System.Collections.Generic;
using CardMaga.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public abstract class BaseDynamicSlotContainer<T> : MonoBehaviour where T : BaseUIElement
    {
        [SerializeField,ReadOnly] protected List<BaseSlot<T>> _slots;

        public BaseDynamicSlotContainer(int numberOfSlots)//need to remove!@#!@
        {
            _slots = new List<BaseSlot<T>>(numberOfSlots);
            
            InitializeSlots(numberOfSlots);
        }

        protected abstract void InitializeSlots(int numberOfSlots);
        
        public bool TryAddObject(T obj)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsHaveValue)
                {
                    _slots[i].AssignValue(obj);
                    return true;
                }
            }

            return false;
        }

        public void RemoveObject(T obj)
        {
            if (TryFindBaseSlot(obj, out BaseSlot<T> baseSlot))
            {
                baseSlot.RemoveValue();
            }

            Debug.LogWarning( typeof(T).Name + "Not in the container " + this);
        }

        #region FindCollectionObject

        public bool TryFindCollectionObject(BaseSlot<T> otherSlot, out T obj)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].Equals(otherSlot))
                {
                    obj = _slots[i].InventoryObject;
                    return true;
                }
            }

            obj = null;
            return false;
        }

        public bool Contain(T collectionObject)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].Contain(collectionObject))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
        
        #region FindBaseSlot
        
        private bool TryFindBaseSlot(T collectionObject, out BaseSlot<T> outSlot)
        {
            for (int i = 0; i < _slots.Count; i++)
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
    }
}