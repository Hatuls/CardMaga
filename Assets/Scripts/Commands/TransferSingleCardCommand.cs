using Battle.Deck;
using CardMaga.Battle.Players;
using CardMaga.Card;

namespace CardMaga.Commands
{
    public class TransferSingleCardCommand : ICommand
    {

        private DeckEnum _fromDeck;
        private DeckEnum _toDeck;
        private BattleCardData _battleCard;
        private DeckHandler _deckHandler;
        public TransferSingleCardCommand(IPlayer player, DeckEnum fromDeck, DeckEnum toDeck, BattleCardData battleCard) : this(player.DeckHandler, fromDeck, toDeck, battleCard) { }
        public TransferSingleCardCommand(DeckHandler deckHandler, DeckEnum fromDeck, DeckEnum toDeck, BattleCardData battleCard)
        {
            _deckHandler = deckHandler;
            _fromDeck = fromDeck;
            _toDeck = toDeck;
            _battleCard = battleCard;
        }

        public void Execute()
        {
            _deckHandler.TransferCard(_fromDeck, _toDeck, _battleCard);
        }

        public void Undo()
        {
            _deckHandler.TransferCard(_toDeck, _fromDeck, _battleCard);
        }
    }

    public class DrawHandCommand : ICommand
    {
        private int _amount;
        private BattleCardData[] _cardsDraw;
        private DeckHandler _deckHandler;
        public DrawHandCommand(IPlayer player, int amount) : this(player.DeckHandler, amount) { }
        public DrawHandCommand(DeckHandler deckHandler, int amount)
        {
            _deckHandler = deckHandler;
            _amount = amount;
            _cardsDraw = new BattleCardData[amount];
        }

        public BattleCardData[] CardsDraw { get => _cardsDraw; }

        public void Execute()
        {
            BaseDeck fromBaseDeck = _deckHandler[DeckEnum.PlayerDeck];
            BaseDeck toBaseDeck = _deckHandler[DeckEnum.Hand];



            BattleCardData battleCardCache;

            for (int i = 0; i < _amount; i++)
            {
                battleCardCache = fromBaseDeck.GetFirstCard();

                if (battleCardCache == null)
                {
                    _deckHandler[DeckEnum.Discard].ResetDeck();
                    battleCardCache = fromBaseDeck.GetFirstCard();
                }

                if (battleCardCache != null)
                {
                    if (toBaseDeck.AddCard(battleCardCache))
                    {
                        _cardsDraw[i] = (battleCardCache);
                        fromBaseDeck.DiscardCard(battleCardCache);
                    }
                }
                else
                    UnityEngine.Debug.LogError($"DeckManager: The Reset from disposal deck to player's deck was not executed currectly and cound not get the first battleCard {battleCardCache} \n " + fromBaseDeck.ToString());
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