using UnityEngine;
using System.Collections.Generic;
namespace Cards
{
    public class CardTypeComparer : IEqualityComparer<CardType>
    {

        // Products are equal if their names and product numbers are equal.
        public bool Equals(CardType x, CardType y)
        {


            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
                return true;


            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;



            //Check whether the products' properties are equal.
            return x._bodyPart == y._bodyPart && x._cardType == y._cardType;

        }


        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(CardType obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null))
                return 0;

            //Get hash code for the Name field if it is not null.
            int hashBodyPart = obj._bodyPart.GetHashCode();
            int hashCardType = obj._cardType.GetHashCode();
            int hash_rarityLevel = obj._rarityLevel.GetHashCode();

            return hashBodyPart ^ hashCardType ^ hash_rarityLevel;
        }

    }
}