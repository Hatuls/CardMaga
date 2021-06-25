﻿using Cards;
using System.Collections.Generic;
using UnityEngine;
using Battles.UI;

namespace Battles.Deck
{
    public class DeckManager : MonoSingleton<DeckManager>
    {
        #region Fields
        
        #region Public Fields
        #endregion
        #region Private Fields
        private Dictionary<DeckEnum, DeckAbst> _decksDict;

        [SerializeField] private Card[] _deck;

      

        [SerializeField] int _playerMaxHandSize;
        [SerializeField] int _playerStartingHandSize;
        [SerializeField] int _placementSize;
        #endregion
        #endregion
        #region Properties
        public Card[] GetSetDeck
        {
            set
            {
                if (value == null)
                {
                    Debug.LogError("Error tried to set null as the player deck!!");
                    return;
                }

                if (_deck != value)
                   _deck = value;
            }
            get
            {
                if (_deck == null || _deck.Length == 0)
                {
                    _deck = Managers.CardManager.GetPlayerDeck();
                    if (_deck == null)
                     Debug.LogError("Error returning null deck or length 0");
                    
                }

                Card[] copyArray = new Card[_deck.Length];
                _deck.CopyTo(copyArray, 0);
                System.Array.Copy(copyArray,_deck , _deck.Length);

                return copyArray;
            }
        }
        #endregion

        #region Functions

        #region Public Functions
        public override void Init()
        {
            //ResetDeckManager();

        }
        public void ResetDeckManager()
        {
            _deck = Managers.CardManager.GetPlayerDeck();
            InitDecks();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                 TransferCard(DeckEnum.PlayerDeck, DeckEnum.Hand, GetDeckAbst(DeckEnum.PlayerDeck).GetFirstCard());

            else if(Input.GetKeyDown(KeyCode.Alpha2))
                TransferCard(DeckEnum.Hand, DeckEnum.Placement, GetDeckAbst(DeckEnum.Hand).GetFirstCard());

          else  if (Input.GetKeyDown(KeyCode.Alpha3))
                TransferCard(DeckEnum.Disposal, DeckEnum.PlayerDeck, GetDeckAbst(DeckEnum.Disposal).GetFirstCard());

            else if (Input.GetKeyDown(KeyCode.Alpha4))
                TransferCard(DeckEnum.Placement, DeckEnum.Disposal, GetDeckAbst(DeckEnum.Placement).GetFirstCard());   

        }
        public Card[] GetCardsFromDeck(DeckEnum from)
        {
            var cache = GetDeckAbst(from);
            if (cache == null)
                return null;

                return cache.GetDeck;
        }
        public Card GetCardFromDeck(int index, DeckEnum from) {

            var cache = GetDeckAbst(from);

            if (cache == null || cache.GetDeck.Length <= 0)
                return null;
            else if (index >= 0 && index < cache.GetDeck.Length)
                return cache.GetDeck[index];

            return null;
        }
        
        public void DrawHand(int drawAmount)
        {
            /*
             * check if everything is valid
             * cache the relevante decks (hand and player deck)
             * for each card we draw :
             * if there is still cards in the deck we want to transfer them to the hand one by one
             * if we found a card that is null its mean the deck is empty so we want to restore the cards from 
             * the disposal deck and redraw the amount we need
            */


            if (_decksDict == null || _decksDict.Count == 0)
            {
                Debug.LogError("DeckManager : Deck Dictionary was not assignd!");
                return;
            }
            else if (drawAmount < 1)
            {
                Debug.LogError("DeckManager :Cannot draw - draw amount is less than 1!");
                return;
            }

            DeckAbst fromDeck = _decksDict[DeckEnum.PlayerDeck];
            DeckAbst toDeck = _decksDict[DeckEnum.Hand];
            Card cardCache;


            if (_decksDict[DeckEnum.PlayerDeck] != null)
            {
                for (int i = 0; i < drawAmount; i++)
                {
                    cardCache = fromDeck.GetFirstCard();

                    if (cardCache == null)
                    {
                        _decksDict[DeckEnum.Disposal].ResetDeck();

                        cardCache = fromDeck.GetFirstCard();
                    }

                    if (cardCache != null)
                    {
                        toDeck.AddCard(cardCache);
                        fromDeck.DiscardCard(cardCache);
                        CardUIManager.Instance.DrawCards(cardCache, DeckEnum.PlayerDeck, 1);
                    }
                    else
                        Debug.LogError("DeckManager: The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card");


                }
            }
        }

        public DeckEnum IntToDeck(int deckID)
        {
            switch (deckID)
            {
             
                case 0:
                    return DeckEnum.PlayerDeck;
                case 1: 
                   return DeckEnum.Hand;
                case 2:
                    return DeckEnum.Disposal;
                case 3:
                    return DeckEnum.Exhaust;
                case 4:
                    return DeckEnum.Placement;
            }

            return 0;
        }
        public void OnEndTurn()
        {
            Debug.Log("Discarding the remain cards from hand and placement!");
            GetDeckAbst(DeckEnum.Placement).ResetDeck();
            GetDeckAbst(DeckEnum.Hand).ResetDeck();
        }
  

        #endregion

