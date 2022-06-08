using System;
using Battles.UI;
using Cards;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using Unity.Events;
using UnityEngine;

namespace Battles.Deck
{
    public class DeckManager : MonoSingleton<DeckManager>
    {
        public static event Action<Card[]> OnDrawCards; 
        #region Fields

        #region Public Fields
        #endregion
        #region Private Fields
        [Sirenix.OdinInspector.ShowInInspector]
        private Dictionary<DeckEnum, DeckAbst> _playerDecks;
        private Dictionary<DeckEnum, DeckAbst> _OpponentDecks;
        [SerializeField]
        StringEvent _soundEvent;

        [SerializeField] BuffIcon _deckIcon;
        [SerializeField] BuffIcon _disposalIcon;

        [SerializeField] int _playerMaxHandSize;
        [SerializeField] int _playerStartingHandSize;
        public const int _placementSize = 1;
        public const int _craftingSlotsSize = 3;
        #endregion
        #endregion
        #region Properties

        public static PlayerCraftingSlots GetCraftingSlots(bool isPlayersDeck) => (PlayerCraftingSlots)Instance.GetDeckAbst(isPlayersDeck, DeckEnum.CraftingSlots);
        #endregion

        #region Functions

        #region Public Functions
        public override void Init(ITokenReciever token)
        {
            ResetDecks();
        }
        public void ResetDecks()
        {
            _OpponentDecks?.Clear();
            _OpponentDecks = null;
            _playerDecks?.Clear();
            _playerDecks = null;

        }

        public Card[] GetCardsFromDeck(bool isPlayersDeck, DeckEnum from)
        {
            var cache = GetDeckAbst(isPlayersDeck, from);
            if (cache == null)
                return null;

            return cache.GetDeck;
        }

        public override string ToString()
        {
            string r = "";

            foreach (var item in _playerDecks)
            {
                r += item.ToString();
                r += "\n\n\n";
            }

            return r;
        }



        public Card GetCardFromDeck(bool isPlayersDeck, int index, DeckEnum from)
        {

            var cache = GetDeckAbst(isPlayersDeck, from);

            if (cache == null || cache.GetDeck.Length <= 0)
                return null;
            else if (index >= 0 && index < cache.GetDeck.Length)
                return cache.GetDeck[index];

            return null;
        }
        public void AddCardToDeck(bool isPlayersDeck, Card addedCard, DeckEnum toDeck)
        {
            if (addedCard == null || toDeck == DeckEnum.Selected)
                return;

            GetDeckAbst(isPlayersDeck, toDeck).AddCard(addedCard);

        }
        public void DrawHand(bool isPlayersDeck, int drawAmount)
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
            var deck = isPlayersDeck ? _playerDecks : _OpponentDecks;

            if (deck == null || deck.Count == 0)
            {
                Debug.LogError("DeckManager : Deck Dictionary was not assignd!");
                return;
            }
            else if (drawAmount < 1)
            {
                Debug.LogError("DeckManager :Cannot draw - draw amount is less than 1!");
                return;
            }

            DeckAbst fromDeck = deck[DeckEnum.PlayerDeck];
            DeckAbst toDeck = deck[DeckEnum.Hand];
            Card cardCache;



            for (int i = 0; i < drawAmount; i++)
            {
                cardCache = fromDeck.GetFirstCard();

                if (cardCache == null)
                {
                    deck[DeckEnum.Disposal].ResetDeck();
                    cardCache = fromDeck.GetFirstCard();
                }

                if (cardCache != null)
                {

                    if (toDeck.AddCard(cardCache))
                        fromDeck.DiscardCard(cardCache);
                }
                else
                    Debug.LogError($"DeckManager: {isPlayersDeck} The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card {cardCache} \n " + fromDeck.ToString());


            }
            if (isPlayersDeck)
                OnDrawCards?.Invoke(toDeck.GetDeck);


        }

        public void OnEndTurn(bool isPlayersDeck)
        {
            Debug.Log("Discarding the remain cards from hand and placement!");
            ResetCharacterDeck(isPlayersDeck, DeckEnum.Selected);
            ResetCharacterDeck(isPlayersDeck, DeckEnum.Hand);
        }
        public void ResetCharacterDeck(bool isPlayer, DeckEnum deck)
            => GetDeckAbst(isPlayer, deck).ResetDeck();


        public static void AddToCraftingSlot(bool toPlayerCraftingSlots, Card card)
            => Instance.GetDeckAbst(toPlayerCraftingSlots, DeckEnum.CraftingSlots).AddCard(card);


        public void ResetDeck(bool isPlayers, DeckEnum resetDeck)
        {
            GetDeck(isPlayers)[resetDeck].ResetDeck();
        }

        public void ReplaceCard(bool isPlayer, DeckEnum firstDeck, Card firstCard, DeckEnum secondDeck, Card secondCard)
        {
            if (firstDeck == DeckEnum.Hand)
            {
                TransferCard(isPlayer, firstDeck, secondDeck, firstCard);
                TransferCard(isPlayer, secondDeck, firstDeck, secondCard);
            }
            else
            {
                TransferCard(isPlayer, secondDeck, firstDeck, secondCard);
                TransferCard(isPlayer, firstDeck, secondDeck, firstCard);
            }
        }

        #endregion

