using System;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public abstract class BaseCollectionDataItem
    {
        #region Fields

        protected int _maxInstants = 0;
        
        #endregion

        #region Props

        public abstract int NumberOfInstance { get; }
        
        public bool NotMoreInstants => NumberOfInstance <= 0;
        public bool MaxInstants => NumberOfInstance == _maxInstants;
        public int NumberOfCurrentInstance => NumberOfInstance;

        #endregion
    }
}