using System;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public abstract class BaseCollectionDataItem
    {
        #region Fields

        private readonly int _maxInstants;
        
        #endregion

        #region Props

        public abstract int NumberOfInstance { get; }
        
        public bool NotMoreInstants => NumberOfInstance <= 0;
        public bool MaxInstants => NumberOfInstance == _maxInstants;
        public int NumberOfCurrentInstance => NumberOfInstance;

        #endregion
    }
}