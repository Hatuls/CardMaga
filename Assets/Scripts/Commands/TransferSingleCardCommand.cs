using Battle.Deck;
using CardMaga.Card;
using Managers;
using System.Collections.Generic;

namespace CardMaga.Commands
{
    public class TransferSingleCardCommand : ICommand
    {

        private DeckEnum _fromDeck;
        private DeckEnum _toDeck;
        private CardData _card;
        private DeckHandler _deckHandler;
        public TransferSingleCardCommand(IPlayer player, DeckEnum fromDeck, DeckEnum toDeck, CardData card) : this(player.DeckHandler, fromDeck, toDeck, card) { }
        public TransferSingleCardCommand(DeckHandler deckHandler, DeckEnum fromDeck, DeckEnum toDeck, CardData card)
        {
            _deckHandler = deckHandler;
            _fromDeck = fromDeck;
            _toDeck = toDeck;
            _card = card;
        }

        public void Execute()
        {
            _deckHandler.TransferCard(_fromDeck, _toDeck, _card);
        }

        public void Undo()
        {
            _deckHandler.TransferCard(_toDeck, _fromDeck, _card);
        }
    }

    public class DrawHandCommand : ICommand
    {
        private int _amount;
        private CardData[] _cardsDraw;
        private DeckHandler _deckHandler;
        public DrawHandCommand(IPlayer player, int amount) : this(player.DeckHandler, amount) { }
        public DrawHandCommand(DeckHandler deckHandler, int amount)
        {
            _deckHandler = deckHandler;
            _amount = amount;
            _cardsDraw = new CardData[amount];
        }

        public CardData[] CardsDraw { get => _cardsDraw;}

        public void Execute()
        {
            BaseDeck fromBaseDeck = _deckHandler[DeckEnum.PlayerDeck];
            BaseDeck toBaseDeck = _deckHandler[DeckEnum.Hand];

          

            CardData cardCache;

            for (int i = 0; i < _amount; i++)
            {
                cardCache = fromBaseDeck.GetFirstCard();

                if (cardCache == null)
                {
                    _deckHandler[DeckEnum.Discard].ResetDeck();
                    cardCache = fromBaseDeck.GetFirstCard();
                }

                if (cardCache != null)
                {
                    if (toBaseDeck.AddCard(cardCache))
                    {
                        _cardsDraw[i] = (cardCache);
                        fromBaseDeck.DiscardCard(cardCache);
                    }
                }
                else
                    UnityEngine.Debug.LogError($"DeckManager: The Reset from disposal deck to player's deck was not executed currectly and cound not get the first card {cardCache} \n " + fromBaseDeck.ToString());
            }


        }

        public void Undo()
        {
            BaseDeck selectDeck = _deckHandler[DeckEnum.Selected];

            if (selectDeck.GetDeck[0] != null)
                _deckHandler.TransferCard(DeckEnum.Selected, DeckEnum.Hand, selectDeck.GetDeck[0]);

            _deckHandler.TransferCardOnTopOfDeck(DeckEnum.Hand, DeckEnum.PlayerDeck, _cardsDraw);
        }
    }
}