using Cards;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName =("CardsCollections"), menuName =("ScriptableObjects/Collections/CardsCollections"))]
public class CardsCollectionSO : ScriptableObject
{
    [SerializeField]
    Cards.CardSO[] _cardCollection;



    /*
     * 0 common
     * 1 uncommon
     * 2 rare 
     * 3 epic
     * 4 LegendRei
     */

    [SerializeField]
    RarityCards[] _rarity;

    [System.Serializable]
    public class RarityCards
    {
        public RarityCards(ushort[] cards , RarityEnum rare)
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

    public void Init(CardSO[] collection)
    {
        int Rareity = System.Enum.GetNames(typeof(RarityEnum)).Length- 1;
        _rarity = new RarityCards[Rareity];

        _cardCollection = collection;
        List<ushort> cards;
        for (int i = 0; i < Rareity; i++)
        {
            cards = new List<ushort>();

            for (int j = 0; j < collection.Length; j++)
            {
                if (collection[j].Rarity == (RarityEnum)(i + 1))
                    cards.Add(collection[j].ID);
            }

            _rarity[i] = new RarityCards(cards.ToArray(), (RarityEnum)(i + 1));
        }


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


    public CardSO GetCard(ushort ID)
    {
        for (int i = 0; i < _cardCollection.Length; i++)
        {
            if (_cardCollection[i].ID == ID)
                return _cardCollection[i];
        }
        return null;
    }
}
