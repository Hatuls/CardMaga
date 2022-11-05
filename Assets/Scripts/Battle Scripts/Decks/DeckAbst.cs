﻿using Cards;
using System;
namespace Battles.Deck
{
    public abstract class DeckAbst : IDeckHandler
    {
        public bool isPlayer { get; private set; }
        private Card[] _deckCards;

        private int amountOfEmptySlots;
        private int amountOfFilledSlots;

        #region Properties
        public int GetAmountOfEmptySlots { get { return amountOfEmptySlots; } }
        public  int GetAmountOfFilledSlots =>  amountOfFilledSlots;
        public ref Card[] GetDeck
            => ref _deckCards; 
        
        public Card[] SetDeck
        {
            
            set
            {

                if (value != null)
                    _deckCards = value;

                    CountCards();
                
                OrderDeck();
            }
        }
        #endregion

        #region Public Functions
        public DeckAbst(bool isPlayer, Card[] deckCards)
        {
            SetDeck = deckCards;
            this.isPlayer = isPlayer;

        }
        public DeckAbst(bool isPlayer,int length)
        {
            this.isPlayer = isPlayer;
            InitDeck(length);
        }

        public void InitDeck(int length)
        {
            // assign new deck in the legnth we want

            SetDeck = new Card[length];
        }

        protected void CountCards()
        {
            // count for the amount of empty and not empty slots 


            amountOfEmptySlots = 0;
            amountOfFilledSlots = 0;

            if (_deckCards == null)
            {
                UnityEngine.Debug.LogError("DeckCards is null");
                return;
            }

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i] == null)
                    amountOfEmptySlots++;
                else
                    amountOfFilledSlots++;
            }
        }
        public virtual Card GetFirstCard()
        {
            // return the Card from the first slot in the array


            if (_deckCards != null && _deckCards.Length > 0)
                return _deckCards[0];

            return null;
        }
        public virtual bool AddCard(Card card)
        {
            //  add card to the deck
            // if its not assigned then create a deck of 5 cards
            // search for the first empty spots
            // order the deck so there wont be gaps between cards and nulls
            // and recount the amounts

            if (_deckCards == null || _deckCards.Length == 0)
                InitDeck(4);

            if (ExpandingDeckPolicy())
            {
                int index = GetIndexOfFirstNull();
                _deckCards[index] = card;

                OrderDeck();
                amountOfEmptySlots--;
                amountOfFilledSlots++;
                //CountCards();
                return true;
            }
            return false;
        }
        public virtual bool ExpandingDeckPolicy() => true;
        public virtual bool DiscardCard(in Card card)
        {
            /*
             * check if the deck and card is valids
             * loop throught the decks and search this card by matching id between the card in the deck and the one we check
             * if we found then reset him to null 
             * and reoder and recount
             */
            bool foundCard= false;

            if (_deckCards != null && card != null && _deckCards.Length > 0)
            {
                for (int i = 0; i < _deckCards.Length; i++)
                {
                    if (_deckCards[i] != null
                       && _deckCards[i].CardInstanceID == card.CardInstanceID)
                    {
                        _deckCards[i] = null;
                        amountOfEmptySlots++;
                        amountOfFilledSlots--;
                        foundCard = true;
                        break;
                    }

                }
            }
            OrderDeck();
            return foundCard;
        }
        public void EmptySlots()
        {
            /*
             * Each slot that has something we want to instantly Remove it
             * 
             */
            if (_deckCards == null)
            {
                UnityEngine.Debug.LogError(this.ToString() + ": Tried to reset deck and the deck cards is null!");
                return;
            }

            if (GetDeck.Length > 0)
            {
                for (int i = 0; i < GetDeck.Length; i++)
                {
                    if (GetDeck[i] != null)
                        GetDeck[i] = null;
                }
            }
            amountOfEmptySlots = GetDeck.Length;
            amountOfFilledSlots=0;

            //  CountCards();
        }
        public virtual void ResetDeck()
        {
            /*
             * Reset the deck to his created state 
             */

            EmptySlots();
        }
        public void PrintDecks(DeckEnum deck)
        {
            /*
             * just for developers printing the data you want in the debug console
             */

            UnityEngine.Debug.Log(deck.ToString() + " Has This Amount Of slots in it :" + this._deckCards.Length);
            int counter = 0;

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i] != null)
                {
                    //  _deckCards[i].ToString();
                    counter++;
                }
            }
            UnityEngine.Debug.Log(deck.ToString() + "Amount OF ACTUALLY CARDS IN DECK : " + counter);
        }
        public bool IsTheCardInDeck(in Card card)
         {
            /*
             *Check if specific card is in the deck
             *check by matching ID
             */

            if (_deckCards == null|| card== null || _deckCards.Length == 0)
                return false;

            bool cardFound = false;

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i].CardInstanceID == card.CardInstanceID)
                {
                    cardFound = true;
                    break;
                }
            }

            return cardFound;
        }
        #endregion


        #region Private Functions
        protected void OrderDeck()
        {
            /*
             * go each card on deck
             * if found null spot 
             * then exchange the indexes
             * if not then there is no more cards in the deck so exit the function
            */

            for (int i = 0; i < _deckCards.Length - 1; i++)
            {
                if (_deckCards[i] == null)
                {
                    for (int j = i+1; j < _deckCards.Length; j++)
                    {
                        if (_deckCards[j] != null)
                        {
                            _deckCards[i] = _deckCards[j];
                            _deckCards[j] = null;
                            break;
                        }
                    }

                    if (_deckCards[i] == null)
                        return;
                }
            }
        }
        protected int GetIndexOfFirstNull()
        {
            /*
             * Get the first index in the deck
             * if the deck is not assigned then init it and return the first place
             * then order the deck so it will get the last empty spots
             * search for empty spots
             * if not found meaning that the deck is full and cannot be assinged then expand the deck and return the first empty index
             */

            if (_deckCards == null || _deckCards.Length == 0)
            {
                InitDeck(4);
                return 0;
            }

            OrderDeck();

            int index = -1;

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i] == null)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                index = _deckCards.Length;
                ExpandDeck();
            }

            return index;
        }
        private void ExpandDeck()
        {
            Array.Resize(ref GetDeck, GetDeck.Length + 5);
            CountCards();
        }
        #endregion


        public override string ToString()
        {
            string ToString = base.ToString() + "\n";

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i] != null)
                    ToString += $" Card Number {i + 1} is {_deckCards[i]}  {_deckCards[i].CardSO.CardName}\n";
                else
                    ToString += $"Card Number {i + 1} is NULL!";
            }

            return ToString;

        }
    }

    public enum DeckEnum
    {
        None = 0,
        Hand = 1,
        PlayerDeck = 2,
        Disposal=3,
        AutoActivate =4,
        Exhaust=5,
        Selected=6,
        CraftingSlots=7,
 
    };
    public interface IDeckHandler {
      
        bool DiscardCard(in Card card);
        void ResetDeck();
        bool AddCard(Card card);
        void InitDeck(int length);
    }
}