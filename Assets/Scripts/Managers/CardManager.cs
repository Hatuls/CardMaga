
using System.Collections.Generic;
using UnityEngine;
using Cards;
namespace Managers
{

    public class CardManager : MonoSingleton<CardManager>
    {
        #region Fields
        [Header("All Cards In Game:")]
        [SerializeField] CardsCollectionSO _cardSOCollections;


        [SerializeField] int _amountOfCardsToStartCache=5;
        static Dictionary<CardNamesEnum, CardSO> _cardSOCollectionDict;
        static Dictionary<int, Card> _playerCardDict;
        static int _cardCreated;


        #endregion

        private void AssignCardSOCollectionDict()
        {
            if (_cardSOCollections == null || _cardSOCollections.GetAllCards == null || _cardSOCollections.GetAllCards.Length == 0)
            {
                Debug.LogError("CardManager : Unassigned Card Collection");
                return;
            }

            _cardSOCollectionDict = new Dictionary<CardNamesEnum, CardSO>();

            for (int i = 0; i < _cardSOCollections.GetAllCards.Length; i++)
            {
                if (!_cardSOCollectionDict.ContainsKey(_cardSOCollections.GetAllCards[i].GetCardName))
                _cardSOCollectionDict.Add(_cardSOCollections.GetAllCards[i].GetCardName, _cardSOCollections.GetAllCards[i]);
            }

        }

        private void AssignPlayerCardDict()
        {
            _playerCardDict = new Dictionary<int, Card>();


            for (int i = 0; i < _amountOfCardsToStartCache; i++)
                CreateCard(true,_cardSOCollections.GetAllCards[i].GetCardName);

            //check if to load from json file or start new one
        }





        public override void Init()
        {
            _cardCreated = 0;
            AssignCardSOCollectionDict();
            AssignPlayerCardDict();
        }





        public static Card CreateCard(bool toPlayer, CardNamesEnum card)
        {
            if (toPlayer)
            {
                if (_cardSOCollectionDict.TryGetValue(card, out CardSO val))
                {
                    Card cardCache = new Card(_cardCreated, val);

                    _playerCardDict.Add(_cardCreated, cardCache);

                    _cardCreated++;
                    Debug.Log("CardManager : Card Was created!");
                    return cardCache;
                }
            }
            else if (!toPlayer)
            {
                if (_cardSOCollectionDict.TryGetValue(card, out CardSO val))
                {
                    Card cardCache = new Card(_cardCreated, val);
                    _cardCreated++;
                    Debug.Log("CardManager : Card Was created!");
                    return cardCache;
                }
            }

            Debug.LogError("CardManager: No Such Thing as " + card.ToString() + " In Our Dictionary");
            return null;
        }
        public static bool CheckIfCardExistInDict(int id) {
            if (_playerCardDict == null)
            {
                Debug.LogError("CardManager: Dictionary not assigned");
                return false;
            }
            return _playerCardDict.ContainsKey(id); 
        }
        public static bool CheckIfCardExistInDict(CardNamesEnum card)
        {
            if (_playerCardDict == null)
            {
                Debug.LogError("CardManager: Dictionary not assigned");
                return false;
            }

            foreach (var item in _playerCardDict)
            {
                if (item.Value.GetSetCard.GetCardName == card)
                  return  CheckIfCardExistInDict(item.Value.GetCardID);
            }

            return false;
        }
        public static void RemoveCard(CardNamesEnum card)
        {
            if (_playerCardDict != null)
            {
                foreach (var _card in _playerCardDict)
                {
                    if (_card.Value.GetSetCard.GetCardName == card)
                        _playerCardDict.Remove(_card.Value.GetCardID);
                }
            }

        }
        public static void RemoveCard(int cardID)
        {
            if (_playerCardDict != null && CheckIfCardExistInDict(cardID))
                _playerCardDict.Remove(cardID);
        }
        public static Card[] GetPlayerDeck()
        {
            if (_playerCardDict == null)
                return null;

            Card[] deck = new Card[_playerCardDict.Count];

            for (int i = 0; i < deck.Length; i++)
                deck[i] = _playerCardDict[i];
            
            return deck;
        }
    }
}
