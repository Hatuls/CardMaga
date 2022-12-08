using System;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : BaseCollectionDataItem<MetaCardData> , IEquatable<MetaCollectionCardData>,IEquatable<MetaCardData>
    {
        private MetaCardData _metaCardData;

        public int CardId => _metaCardData.CardInstance.ID;

        public override MetaCardData ItemReference => _metaCardData;
        

        public MetaCollectionCardData(int numberOfInstants,int numberOfMaxInstants,MetaCardData metaCardData)
        {
            _numberOfInstant = numberOfInstants;
            _metaCardData = metaCardData;
            _maxInstants = numberOfMaxInstants;
        }

        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardId == other.CardId;
        }

        public bool Equals(MetaCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardId == other.CardInstance.ID;
        }
    }
}