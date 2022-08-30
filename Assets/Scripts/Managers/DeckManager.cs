
using Battle.Turns;
using CardMaga.Card;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Deck
{
    //public class DeckManager : MonoSingleton<DeckManager>
    //{
    //    public static event Action<CardData[]> OnDrawCards;
    //    #region Fields

    //    #region Public Fields
    //    #endregion
    //    #region Private Fields
    //    [Sirenix.OdinInspector.ShowInInspector]
    //    private Dictionary<DeckEnum, BaseDeck> _playerDecks;
    //    private Dictionary<DeckEnum, BaseDeck> _OpponentDecks;
    //    [SerializeField]
    //    StringEvent _soundEvent;

    //    [SerializeField] int _playerMaxHandSize;
    //    [SerializeField] int _playerStartingHandSize;

    //    public const int _placementSize = 1;
    //    public const int _craftingSlotsSize = 3;

    //    #endregion
    //    #endregion
    //    #region Properties

    //    public static PlayerCraftingSlots GetCraftingSlots(bool isPlayersDeck) => (PlayerCraftingSlots)Instance.GetBaseDeck(isPlayersDeck, DeckEnum.CraftingSlots);
    //    #endregion

    //    #region Functions

    //    #region Public Functions

    //    public void ResetDecks()
    //    {
    //        _OpponentDecks?.Clear();
    //        _OpponentDecks = null;
    //        _playerDecks?.Clear();
    //        _playerDecks = null;

    //    }


    //    public override string ToString()
    //    {
    //        string r = "Deck Manager:\n";

    //        foreach (var item in _playerDecks)
    //        {
    //            r += item.ToString();
    //            r += "\n\n\n";
    //        }

    //        return r;
    //    }

    //    public CardData[] GetCardsFromDeck(bool isPlayersDeck, DeckEnum from)
    //    {
    //        var cache = GetBaseDeck(isPlayersDeck, from);
    //        if (cache == null)
    //            return null;

    //        return cache.GetDeck;
    //    }
    //    public CardData GetCardFromDeck(bool isPlayersDeck, int index, DeckEnum from)
    //    {

    //        var cache = GetBaseDeck(isPlayersDeck, from);

    //        if (cache == null || cache.GetDeck.Length <= 0)
    //            return null;
    //        else if (index >= 0 && index < cache.GetDeck.Length)
    //            return cache.GetDeck[index];

    //        return null;
    //    }
    //    public void AddCardToDeck(bool isPlayersDeck, CardData addedCard, DeckEnum toDeck)
    //    {
    //        if (addedCard == null || toDeck == DeckEnum.Selected)
    //            return;

    //        GetBaseDeck(isPlayersDeck, toDeck).AddCard(addedCard);

    //    }


    //    public void DrawHand(bool isPlayersDeck, int drawAmount)
    //    {
    //        if (BattleManager.isGameEnded)
    //            return;
    //        /*
    //         * check if everything is valid
    //         * cache the relevante decks (hand and player deck)
    //         * for each card we draw :
    //         * if there is still cards in the deck we want to transfer them to the hand one by one
    //         * if we found a card that is null its mean the deck is empty so we want to restore the cards from 
    //         * the disposal deck and redraw the amount we need
    //        */
    //        var deck = isPlayersDeck ? _playerDecks : _OpponentDecks;

    //        if (deck == null || deck.Count == 0)
    //        {
    //            Debug.LogError("DeckManager : Deck Dictionary was not assignd!");
    //            return;
    //        }
    //        else if (drawAmount < 1)
    //        {
    //            Debug.LogError("DeckManager :Cannot draw - draw amount is less than 1!");
    //            return;
    //        }

    //        BaseDeck fromBaseDeck = deck[DeckEnum.PlayerDeck];
    //        BaseDeck toBaseDeck = deck[DeckEnum.Hand];

    //        List<CardData> cardsDraw = new List<CardData>();

    //        CardData cardCache;

    //        for (int i = 0; i < drawAmount; i++)
    //        {
    //            cardCache = fromBaseDeck.GetFirstCard();

    //            if (cardCache == null)
    //            {
    //                deck[DeckEnum.Discard].ResetDeck();
    //                cardCache = fromBaseDeck.GetFirstCard();
    //            }

    //            if (cardCache != null)
    //            {
    //                if (toBaseDeck.AddCard(cardCache))
    //                {
    //                    cardsDraw.Add(cardCache);
    //                    fromBaseDeck.DiscardCard(cardCache);
    //                }
    //            }
    //            else
    //                Debug.LogError($"DeckManager: {isPlayersDeck} The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card {cardCache} \n " + fromBaseDeck.ToString());

    //        }
    //        if (isPlayersDeck)
    //            OnDrawCards?.Invoke(cardsDraw.ToArray());
    //    }

    //    public void OnEndTurn(bool isPlayersDeck)
    //    {
    //        Debug.Log("Discarding the remain cards from hand and placement!");
    //        ResetCharacterDeck(isPlayersDeck, DeckEnum.Selected);
    //        ResetCharacterDeck(isPlayersDeck, DeckEnum.Hand);
    //    }

    //    public void ResetCharacterDeck(bool isPlayer, DeckEnum deck)
    //        => GetBaseDeck(isPlayer, deck).ResetDeck();

    //    public static void AddToCraftingSlot(bool toPlayerCraftingSlots, CardData card)
    //        => Instance.GetBaseDeck(toPlayerCraftingSlots, DeckEnum.CraftingSlots).AddCard(card);

    //    public void ResetDeck(bool isPlayers, DeckEnum resetDeck)
    //    {
    //        GetDeck(isPlayers)[resetDeck].ResetDeck();
    //    }

    //    public void ReplaceCard(bool isPlayer, DeckEnum firstDeck, CardData firstCard, DeckEnum secondDeck, CardData secondCard)
    //    {
    //        if (firstDeck == DeckEnum.Hand)
    //        {
    //            TransferCard(isPlayer, firstDeck, secondDeck, firstCard);
    //            TransferCard(isPlayer, secondDeck, firstDeck, secondCard);
    //        }
    //        else
    //        {
    //            TransferCard(isPlayer, secondDeck, firstDeck, secondCard);
    //            TransferCard(isPlayer, firstDeck, secondDeck, firstCard);
    //        }
    //    }

    //    public void TransferCard(bool isPlayersDeck, DeckEnum from, DeckEnum to, CardData card)
    //    {

    //        if (card == null && !GetBaseDeck(isPlayersDeck, from).IsTheCardInDeck(card))
    //            return;

    //        BaseDeck fromBaseDeck = GetBaseDeck(isPlayersDeck, from);
    //        BaseDeck toBaseDeck = GetBaseDeck(isPlayersDeck, to);



    //        if (fromBaseDeck.DiscardCard(card))
    //            toBaseDeck.AddCard(card);


    //        //fromBaseDeck.PrintDecks(from);
    //        //  toBaseDeck.PrintDecks(to);
    //    }

    //    //public void AddCardOnTopOfDeck(bool isPlayersDeck, DeckEnum deck, CardData card)
    //    //{
    //    //    if (card == null && !GetBaseDeck(isPlayersDeck, deck).IsTheCardInDeck(card))
    //    //        return;

    //    //    BaseDeck baseDeck = GetBaseDeck(isPlayersDeck, deck);

    //    //    baseDeck.AddCardAtFirstPosition(card);
    //    //}
    //    public BaseDeck GetBaseDeck(bool isPlayersDeck, DeckEnum deckEnum)
    //    {
    //        var deck = GetDeck(isPlayersDeck);
    //        if (deck != null && deck.TryGetValue(deckEnum, out BaseDeck deckAbst))
    //            return deckAbst;

    //        Debug.LogError($"DeckManager Didnt find the " + ((isPlayersDeck == true) ? "Player's" : "Enemy's") + " " + deckEnum.ToString() + " deck");
    //        return null;
    //    }

    //    public void InitDeck(bool isPlayer, CardData[] deck)
    //    {
    //        const int size = 6;

    //        if (isPlayer && _playerDecks == null)
    //            _playerDecks = new Dictionary<DeckEnum, BaseDeck>(size);
    //        else if (!isPlayer && _OpponentDecks == null)
    //            _OpponentDecks = new Dictionary<DeckEnum, BaseDeck>(size);

    //        var characterDeck = (isPlayer ? _playerDecks : _OpponentDecks);
    //        characterDeck.Clear();

    //        //characterDeck.Add(DeckEnum.PlayerDeck, new PlayerBaseDeck(isPlayer, deck, _soundEvent));
    //        //characterDeck.Add(DeckEnum.Exhaust, new Exhaust(isPlayer, _playerMaxHandSize));
    //        //characterDeck.Add(DeckEnum.Discard, new Discard(isPlayer, deck.Length, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.PlayerDeck] as PlayerBaseDeck));
    //        //characterDeck.Add(DeckEnum.Hand, new PlayerHand(isPlayer, _playerStartingHandSize, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Discard] as Discard));
    //        //characterDeck.Add(DeckEnum.Selected, new Selected(isPlayer, _placementSize, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Discard] as Discard, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Hand] as PlayerHand));
    //        //characterDeck.Add(DeckEnum.CraftingSlots, new PlayerCraftingSlots(isPlayer, _craftingSlotsSize));


    //        if (isPlayer)
    //            _playerDecks = characterDeck;
    //        else
    //            _OpponentDecks = characterDeck;

    //        ResetDecks(isPlayer);
    //    }

    //    #endregion

    //    #region Private Functions   

    //    private void TransferCard(bool isPlayersDeck, DeckEnum from, DeckEnum to, int amount)
    //    {
    //        if (amount <= 0 || from == to)
    //            return;

    //        BaseDeck fromBaseDeckCache = GetBaseDeck(isPlayersDeck, from);
    //        BaseDeck toBaseDeckCache = GetBaseDeck(isPlayersDeck, to);

    //        if (fromBaseDeckCache.AmountOfFilledSlots >= amount)
    //        {

    //            for (int i = 0; i < amount; i++)
    //            {
    //                if (fromBaseDeckCache.DiscardCard(fromBaseDeckCache.GetFirstCard()))
    //                    toBaseDeckCache.AddCard(toBaseDeckCache.GetFirstCard());
    //                // ui Transfer
    //            }

    //        }
    //        else
    //        {
    //            int remainTransfer = amount - fromBaseDeckCache.AmountOfFilledSlots;

    //            for (int i = 0; i < fromBaseDeckCache.AmountOfFilledSlots; i++)
    //            {

    //                if (fromBaseDeckCache.DiscardCard(fromBaseDeckCache.GetFirstCard()))
    //                    toBaseDeckCache.AddCard(toBaseDeckCache.GetFirstCard());



    //                // ui Transfer
    //            }


    //            if (from == DeckEnum.PlayerDeck)
    //                RefillDeck(isPlayersDeck, from);

    //            TransferCard(isPlayersDeck, from, to, remainTransfer);

    //        }
    //    }

    //    private void RefillDeck(bool isPlayersDeck, DeckEnum deck)
    //    {
    //        switch (deck)
    //        {
    //            case DeckEnum.PlayerDeck:
    //                GetBaseDeck(isPlayersDeck, deck).SetDeck = GetBaseDeck(isPlayersDeck, DeckEnum.Discard).GetDeck;
    //                GetBaseDeck(isPlayersDeck, DeckEnum.Discard).ResetDeck();
    //                break;

    //            case DeckEnum.Discard:
    //                GetBaseDeck(isPlayersDeck, DeckEnum.Discard).ResetDeck();
    //                break;

    //            case DeckEnum.Exhaust:
    //                GetBaseDeck(isPlayersDeck, DeckEnum.Exhaust).ResetDeck();
    //                break;

    //            case DeckEnum.Selected:
    //                GetBaseDeck(isPlayersDeck, DeckEnum.Exhaust).ResetDeck();
    //                break;
    //            case DeckEnum.CraftingSlots:
    //                GetBaseDeck(isPlayersDeck, DeckEnum.CraftingSlots).ResetDeck();
    //                break;
    //            case DeckEnum.Hand:
    //            default:
    //                break;
    //        }

    //    }
    //    private Dictionary<DeckEnum, BaseDeck> GetDeck(bool playersDeck)
    //        => playersDeck ? _playerDecks : _OpponentDecks;



    //    private void ResetDecks(bool playersDeck)
    //    {
    //        var Deck = playersDeck ? _playerDecks : _OpponentDecks;
    //        if (Deck != null)
    //        {
    //            foreach (var item in Deck)
    //            {
    //                if (!(item.Value is PlayerBaseDeck))
    //                    item.Value.EmptySlots();
    //                else
    //                    Deck[DeckEnum.PlayerDeck].ResetDeck();
    //            }
    //        }
    //    }


    //    #endregion


    //    #endregion

    //    #region Monobehaviour Callbacks 
    //    #endregion
    //}
  //  [Serializable]
    public class DeckHandler
    {
        public const int MAX_DECK_HAND_SIZE = 10;
        public const int MAX_CRAFTING_SLOT_SIZE = 4;
        public event Action<CardData[]> OnDrawCards;

        private Dictionary<DeckEnum, BaseDeck> _deckDictionary;

        public BaseDeck this[DeckEnum deckEnum]
        {
            get
            {

                if (_deckDictionary.TryGetValue(deckEnum, out BaseDeck deck))
                    return deck;
                throw new Exception("Deck Dictionary was not assigend and current deck - " + deckEnum.ToString() + " does not have value");
            }
        }



        public DeckHandler(IPlayer character, BattleManager battleManager)
        {
            InitDeck(character.StartingCards);
            GameTurnHandler turnhandler = battleManager.TurnHandler;
            var playersTurn = turnhandler.GetCharacterTurn(character.IsLeft);
            playersTurn.EndTurnOperations.Register(EndTurn);
        }



        #region Private Functions

        private void TransferCard(DeckEnum from, DeckEnum to, int amount)
        {
            if (amount <= 0 || from == to)
                return;

            BaseDeck fromBaseDeckCache = this[from];
            BaseDeck toBaseDeckCache = this[to];

            if (fromBaseDeckCache.AmountOfFilledSlots >= amount)
            {

                for (int i = 0; i < amount; i++)
                {
                    if (fromBaseDeckCache.DiscardCard(fromBaseDeckCache.GetFirstCard()))
                        toBaseDeckCache.AddCard(toBaseDeckCache.GetFirstCard());
                    // ui Transfer
                }

            }
            else
            {
                int remainTransfer = amount - fromBaseDeckCache.AmountOfFilledSlots;

                for (int i = 0; i < fromBaseDeckCache.AmountOfFilledSlots; i++)
                {

                    if (fromBaseDeckCache.DiscardCard(fromBaseDeckCache.GetFirstCard()))
                        toBaseDeckCache.AddCard(toBaseDeckCache.GetFirstCard());



                    // ui Transfer
                }


                if (from == DeckEnum.PlayerDeck)
                    RefillDeck(from);

                TransferCard(from, to, remainTransfer);

            }
        }
        private void ResetDecks()
        {

            foreach (var item in _deckDictionary)
            {
                if (!(item.Value is PlayerBaseDeck))
                    item.Value.EmptySlots();
                else
                    ResetDeck(DeckEnum.PlayerDeck);

            }
        }
        private void RefillDeck(DeckEnum deck)
        {
            switch (deck)
            {
                case DeckEnum.PlayerDeck:
                    this[deck].SetDeck = this[DeckEnum.Discard].GetDeck;
                    this[DeckEnum.Discard].ResetDeck();
                    break;

                case DeckEnum.Discard:
                    this[DeckEnum.Discard].ResetDeck();
                    break;

                case DeckEnum.Exhaust:
                    this[DeckEnum.Exhaust].ResetDeck();
                    break;

                case DeckEnum.Selected:
                    this[DeckEnum.Exhaust].ResetDeck();
                    break;
                case DeckEnum.CraftingSlots:
                    this[DeckEnum.CraftingSlots].ResetDeck();
                    break;
                case DeckEnum.Hand:
                default:
                    break;
            }
        }
        private void InitDeck(CardData[] deck)
        {
            const int size = 6;

            _deckDictionary = new Dictionary<DeckEnum, BaseDeck>(size);


            _deckDictionary.Add(DeckEnum.PlayerDeck,    new PlayerBaseDeck(deck));
            _deckDictionary.Add(DeckEnum.Discard,       new Discard(deck.Length, this[DeckEnum.PlayerDeck] as PlayerBaseDeck));
            _deckDictionary.Add(DeckEnum.Hand,          new PlayerHand(MAX_DECK_HAND_SIZE, this[DeckEnum.Discard] as Discard));
            _deckDictionary.Add(DeckEnum.Selected,      new Selected(1, this[DeckEnum.Discard] as Discard, this[DeckEnum.Hand] as PlayerHand, AddCardToDeck, TransferCard));
            _deckDictionary.Add(DeckEnum.Exhaust,       new Exhaust(MAX_DECK_HAND_SIZE));
            _deckDictionary.Add(DeckEnum.CraftingSlots, new PlayerCraftingSlots(MAX_CRAFTING_SLOT_SIZE));
            ResetDecks();
        }
        private void EndTurn(ITokenReciever tokenMachine)
        {
            using (tokenMachine.GetToken())
            {
                ResetDeck(DeckEnum.Selected);
                ResetDeck(DeckEnum.Hand);
            }
        }
        public void ResetDeck(DeckEnum deck)
        => this[deck].ResetDeck();
        #endregion


        #region Public Functions
        public void AddCardToDeck(CardData addedCard, DeckEnum toDeck)
        {
            if (addedCard == null || toDeck == DeckEnum.Selected)
                return;

            this[toDeck].AddCard(addedCard);
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


            else if (drawAmount < 1)
            {
                Debug.LogError("DeckManager :Cannot draw - draw amount is less than 1!");
                return;
            }

            BaseDeck fromBaseDeck = this[DeckEnum.PlayerDeck];
            BaseDeck toBaseDeck = this[DeckEnum.Hand];

            List<CardData> cardsDraw = new List<CardData>();

            CardData cardCache;

            for (int i = 0; i < drawAmount; i++)
            {
                cardCache = fromBaseDeck.GetFirstCard();

                if (cardCache == null)
                {
                    this[DeckEnum.Discard].ResetDeck();
                    cardCache = fromBaseDeck.GetFirstCard();
                }

                if (cardCache != null)
                {
                    if (toBaseDeck.AddCard(cardCache))
                    {
                        cardsDraw.Add(cardCache);
                        fromBaseDeck.DiscardCard(cardCache);
                    }
                }
                else
                    Debug.LogError($"DeckManager: The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card {cardCache} \n " + fromBaseDeck.ToString());

            }

            OnDrawCards?.Invoke(cardsDraw.ToArray());
        }
        public void TransferCard(DeckEnum from, DeckEnum to, CardData card)
        {

            if (card == null && !this[from].IsTheCardInDeck(card))
                return;

            BaseDeck fromBaseDeckCache = this[from];
            BaseDeck toBaseDeckCache = this[to];

            if (fromBaseDeckCache.DiscardCard(card))
                toBaseDeckCache.AddCard(card);
        }
        public CardData[] GetCardsFromDeck(DeckEnum from)
         => this[from]?.GetDeck;

        public CardData GetCardFromDeck(int index, DeckEnum from)
        {

            var cache = this[from];

            if (cache == null || cache.GetDeck.Length <= 0)
                return null;
            else if (index >= 0 && index < cache.GetDeck.Length)
                return cache.GetDeck[index];

            return null;
        }
        #endregion
    }
}