        #region Private Functions   
        public void TransferCard(DeckEnum from, DeckEnum to, Card card) {

            if (card == null && !GetDeckAbst(from).IsTheCardInDeck(card))
                return;

            var fromDeck = GetDeckAbst(from);
            var toDeck = GetDeckAbst(to);

            if (to == DeckEnum.Placement && toDeck.GetAmountOfFilledSlots == _placementSize)
            {
                Debug.LogError("DeckManager : All Slots In Placement Is Already Filled Tranfer Cancel");
                return;
            }
            else if (to == DeckEnum.Hand && toDeck.GetAmountOfFilledSlots == _playerMaxHandSize)
            {
                Debug.LogError("DeckManager : Max Card Number Limit Cancel Drawing Cards");

            }




            toDeck.AddCard( card);
            fromDeck.DiscardCard(card);
            //fromDeck.PrintDecks(from);
          //  toDeck.PrintDecks(to);
        }
        public void TransferCard(DeckEnum from, DeckEnum to, Card card, int Index)
        {

            if (card == null && !GetDeckAbst(from).IsTheCardInDeck(card))
                return;

            var fromDeck = GetDeckAbst(from);
            var toDeck = GetDeckAbst(to);

            if (fromDeck != toDeck)
            {
                if (to == DeckEnum.Placement && toDeck.GetAmountOfFilledSlots == _placementSize)
                {
                    Debug.LogError("DeckManager : All Slots In Placement Is Already Filled Transfer Cancel");
                    return;
                }
                else if (to == DeckEnum.Hand && toDeck.GetAmountOfFilledSlots == _playerMaxHandSize)
                {
                    Debug.LogError("DeckManager : Max Card Number Limit Cancel Drawing Cards");

                }
            }


            if (from == DeckEnum.Placement)
                (fromDeck as Placements).DiscardCard(card, Index);
            else
                fromDeck.DiscardCard(card);

            if (to == DeckEnum.Placement)
                (toDeck as Placements).AddCard(card, Index);
            else
                toDeck.AddCard(card);

            //fromDeck.PrintDecks(from);
            //  toDeck.PrintDecks(to);
        }
        private void TransferCard(DeckEnum from, DeckEnum to, int amount)
        {
            if (amount <= 0 || from == to)
                return;

            DeckAbst fromDeckCache = GetDeckAbst(from);
            DeckAbst toDeckCache = GetDeckAbst(to);

            if (fromDeckCache.GetAmountOfFilledSlots >= amount)
            {

                for (int i = 0; i < amount; i++)
                {
                    toDeckCache.AddCard(toDeckCache.GetFirstCard());
                    fromDeckCache.DiscardCard(fromDeckCache.GetFirstCard());
                    // ui Transfer
                }
            }
            else
            {
                int remainTransfer = amount - fromDeckCache.GetAmountOfFilledSlots;

                 for (int i = 0; i < fromDeckCache.GetAmountOfFilledSlots; i++)
                {

                    toDeckCache.AddCard(toDeckCache.GetFirstCard());
                    fromDeckCache.DiscardCard(fromDeckCache.GetFirstCard());

                  

                 // ui Transfer
                }
                if (from == DeckEnum.PlayerDeck)
                    RefillDeck(from);



                TransferCard(from, to, remainTransfer);
            }
           




        }
        private void DiscardCard(DeckEnum discardFrom, DeckEnum discardTo, CardSO card) { }
        private void DiscardCard(DeckEnum discardFrom, DeckEnum discardTo, int amount) { }
        private void RefillDeck(DeckEnum deck)
        {
            switch (deck)
            {
                case DeckEnum.PlayerDeck:
                    GetDeckAbst(deck).SetDeck = GetDeckAbst(DeckEnum.Disposal).GetDeck;
                    GetDeckAbst(DeckEnum.Disposal).ResetDeck();
                    break;

                case DeckEnum.Disposal:
                    GetDeckAbst(DeckEnum.Disposal).ResetDeck();
                    break;

                case DeckEnum.Exhaust:
                    GetDeckAbst(DeckEnum.Exhaust).ResetDeck();
                    break;

                case DeckEnum.Placement:
                    break;
                case DeckEnum.Hand:
                default:
                    break;
            }

        }
        private DeckAbst GetDeckAbst(DeckEnum deck)
        {
            if (_decksDict != null && _decksDict.TryGetValue(deck, out DeckAbst deckAbst))
                return deckAbst;

            Debug.LogError("DeckManager Didnt find the " + deck.ToString() + " deck");
            return null;
        }
        private void InitDecks()
        {
            // assing dictionary if its not assigned and then reset all the decks

            if (_decksDict == null)
            {
              _decksDict = new Dictionary<DeckEnum, DeckAbst>();

            _decksDict.Add(DeckEnum.PlayerDeck, new PlayerDeck(GetSetDeck));
            _decksDict.Add(DeckEnum.Exhaust, new Exhaust(_playerMaxHandSize));
            _decksDict.Add(DeckEnum.Disposal,new Disposal(GetSetDeck.Length , _decksDict[DeckEnum.PlayerDeck] as PlayerDeck));
            _decksDict.Add(DeckEnum.Hand, new PlayerHand(_playerStartingHandSize, _decksDict[DeckEnum.Disposal] as Disposal));
            _decksDict.Add(DeckEnum.Placement, new Placements (_placementSize, _decksDict[DeckEnum.Disposal] as Disposal,_decksDict[DeckEnum.Hand] as PlayerHand ));
            }

            ResetDecks();

        }

        private void ResetDecks()
        {
            // reset the decks

            if (_decksDict == null)
            {
                Debug.LogError("DeckManager : Deck Dictionary was not assigned!");
                return;
            }


            foreach (var item in _decksDict)
            {
                if (!(item.Value is  PlayerDeck))
                    item.Value.EmptySlots();
                else
                    _decksDict[DeckEnum.PlayerDeck].ResetDeck();

            }
               

            
        }
        #endregion


        #endregion
    }
}