using System;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public abstract class BaseCollectionDataItem
    {
        #region Fields

        private readonly int _maxInstants;

        protected int numberOfCurrentInstance;

        #endregion

        #region Props

        public abstract int NumberOfInstance { get; }
        
        public bool NotMoreInstants => NumberOfInstance <= 0;
        public bool MaxInstants => numberOfCurrentInstance == _maxInstants;
        public int NumberOfCurrentInstance => numberOfCurrentInstance;

        #endregion

        public void AddItemToCollection()
        {
            numberOfCurrentInstance--;
        }
        
        public void RemoveItemFromCollection()
        {
            numberOfCurrentInstance++;
        }
    }
}