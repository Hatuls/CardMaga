using System;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : BaseCollectionItemData<MetaCardData> , IEquatable<MetaCollectionCardData>,IEquatable<MetaCardData>
    {
        private MetaCardData _cardReference;

        public int CardId => _cardReference.CardInstance.ID;

        public override MetaCardData ItemReference => _cardReference;
        

        public MetaCollectionCardData(int numberOfInstant,MetaCardData cardReference)
        {
            _numberOfInstant = numberOfInstant;
            _cardReference = cardReference;
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