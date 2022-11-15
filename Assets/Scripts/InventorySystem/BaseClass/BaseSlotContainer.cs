using System;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public abstract class BaseSlotContainer<T> : BaseUIElement where T : BaseUIElement
    {
        private MBPool<BaseSlot<T>> _slotsPool;
        [SerializeField] private BaseSlot<T> _slotType;
        [SerializeField] private RectTransform _continerParent;
        [SerializeField,ReadOnly] protected BaseSlot<T>[] _slots;
        [SerializeField] private bool _isDynamic;
        [SerializeField] private int _numberOfInitializeSlots;
        [SerializeField] private bool _haveMaxValue;
        [SerializeField] private int _numberOfMaxlots;

        public int CollectionLength => _slots.Length;
        
        public void Awake()
        {
            _slots = new BaseSlot<T>[_numberOfInitializeSlots];
            _slotsPool = new MBPool<BaseSlot<T>>(_slotType,_continerParent,_numberOfInitializeSlots);
        }

        public void InitializeSlots(T[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if(TryAddObject(objects[i]))
                {
                    Debug.LogWarning("Failed to add object");
                }
            }
        }

        public bool TryAddObject(T obj)
        {
            for (int i = 0; i < CollectionLength; i++)
            {
                if (!_slots[i].IsHaveValue)
                {
                    _slots[i].AssignValue(obj);
                    return true;
                }
            }
            
            if (_isDynamic && (!_haveMaxValue || CollectionLength <= _numberOfMaxlots))
            {
                var cache = _slotsPool.Pull();
                cache.AssignValue(obj);
                ExpandSlots();
                _slots[_slots.Length - 1] = cache;
                return true;
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
            for (int i = 0; i < CollectionLength; i++)
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
            for (int i = 0; i < CollectionLength; i++)
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
            for (int i = 0; i < CollectionLength; i++)
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

        #region ArrayManament

        private void ExpandSlots()
        {
            Array.Resize(ref _slots,_slots.Length + 1);
        }
        
        #endregion
    }
}