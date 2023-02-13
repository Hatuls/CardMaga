using Battle.Turns;
using CardMaga.Card;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using CardMaga.Collection;
using UnityEngine;
using CardMaga.Battle.Players;
using CardMaga.Battle;
using CardMaga.Commands;
using CardMaga.Battle.Execution;

namespace Battle.Deck
{
    public class DeckHandler : IGetCollection<BattleCardData>
    {
        public const int MAX_DECK_HAND_SIZE = 10;
        public const int MAX_CRAFTING_SLOT_SIZE = 3;
        public event Action<BattleCardData[]> OnDrawCards;
        
        private Dictionary<DeckEnum, BaseDeck> _deckDictionary;
        private GameCommands _gameCommands;
        #region CardDataProprty
        
        public IEnumerable<BattleCardData> GetCollection 
        {
            get
            {
                foreach (var deck in _deckDictionary)
                {
                    foreach (BattleCardData data in deck.Value.GetCollection)
                    {
                        yield return data;
                    }
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
            InitDeck(character.StartingCards,battleManager.BattleData.BattleConfigSO.IsShufflingCards);
            _gameCommands = battleManager.GameCommands;
            TurnHandler turnhandler = battleManager.TurnHandler;
            var playersTurn = turnhandler.GetCharacterTurn(character.IsLeft);
            playersTurn.EndTurnOperations.Register(EndTurn);
        }
        internal void TransferCardOnTopOfDeck(DeckEnum from, DeckEnum to, BattleCardData[] cards)
        {
            BaseDeck fromDeck = this[from];
            BaseDeck toDeck = this[to];

            for (int i = 0; i < cards.Length; i++)
            {
              if(toDeck.DiscardCard(cards[i]))
                fromDeck.AddCardAtFirstPosition(cards[i]);
            }
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
        
        private void InitDeck(BattleCardData[] deck,bool toShuffleDeck)
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
        
        private void EndTurn(ITokenReceiver tokenMachine)
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
        public void AddCardToDeck(BattleCardData addedBattleCard, DeckEnum toDeck)
        {
            if (addedBattleCard == null || toDeck == DeckEnum.Selected)
                return;

            this[toDeck].AddCard(addedBattleCard);
        }

        public void DrawHand(int drawAmount)
        {
             if (drawAmount < 1)
            {
                Debug.LogError("DeckManager :Cannot draw - draw amount is less than 1!");
                return;
            }
            DrawHandCommand drawHandCommand = new DrawHandCommand(this, drawAmount);
            _gameCommands.GameDataCommands.DataCommands.AddCommand(drawHandCommand);
            
            OnDrawCards?.Invoke(drawHandCommand.CardsDraw);
        }
        public void TransferCard(DeckEnum from, DeckEnum to, BattleCardData battleCard)
        {
            if (battleCard == null && !this[from].IsTheCardInDeck(battleCard))
                return;

            BaseDeck fromBaseDeckCache = this[from];
            BaseDeck toBaseDeckCache = this[to];

            if (fromBaseDeckCache.DiscardCard(battleCard))
                toBaseDeckCache.AddCard(battleCard);
        }
        
        public BattleCardData[] GetCardsFromDeck(DeckEnum from)
         => this[from]?.GetDeck;

        public BattleCardData GetCardFromDeck(int index, DeckEnum from)
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