using Cards;

using System.Collections.Generic;
using UnityEngine;

namespace Collections
{

    [CreateAssetMenu(fileName = ("CardsCollections"), menuName = ("ScriptableObjects/Collections/CardsCollections"))]
    public class CardsCollectionSO : ScriptableObject, IScriptableObjectCollection
    {
        [SerializeField]
        CardSO[] _cardCollection;

        private Dictionary<ushort, CardSO> _cardDict;

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


        public CardSO GetCard(ushort ID)
        {
            if (_cardDict == null)
                AssignDictionary();

            if (_cardDict.TryGetValue(ID, out CardSO card))
                return card;
            
            throw new System.Exception($"Card SO Could not been found from ID \nID is {ID}\nCheck Collection For card SO");
        }


        public async void AssignDictionary()
        {
            Debug.Log("<a>Started  Assiging Card Dictionary!!!</a>");

            const int Module = 50;
            int length = _cardCollection.Length;
            _cardDict = new Dictionary<ushort, CardSO>(length);

            for (int i = 0; i < length; i++)
            {
                var cardSO = _cardCollection[i];

                if (!_cardDict.ContainsKey(cardSO.ID))
                    _cardDict.Add(cardSO.ID, cardSO);
                else
                    throw new System.Exception($"Card CollectionSO : could not assign dictionary right\n there is 2 - cardSO {cardSO.CardName} with the same ID: {cardSO.ID}");

                if (i % Module == 0 && i > 0)
                    await System.Threading.Tasks.Task.Yield();
                
            }

            Debug.Log("<a>Finished Assiging Card Dictionary!!!</a>");
        }
    }

    public interface IScriptableObjectCollection
    {
        void AssignDictionary();
    }
}