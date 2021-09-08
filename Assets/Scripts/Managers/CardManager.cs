﻿
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
        static Dictionary<string, CardSO> _cardSOCollectionDict;
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

            _cardSOCollectionDict = new Dictionary<string, CardSO>();

            for (int i = 0; i < _cardSOCollections.GetAllCards.Length; i++)
            {
                if (!_cardSOCollectionDict.ContainsKey(_cardSOCollections.GetAllCards[i].CardName))
                _cardSOCollectionDict.Add(_cardSOCollections.GetAllCards[i].CardName, _cardSOCollections.GetAllCards[i]);
            }

        }

        public void AssignPlayerCardDict(Card[] _playerCards)
        {
            if (_playerCardDict==null)
                        _playerCardDict = new Dictionary<int, Card>();


            for (int i = 0; i < _playerCards.Length; i++)
            {
                if (_playerCardDict.ContainsKey(_playerCards[i].CardID) == false)
                                _playerCardDict.Add(_playerCards[i].CardID, _playerCards[i]);
            }
            //check if to load from json file or start new one
        }





        public override void Init()
        {
            _cardCreated = 0;
            AssignCardSOCollectionDict();
           
        }





        public static Card CreateCard(bool toPlayer, string card)
        {
            if (toPlayer)
            {
                if (_cardSOCollectionDict.TryGetValue(card, out CardSO val))
                {
                    Card cardCache = new Card(_cardCreated, val);
                    if (_playerCardDict == null)
                        _playerCardDict = new Dictionary<int, Card>();

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
        public static bool CheckIfCardExistInDict(string card)
        {
            if (_playerCardDict == null)
            {
                Debug.LogError("CardManager: Dictionary not assigned");
                return false;
            }

            foreach (var item in _playerCardDict)
            {
                if (item.Value.CardSO.CardName == card)
                  return  CheckIfCardExistInDict(item.Value.CardID);
            }

            return false;
        }
        public static void RemoveCard(string card)
        {
            if (_playerCardDict != null)
            {
                foreach (var _card in _playerCardDict)
                {
                    if (_card.Value.CardSO.CardName == card)
                        _playerCardDict.Remove(_card.Value.CardID);
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
