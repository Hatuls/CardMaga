using CardMaga.Card;
using System;

namespace Battle.Deck
{
    public class Discard : BaseDeck
    {
        public override event Action OnResetDeck;
        PlayerBaseDeck _playerBaseDeck;
        public Discard(int length, PlayerBaseDeck baseDeck) : base(length)
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
        public override bool AddCard(BattleCardData battleCard)
        {
            if (CheckDuplicate(battleCard))
                return false;

            bool added =   base.AddCard(battleCard);

            return added;
        }
        public override bool DiscardCard(in BattleCardData battleCard)
        {
            bool succeed = base.DiscardCard(battleCard);
            return succeed;
        }
       
        private bool CheckDuplicate(BattleCardData battleCard)
        {
            if (battleCard == null)
                return false;
            for (int i = 0; i < GetDeck.Length; i++)
            {
                if (GetDeck[i] == null)
                    continue;

                if (battleCard.CardInstance.InstanceID == GetDeck[i].CardInstance.InstanceID)
                    return true;
            }
            return false;
        }
    }
}