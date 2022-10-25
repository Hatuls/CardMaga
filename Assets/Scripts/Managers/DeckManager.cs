using Battle.Turns;
using CardMaga.Card;
using CardMaga.Commands;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Deck
{
    public class DeckHandler
    {
        public const int MAX_DECK_HAND_SIZE = 10;
        public const int MAX_CRAFTING_SLOT_SIZE = 3;
        public event Action<CardData[]> OnDrawCards;
        
        private Dictionary<DeckEnum, BaseDeck> _deckDictionary;
        private GameCommands _gameCommands;
        #region CardDataProprty

        public IEnumerable<CardData> GetAllCardData
        {
            get
            {
                foreach (var cardData in GetHandDeck)
                {
                    yield return cardData;
                }
                foreach (var cardData in GetPlayerDeck)
                {
                    yield return cardData;
                }
                foreach (var cardData in GetDiscardDeck)
                {
                    yield return cardData;
                }
                foreach (var cardData in GetExhaustDeck)
                {
                    yield return cardData;
                }
                foreach (var cardData in GetSelectedDeck)
                {
                    yield return cardData;
                }
            }
        }

        public IEnumerable<CardData> GetHandDeck
        {
            get
            {
                BaseDeck deck = this[DeckEnum.Hand];

                CardData[] cardDatas = deck.GetDeck;
                int length = cardDatas.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (cardDatas[i] != null)
                        yield return cardDatas[i];
                    else
                        yield break;
                }
            }
        }

        public IEnumerable<CardData> GetPlayerDeck
        {
            get
            {
                BaseDeck deck = this[DeckEnum.PlayerDeck];

                CardData[] cardDatas = deck.GetDeck;
                int length = cardDatas.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (cardDatas[i] != null)
                        yield return cardDatas[i];
                    else
                        yield break;
                }
            }
        }

        public IEnumerable<CardData> GetDiscardDeck
        {
            get
            {
                BaseDeck deck = this[DeckEnum.Discard];

                CardData[] cardDatas = deck.GetDeck;
                int length = cardDatas.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (cardDatas[i] != null)
                        yield return cardDatas[i];
                    else
                        yield break;
                }
            }
        }

        public IEnumerable<CardData> GetExhaustDeck
        {
            get
            {
                BaseDeck deck = this[DeckEnum.Exhaust];

                CardData[] cardDatas = deck.GetDeck;
                int length = cardDatas.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (cardDatas[i] != null)
                        yield return cardDatas[i];
                    else
                        yield break;
                }
            }
        }

        public IEnumerable<CardData> GetSelectedDeck
        {
            get
            {
                BaseDeck deck = this[DeckEnum.Selected];

                CardData[] cardDatas = deck.GetDeck;
                int length = cardDatas.Length;
                
                for (int i = 0; i < length; i++)
                {
                    if (cardDatas[i] != null)
                        yield return cardDatas[i];
                    else
                        yield break;
                }
            }
        }

        #endregion
        
        public BaseDeck this[DeckEnum deckEnum]
        {
            get
            {
                if (_deckDictionary.TryGetValue(deckEnum, out BaseDeck deck))
                    return deck;
                throw new Exception("Deck Dictionary was not assigend and current deck - " + deckEnum.ToString() + " does not have value");
            }
        }
        
        public DeckHandler(IPlayer character, IBattleManager battleManager)
        {
            InitDeck(character.StartingCards,battleManager.BattleData.BattleConfigSO.IsShuffleCard);
            GameTurnHandler turnhandler = battleManager.TurnHandler;
            _gameCommands = battleManager.GameCommands;
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
                //case DeckEnum.CraftingSlots:
                //    this[DeckEnum.CraftingSlots].ResetDeck();
                   // break;
                case DeckEnum.Hand:
                default:
                    break;
            }
        }
        
        private void InitDeck(CardData[] deck,bool toShuffleDeck)
        {
            const int size = 6;

            _deckDictionary = new Dictionary<DeckEnum, BaseDeck>(size);
            
            _deckDictionary.Add(DeckEnum.PlayerDeck,    new PlayerBaseDeck(deck,toShuffleDeck));
            _deckDictionary.Add(DeckEnum.Discard,       new Discard(deck.Length, this[DeckEnum.PlayerDeck] as PlayerBaseDeck));
            _deckDictionary.Add(DeckEnum.Hand,          new PlayerHand(MAX_DECK_HAND_SIZE, this[DeckEnum.Discard] as Discard));
            _deckDictionary.Add(DeckEnum.Selected,      new Selected(1, this[DeckEnum.Discard] as Discard, this[DeckEnum.Hand] as PlayerHand, AddCardToDeck, TransferCard));
            _deckDictionary.Add(DeckEnum.Exhaust,       new Exhaust(MAX_DECK_HAND_SIZE));
            //_deckDictionary.Add(DeckEnum.CraftingSlots, new PlayerCraftingSlots(MAX_CRAFTING_SLOT_SIZE));
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
            DrawHandCommand drawCommand = new DrawHandCommand(this, drawAmount);
            _gameCommands.GameDataCommands.DataCommands.AddCommand(drawCommand);
            OnDrawCards?.Invoke(drawCommand.CardsDraw);
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
        public void TransferCardOnTopOfDeck(DeckEnum from, DeckEnum to,params CardData[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
                TransferCardOnTopOfDeck(from, to, cards[i]);
        }
        public void TransferCardOnTopOfDeck(DeckEnum from, DeckEnum to, CardData card)
        {
            if (card == null && !this[from].IsTheCardInDeck(card))
                return;

            BaseDeck fromBaseDeckCache = this[from];
            BaseDeck toBaseDeckCache = this[to];

            if (fromBaseDeckCache.DiscardCard(card))
                toBaseDeckCache.AddCardAtFirstPosition(card);
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