using System;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.InventorySystem
{
    public abstract class BaseSlotContainer<T> : MonoBehaviour where T : BaseUIElement , IEquatable<T>
    {
        private BaseSlot<T>[] _slots;
        [SerializeField] private RectTransform _continerParent;
        [Header("Container Configuration")]
        [SerializeField,Tooltip("Can the Container grow dynamically")] private bool _isDynamic;
        [SerializeField,Tooltip("Number of start slot")] private int _numberOfInitializeSlots;
        [SerializeField,Tooltip("Defines whether there is a maximum value for the slots")] private bool _haveMaxValue;
        [SerializeField,Tooltip("The maximum value of slots")] private int _numberOfMaxlots;

        private int CollectionLength => _slots.Length;
        
        public void Awake()
        {
            _slots = new BaseSlot<T>[_numberOfInitializeSlots];

            for (int i = 0; i < _numberOfInitializeSlots; i++)
            {
                _slots[i] = new BaseSlot<T>();
            }
        }

        public void InitializeSlots(T[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                _slots[i].AssignValue(objects[i]);
                objects[i].transform.SetParent(_continerParent);
            }
        }

        public bool TryAddObject(T obj)
        {
            for (int i = 0; i < CollectionLength; i++)
            {
                if (!_slots[i].IsHaveValue)
                {
                    _slots[i].AssignValue(obj);
                    obj.transform.SetParent(_continerParent);
                    return true;
                }
            }
            
            if (_isDynamic && (!_haveMaxValue || CollectionLength <= _numberOfMaxlots))
            {
                var cache = new BaseSlot<T>();
                cache.AssignValue(obj);
                obj.transform.SetParent(_continerParent);
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

        public bool GetEmptySlot(out BaseSlot<T> baseSlot)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsHaveValue)
                {
                    baseSlot = slot;
                    return true;
                }
            }

            baseSlot = null;
            return false;
        }
        
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