        #region Private Functions   
        public void TransferCard(bool isPlayersDeck, DeckEnum from, DeckEnum to, Card card)
        {

            if (card == null && !GetDeckAbst(isPlayersDeck, from).IsTheCardInDeck(card))
                return;

            DeckAbst fromDeck = GetDeckAbst(isPlayersDeck, from);
            DeckAbst toDeck = GetDeckAbst(isPlayersDeck, to);



            if (fromDeck.DiscardCard(card))
                toDeck.AddCard(card);


            //fromDeck.PrintDecks(from);
            //  toDeck.PrintDecks(to);
        }

        private void TransferCard(bool isPlayersDeck, DeckEnum from, DeckEnum to, int amount)
        {
            if (amount <= 0 || from == to)
                return;

            DeckAbst fromDeckCache = GetDeckAbst(isPlayersDeck, from);
            DeckAbst toDeckCache = GetDeckAbst(isPlayersDeck, to);

            if (fromDeckCache.GetAmountOfFilledSlots >= amount)
            {

                for (int i = 0; i < amount; i++)
                {
                    if (fromDeckCache.DiscardCard(fromDeckCache.GetFirstCard()))
                        toDeckCache.AddCard(toDeckCache.GetFirstCard());
                    // ui Transfer
                }

            }
            else
            {
                int remainTransfer = amount - fromDeckCache.GetAmountOfFilledSlots;

                for (int i = 0; i < fromDeckCache.GetAmountOfFilledSlots; i++)
                {

                    if (fromDeckCache.DiscardCard(fromDeckCache.GetFirstCard()))
                        toDeckCache.AddCard(toDeckCache.GetFirstCard());



                    // ui Transfer
                }


                if (from == DeckEnum.PlayerDeck)
                    RefillDeck(isPlayersDeck, from);

                TransferCard(isPlayersDeck, from, to, remainTransfer);

            }
        }

        private void RefillDeck(bool isPlayersDeck, DeckEnum deck)
        {
            switch (deck)
            {
                case DeckEnum.PlayerDeck:
                    GetDeckAbst(isPlayersDeck, deck).SetDeck = GetDeckAbst(isPlayersDeck, DeckEnum.Disposal).GetDeck;
                    GetDeckAbst(isPlayersDeck, DeckEnum.Disposal).ResetDeck();
                    break;

                case DeckEnum.Disposal:
                    GetDeckAbst(isPlayersDeck, DeckEnum.Disposal).ResetDeck();
                    break;

                case DeckEnum.Exhaust:
                    GetDeckAbst(isPlayersDeck, DeckEnum.Exhaust).ResetDeck();
                    break;

                case DeckEnum.Selected:
                    GetDeckAbst(isPlayersDeck, DeckEnum.Exhaust).ResetDeck();
                    break;
                case DeckEnum.CraftingSlots:
                    GetDeckAbst(isPlayersDeck, DeckEnum.CraftingSlots).ResetDeck();
                    break;
                case DeckEnum.Hand:
                default:
                    break;
            }

        }
        private Dictionary<DeckEnum, DeckAbst> GetDeck(bool playersDeck)
            => playersDeck ? _playerDecks : _OpponentDecks;
        private DeckAbst GetDeckAbst(bool isPlayersDeck, DeckEnum deckEnum)
        {
            var deck = GetDeck(isPlayersDeck);
            if (deck != null && deck.TryGetValue(deckEnum, out DeckAbst deckAbst))
                return deckAbst;

            Debug.LogError($"DeckManager Didnt find the " + ((isPlayersDeck == true) ? "Player's" : "Enemy's") + " " + deckEnum.ToString() + " deck");
            return null;
        }

        public void InitDeck(bool isPlayer, Card[] deck)
        {
            const int size = 6;

            if (isPlayer && _playerDecks == null)
                _playerDecks = new Dictionary<DeckEnum, DeckAbst>(size);
            else if (!isPlayer && _OpponentDecks == null)
                _OpponentDecks = new Dictionary<DeckEnum, DeckAbst>(size);

            var characterDeck = (isPlayer ? _playerDecks : _OpponentDecks);
            characterDeck.Clear();

            characterDeck.Add(DeckEnum.PlayerDeck, new PlayerDeck(isPlayer, deck, _deckIcon,_soundEvent));
            characterDeck.Add(DeckEnum.Exhaust, new Exhaust(isPlayer, _playerMaxHandSize));
            characterDeck.Add(DeckEnum.Disposal, new Disposal(isPlayer, deck.Length, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.PlayerDeck] as PlayerDeck, _disposalIcon));
            characterDeck.Add(DeckEnum.Hand, new PlayerHand(isPlayer, _playerStartingHandSize, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Disposal] as Disposal));
            characterDeck.Add(DeckEnum.Selected, new Selected(isPlayer, _placementSize, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Disposal] as Disposal, (isPlayer ? _playerDecks : _OpponentDecks)[DeckEnum.Hand] as PlayerHand));
            characterDeck.Add(DeckEnum.CraftingSlots, new PlayerCraftingSlots(isPlayer, _craftingSlotsSize));


            if (isPlayer)
                _playerDecks = characterDeck;
            else
                _OpponentDecks = characterDeck;

            ResetDecks(isPlayer);
        }

        private void ResetDecks(bool playersDeck)
        {
            var Deck = playersDeck ? _playerDecks : _OpponentDecks;
            if (Deck != null)
            {

                foreach (var item in Deck)
                {
                    if (!(item.Value is PlayerDeck))
                        item.Value.EmptySlots();
                    else
                        Deck[DeckEnum.PlayerDeck].ResetDeck();
                }
            }
        }
        #endregion


        #endregion

        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion
    }
}