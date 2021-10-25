using Cards;

namespace Battles.Deck
{
    public class Disposal : DeckAbst
    {
        PlayerDeck _playerDeck;
        BuffIcon _disposalIcon;
        public Disposal(bool isPlayer,int length, PlayerDeck deck, BuffIcon icon) : base(isPlayer,length)
        {
            _playerDeck = deck;
            _disposalIcon = icon;
        }

        public override void ResetDeck()
        {
            var disposalDeck = GetDeck;
            for (int i = disposalDeck.Length - 1; i >= 0; i--)
            {
                if (disposalDeck[i] != null)
                {
                    _playerDeck.AddCard(disposalDeck[i]);
                    DiscardCard(disposalDeck[i]);
                }
            }
            _playerDeck.Shuffle();
        }
        public override bool AddCard(Card card)
        {
            if (CheckDuplicate(card))
                return false;


          bool added =   base.AddCard(card);
            if (isPlayer)
                _disposalIcon?.SetAmount(GetAmountOfFilledSlots);
            return added;
        }
        public override bool DiscardCard(in Card card)
        {
           bool succeed =  base.DiscardCard(card);
            if (isPlayer)
                _disposalIcon?.SetAmount(GetAmountOfFilledSlots);
            return succeed;
        }

        private bool CheckDuplicate(Card card)
        {
            if (card == null)
                return false;
            for (int i = 0; i < GetDeck.Length; i++)
            {
                if (GetDeck[i] == null)
                    continue;

                if (card.CardID == GetDeck[i].CardID)
                    return true;
            }
            return false;
        }
    }
}