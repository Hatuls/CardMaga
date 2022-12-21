using System;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionComboData :BaseCollectionDataItem<MetaComboData> , IEquatable<MetaCollectionComboData>,IEquatable<MetaComboData>
    {
        private MetaComboData _metaComboData;
        public override MetaComboData ItemReference => _metaComboData;
        
        public int ComboID => _metaComboData.ID;
        
        public MetaCollectionComboData(int numberOfInstant,int numberOfMaxInstants,MetaComboData comboReference) : base(numberOfInstant,numberOfMaxInstants)
        {
            _metaComboData = comboReference;
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