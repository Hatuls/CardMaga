using System.Collections.Generic;
using CardMaga.MetaUI.CollectionUI;

namespace CardMaga.MetaData.AccoutData
{
    public abstract class BaseFixSlotsContainer<T> where T : class
    {
        private BaseSlot<T>[] _slots;
        
        public BaseFixSlotsContainer(List<BaseSlot<T>> objects)
        {
            _slots = objects.ToArray();
        }
        
        public BaseFixSlotsContainer(BaseSlot<T>[] slots)
        {
            _slots = slots;
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
            _slots.Remove(obj);
        }
        
        public bool FindMetaDataObject(int objectID, out T metaCardData)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Equals(objectID))
                {
                    metaCardData = _slots[i].CollectionObject;
                    return true;
                }
            }

            metaCardData = null;
            return false;
        }
    }
}