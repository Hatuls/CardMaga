using CardMaga.Card;
using System;
using System.Collections.Generic;
using CardMaga.Collection;

namespace Battle.Deck
{
    public abstract class BaseDeck : IDeckHandler ,IGetCollection<BattleCardData>
    {
        public event Action<int> OnAmountOfFilledSlotsChange;
        public virtual event Action OnResetDeck;

        private BattleCardData[] _deckCards;
        private int _amountOfFilledSlots = 0;
        
        #region Properties

        public IEnumerable<BattleCardData> GetCollection
        {
            get
            {
                int length = _deckCards.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (_deckCards[i] != null)
                        yield return _deckCards[i];
                    else
                        yield break;
                }
            }
        }

        public int AmountOfFilledSlots { 
            get
            {
                return _amountOfFilledSlots;
            }
            protected set
            {
                if (_amountOfFilledSlots != value)
                {
                    _amountOfFilledSlots = value;
                    OnAmountOfFilledSlotsChange?.Invoke(value);
                }
            }
        }
        public ref BattleCardData[] GetDeck
            => ref _deckCards; 
        
        public BattleCardData[] SetDeck
        {
            set
            {
                if (value != null)
                    _deckCards = value;

                CountCards();
                OrderDeck();
            }
        }

        public int AmountOfEmptySlots { get; set; }
        #endregion

        #region Public Functions
        public BaseDeck(BattleCardData[] deckCards)
        {
            SetDeck = deckCards;

        }
        public BaseDeck(int length)
        {
            InitDeck(length);
        }

        public void InitDeck(int length)
        {
            // assign new deck in the legnth we want

            SetDeck = new BattleCardData[length];
        }

        public void CountCards()
        {
            // count for the amount of empty and not empty slots 
            int currentAmountOfFilledSlots = AmountOfFilledSlots;
            AmountOfEmptySlots = 0;
            AmountOfFilledSlots = 0;
            if (_deckCards == null)
            {
                UnityEngine.Debug.LogError("DeckCards is null");
                return;
            }

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i] == null)
                    AmountOfEmptySlots++;
                else
                    AmountOfFilledSlots++;
            }
            if (currentAmountOfFilledSlots != AmountOfFilledSlots&& OnAmountOfFilledSlotsChange!=null)
                OnAmountOfFilledSlotsChange.Invoke(AmountOfFilledSlots);

        }
        public virtual BattleCardData GetFirstCard()
        {
            // return the BattleCard from the first slot in the array


            if (_deckCards != null && _deckCards.Length > 0)
                return _deckCards[0];

            return null;
        }
        public virtual bool AddCard(BattleCardData battleCard)
        {
            //  add battleCard to the deck
            // if its not assigned then create a deck of 5 cards
            // search for the first empty spots
            // order the deck so there wont be gaps between cards and nulls
            // and recount the amounts

            if (_deckCards == null || _deckCards.Length == 0)
                InitDeck(4);

            if (ExpandingDeckPolicy())
            {
                int index = GetIndexOfFirstNull();
                _deckCards[index] = battleCard;

                OrderDeck();
                AmountOfEmptySlots--;
                AmountOfFilledSlots++;
                //CountCards();
                return true;
            }
            return false;
        }
        public virtual bool ExpandingDeckPolicy() => true;
        public virtual bool DiscardCard(in BattleCardData battleCard)
        {
            /*
             * check if the deck and battleCard is valids
             * loop throught the decks and search this battleCard by matching id between the battleCard in the deck and the one we check
             * if we found then reset him to null 
             * and reoder and recount
             */
            bool foundCard= false;

            if (_deckCards != null && battleCard != null && _deckCards.Length > 0)
            {
                for (int i = 0; i < _deckCards.Length; i++)
                {
                    if (_deckCards[i] != null
                       && _deckCards[i].CardInstance.InstanceID == battleCard.CardInstance.InstanceID)
                    {
                        _deckCards[i] = null;
                        AmountOfEmptySlots++;
                        AmountOfFilledSlots--;
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
            AmountOfEmptySlots = GetDeck.Length;
            AmountOfFilledSlots = 0;

            //  CountCards();
        }
        public void AddCardAtFirstPosition(BattleCardData battleCard)
        {
            BattleCardData[] cardDatas = new BattleCardData[GetDeck.Length + 1];
            Array.Copy(GetDeck, cardDatas, 1);

            GetDeck = cardDatas;
            GetDeck[0] = battleCard;

            OrderDeck();
            CountCards();
        }
        public abstract void ResetDeck();
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
        public bool IsTheCardInDeck(in BattleCardData battleCard)
         {
            /*
             *Check if specific battleCard is in the deck
             *check by matching CoreID
             */

            if (_deckCards == null|| battleCard== null || _deckCards.Length == 0)
                return false;

            bool cardFound = false;

            for (int i = 0; i < _deckCards.Length; i++)
            {
                if (_deckCards[i].CardInstance.InstanceID == battleCard.CardInstance.InstanceID)
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
             * go each battleCard on deck
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
                    ToString += $" BattleCard Number {i + 1} is {_deckCards[i]}  {_deckCards[i].CardSO.CardName}\n";
                else
                    ToString += $"BattleCard Number {i + 1} is NULL!";
            }

            return ToString;
        }

        
    }

    public enum DeckEnum
    {
        None = 0,
        Hand = 1,
        PlayerDeck = 2,
        Discard=3,
        AutoActivate =4,
        Exhaust=5,
        Selected=6,
      //  CraftingSlots=7,
 
    };
    
    public interface IDeckHandler {
        bool DiscardCard(in BattleCardData battleCard);
        void ResetDeck();
        bool AddCard(BattleCardData battleCard);
        void InitDeck(int length);
    }
}