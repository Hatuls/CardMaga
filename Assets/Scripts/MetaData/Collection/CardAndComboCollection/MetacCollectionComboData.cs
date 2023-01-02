using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionComboData :BaseCollectionDataItem , IEquatable<MetaCollectionComboData>,IEquatable<ComboCore>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove item"; 
        
        public event Action<string> OnFailedAction;
        public event Action<MetaCollectionComboData> OnTryAddItemToCollection; 
        public event Action<MetaCollectionComboData> OnTryRemoveItemFromCollection;
        public event Action OnSuccessAddOrRemoveFromCollection;
        
        private ComboCore _comboData;
        private List<int> _associateDeck;

        public ComboCore ComboData => _comboData;

        public override int NumberOfInstance { get; }

        public int ComboID => _comboData.ID;
        
        public MetaCollectionComboData(ComboCore comboReference)
        {
            _associateDeck = new List<int>();
            _comboData = comboReference;
        }

        public void AddComboToCollection()
        {
            OnTryAddItemToCollection?.Invoke(this);
        }

        public void RemoveComboFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(this);
        }
        
        public bool Equals(MetaCollectionComboData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ComboID == other.ComboID;
        }

        public bool Equals(ComboCore other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ComboID == other.ID;
        }
    }
}