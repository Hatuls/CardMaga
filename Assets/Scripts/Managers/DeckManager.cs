using Cards;
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


        [SerializeField] BuffIcon _deckIcon;
        [SerializeField] BuffIcon _disposalIcon;

        [SerializeField] int _playerMaxHandSize;
        [SerializeField] int _playerStartingHandSize;
        public const int _placementSize = 1;
        public const int _craftingSlotsSize = 4;
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
        public static CraftingDeck GetCraftingSlots => (CraftingDeck)Instance.GetDeckAbst(DeckEnum.CraftingSlots);
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
        public void AddCardToDeck(Card addedCard, DeckEnum toDeck)
        {
            if (addedCard == null || toDeck == DeckEnum.Selected)
                return;

            GetDeckAbst(toDeck).AddCard(addedCard);
            CardUIManager.Instance.CraftCardUI(addedCard, toDeck);
        }
        public void DrawHand(int drawAmount)
        {
            if (BattleManager.isGameEnded)
                return;
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
                    }
                 //  else
                 //      Debug.LogError("DeckManager: The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card");


                }
                CardUIManager.Instance.DrawCards(toDeck.GetDeck, DeckEnum.PlayerDeck);
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
                    return DeckEnum.Selected;
                case 5:
                    return DeckEnum.CraftingSlots;
            }

            return 0;
        }
        public void OnEndTurn()
        {
            Debug.Log("Discarding the remain cards from hand and placement!");
            GetDeckAbst(DeckEnum.Selected).ResetDeck();
            GetDeckAbst(DeckEnum.Hand).ResetDeck();
        }


        
        public static void AddToCraftingSlot(Card card) 
            => Instance.GetDeckAbst(DeckEnum.CraftingSlots).AddCard(card);
        #endregion

        #region Private Functions   
        public void TransferCard(DeckEnum from, DeckEnum to, Card card) {

            if (card == null && !GetDeckAbst(from).IsTheCardInDeck(card))
                return;

            DeckAbst fromDeck = GetDeckAbst(from);
            DeckAbst toDeck = GetDeckAbst(to);



            toDeck.AddCard( card);
            fromDeck.DiscardCard(card);
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

                case DeckEnum.Selected:
                    GetDeckAbst(DeckEnum.Exhaust).ResetDeck();
                    break;
                case DeckEnum.CraftingSlots:
                    GetDeckAbst(DeckEnum.CraftingSlots).ResetDeck();
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

            _decksDict.Add(DeckEnum.PlayerDeck, new PlayerDeck(GetSetDeck, _deckIcon));
            _decksDict.Add(DeckEnum.Exhaust, new Exhaust(_playerMaxHandSize));
            _decksDict.Add(DeckEnum.Disposal,new Disposal(GetSetDeck.Length , _decksDict[DeckEnum.PlayerDeck] as PlayerDeck , _disposalIcon));
            _decksDict.Add(DeckEnum.Hand, new PlayerHand(_playerStartingHandSize, _decksDict[DeckEnum.Disposal] as Disposal));
            _decksDict.Add(DeckEnum.Selected, new Selected (_placementSize, _decksDict[DeckEnum.Disposal] as Disposal,_decksDict[DeckEnum.Hand] as PlayerHand ));
            _decksDict.Add(DeckEnum.CraftingSlots, new CraftingDeck(_craftingSlotsSize));

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