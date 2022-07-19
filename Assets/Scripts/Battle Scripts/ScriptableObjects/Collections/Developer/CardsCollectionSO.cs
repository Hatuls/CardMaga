using Cards;

using System.Collections.Generic;
using UnityEngine;

namespace Collections
{



    [CreateAssetMenu(fileName = ("CardsCollections"), menuName = ("ScriptableObjects/Collections/CardsCollections"))]
    public class CardsCollectionSO : ScriptableObject    {
        [SerializeField]
        CardSO[] _cardCollection;

     
        /*
         * 0 common
         * 1 uncommon
         * 2 rare 
         * 3 epic
         * 4 LegendRei
         */

        [SerializeField]
        RarityCards[] _rarity;
        public RarityCards[] CardsByRarity => _rarity;

        public RarityCards GetCardByRarity(RarityEnum rarity)
        {
            for (int i = 0; i < _rarity.Length; i++)
            {
                if (_rarity[i].Rarity == rarity)
                    return _rarity[i];
            }
            throw new System.Exception("Rarity was Not Valid or Rarity Cards variable was not start up correctly");
        }

        [System.Serializable]
        public class RarityCards
        {
            public RarityCards(ushort[] cards, RarityEnum rare)
            {
                _rarity = rare;
                _cardsID = cards;
            }

            [SerializeField]
            RarityEnum _rarity;
            public RarityEnum Rarity => _rarity;

            [SerializeField] ushort[] _cardsID;
            public ushort[] CardsID => _cardsID;
        }

        public void Init(CardSO[] collection, RarityCards[] rarityCards)
        {
            _cardCollection = collection;
            _rarity = rarityCards;
        }

        public CardSO[] GetAllCards
        {
            get
            {
                if (_cardCollection != null)
                    return _cardCollection;
                else
                {
                    Debug.LogError("Error Getting All Cards");
                    return null;
                }
            }
        }


       
    }

  
}