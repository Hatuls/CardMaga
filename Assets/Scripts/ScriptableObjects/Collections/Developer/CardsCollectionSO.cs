using Account.GeneralData;
using CardMaga.Card;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
#endif
namespace Collections
{
    [CreateAssetMenu(fileName = ("CardsCollections"), menuName = ("ScriptableObjects/Collections/CardsCollections"))]
    public class CardsCollectionSO : ScriptableObject
    {
        [SerializeField]
        CardSO[] _cardCollection;
        [SerializeField]
        private RarityCardsContainer[] _rarityCardsContainer;
        public CardSO this[int cardSO]
        {
            get
            {
                for (int i = 0; i < _cardCollection.Length; i++)
                {
                    if (_cardCollection[i].ContainID(cardSO))
                        return _cardCollection[i];
                }
                throw new Exception("CardsCollectionSO: Card id was not found in collection\nID: " + cardSO);
            }
        }
        /*
         * 0 common
         * 1 uncommon
         * 2 rare 
         * 3 epic
         * 4 LegendRei
         */

        public RarityCardsContainer[] RarityCardsContainer => _rarityCardsContainer;
        public CardSO[] GetAllCardsSO
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
        public IEnumerable<int> AllCardsID
        {
            get
            {
                for (int i = 0; i < GetAllCardsSO.Length; i++)
                    foreach (var cardID in GetAllCardsSO[i].CardsID)
                        yield return cardID;
            }
        }
        public IEnumerable<CardCoreInfo> AllCardsCoreInfo
        {
            get
            {
                for (int i = 0; i < GetAllCardsSO.Length; i++)
                    foreach (var cardID in GetAllCardsSO[i].CardsCoreInfo)
                        yield return cardID;
            }
        }
#if UNITY_EDITOR
        public void Init(CardSO[] collection)
        {
            _cardCollection = collection;

            RarityEnum[] rarityValues = (RarityEnum[])Enum.GetValues(typeof(RarityEnum));
            _rarityCardsContainer = new RarityCardsContainer[rarityValues.Length-1];
            for (int i = 0; i < _rarityCardsContainer.Length; i++)
            {
                var current = rarityValues[i+1];
                if (current == RarityEnum.None)
                    continue;
                _rarityCardsContainer[i] = new RarityCardsContainer();
                List<int> cards = GetCardCore(_cardCollection.Where(x => x.Rarity == current).ToList());
                _rarityCardsContainer[i].AssignValues(current, cards);
            }

            List<int> GetCardCore(IReadOnlyList<CardSO> cardSOs)
            {
                List<int> cardCores = new List<int>();
                foreach (var card in cardSOs)
                {
                    for (int i =0; i < card.CardsMaxLevel; i++)
                        cardCores.Add(card.ID+i);
                }
                return cardCores;
            }
        }
#endif




    }

    [Serializable]
    public class RarityCardsContainer
    {
        [SerializeField] private RarityEnum _rarity;
        [SerializeField] private List<int> _cardsID;

        public RarityEnum Rarity => _rarity;
        public IReadOnlyList<int> CardsID => _cardsID;

#if UNITY_EDITOR
        public void AssignValues(RarityEnum rarityEnum, List<int> cardCores)
        {
            AssignCards( cardCores);
            AssignRarity(rarityEnum);
        }
        public void AssignRarity(RarityEnum rarityEnum)
            => _rarity = rarityEnum;
        public void AssignCards(List<int> cardCores)
            => _cardsID = cardCores;
#endif
    }

}