using System;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCollectionDataCombo :BaseCollectionDataItem<MetaComboData> , IEquatable<MetaCollectionDataCombo>,IEquatable<MetaComboData>
    {
        private MetaComboData _metaComboData;
        public override MetaComboData ItemReference => _metaComboData;
        
        public int ComboID => _metaComboData.ID;
        
        public MetaCollectionDataCombo(int numberOfInstant,MetaComboData comboReference)
        {
            _numberOfInstant = numberOfInstant;
            _metaComboData = comboReference;
        }
        
        public bool Equals(MetaCollectionDataCombo other)
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