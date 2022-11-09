using System;
using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : IEquatable<MetaCollectionCardData> ,IEqualityComparer<MetaCollectionCardData>
    {
        private int _cardId;
        private int _numberOfInstant;
        private MetaCardData _cardReference;

        public int CardId => _cardId;

        public int NumberOfInstant
        {
            get
            {
                return _numberOfInstant;
            }
            set
            {
                if (_numberOfInstant <= 0)
                {
                    return;
                }

                _numberOfInstant = value;
            }
        }

        public MetaCardData CardReference => _cardReference;


        public MetaCollectionCardData(int cardId,int numberOfInstant,MetaCardData cardReference)
        {
            _cardId = cardId;
            _numberOfInstant = numberOfInstant;
            _cardReference = cardReference;
        }

        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return _cardId == other._cardId;
        }

        public bool Equals(MetaCollectionCardData x, MetaCollectionCardData y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            return x.CardId == y.CardId;
        }

        public int GetHashCode(MetaCollectionCardData obj)
        {
            throw new NotImplementedException();
        }
    }
}