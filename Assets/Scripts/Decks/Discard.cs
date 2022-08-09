using CardMaga.Card;
using System;

namespace Battle.Deck
{
    public class Discard : BaseDeck
    {
        public override event Action OnResetDeck;
        PlayerBaseDeck _playerBaseDeck;
        public Discard(bool isPlayer,int length, PlayerBaseDeck baseDeck) : base(isPlayer,length)
        {
            _playerBaseDeck = baseDeck;
        }

        public override void ResetDeck()
        {
            var disposalDeck = GetDeck;
            for (int i = disposalDeck.Length - 1; i >= 0; i--)
            {
                if (disposalDeck[i] != null)
                {
                    _playerBaseDeck.AddCard(disposalDeck[i]);
                    DiscardCard(disposalDeck[i]);
                }
            }
            _playerBaseDeck.Shuffle();
            OnResetDeck?.Invoke();
        }
        public override bool AddCard(CardData card)
        {
            if (CheckDuplicate(card))
                return false;

          bool added =   base.AddCard(card);

            return added;
        }
        public override bool DiscardCard(in CardData card)
        {
            bool succeed = base.DiscardCard(card);
            return succeed;
        }
       
        private bool CheckDuplicate(CardData card)
        {
            if (card == null)
                return false;
            for (int i = 0; i < GetDeck.Length; i++)
            {
                if (GetDeck[i] == null)
                    continue;

                if (card.CardInstanceID == GetDeck[i].CardInstanceID)
                    return true;
            }
            return false;
        }
    }
}