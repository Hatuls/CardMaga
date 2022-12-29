using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionComboData :BaseCollectionDataItem , IEquatable<MetaCollectionComboData>,IEquatable<MetaComboData>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove item"; 
        
        public event Action<string> OnFailedAction;
        public event Action<MetaCollectionComboData> OnTryAddItemToCollection; 
        public event Action<MetaCollectionComboData> OnTryRemoveItemFromCollection;
        public event Action OnSuccessAddOrRemoveFromCollection;
        
        private MetaComboData _metaComboData;
        private List<int> _associateDeck;

        public MetaComboData MetaComboData => _metaComboData;

        public override int NumberOfInstance { get; }

        public int ComboID => _metaComboData.ID;
        
        public MetaCollectionComboData(MetaComboData comboReference)
        {
            _associateDeck = new List<int>();
            _metaComboData = comboReference;
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

        public bool Equals(MetaComboData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ComboID == other.ID;
        }
    }
}