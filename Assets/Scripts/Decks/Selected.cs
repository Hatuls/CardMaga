using CardMaga.Card;
using System;
using UnityEngine;
namespace Battle.Deck
{
    public class Selected : BaseDeck
    {
        Discard _disposalDeck;
        PlayerHand _playerHandDeck;

        private event Action<BattleCardData,DeckEnum> OnSelectCardReset;
        private event Action<DeckEnum, DeckEnum,BattleCardData> OnSelectedCardDiscard;
        public static DeckEnum _discardTo;
        public Selected( int length, Discard deck, PlayerHand hand , Action<BattleCardData, DeckEnum> onCardReset, Action<DeckEnum, DeckEnum, BattleCardData> onSelectedCardDiscard) : base(length)
        {
            _disposalDeck = deck;
            _playerHandDeck = hand;
            OnSelectCardReset = onCardReset;
            OnSelectedCardDiscard = onSelectedCardDiscard;
        }

        public override void ResetDeck()
        {
            if (_disposalDeck == null || GetDeck == null)
            {
                Debug.LogError("Selected: Deck was not found!");
                return;
            }

            var deck = GetDeck;
       
            if (deck[0] != null)
            {
                //DeckManager.Instance.AddCardToDeck( isPlayer, deck[0], deck[0].IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard
                //    );
                OnSelectCardReset?.Invoke(deck[0], deck[0].IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard);
                DiscardCard(deck[0]);
            }
        }
      
        public bool DiscardCard(in BattleCardData battleCard, DeckEnum? discardTo = null)
        {

            if (battleCard == null)
                return false;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(1);
  
            if (GetDeck[0] == null)
                return true;

            DeckEnum destination = (discardTo == null) ?
                ( (battleCard.IsExhausted) ? DeckEnum.Exhaust : DeckEnum.Discard)
                : discardTo.Value;

            //DeckManager.Instance.TransferCard(
            //    isPlayer,
            //    DeckEnum.Selected,
            //    destination,
            //    battleCard);
            OnSelectedCardDiscard?.Invoke(DeckEnum.Selected, destination, battleCard);
            GetDeck[0] = null;

            return true;
        }
        public override bool AddCard(BattleCardData battleCard)
        {
            if (battleCard == null)
                return false;
            else if (GetDeck == null || GetDeck.Length == 0)
                InitDeck(1);


            if (GetDeck[0] == null)
            {
                _discardTo = DeckEnum.Hand;
                GetDeck[0] = battleCard;
            }

           // DiscardCard(GetDeck[0], DeckEnum.Hand);
            //AddCard(battleCard);
         return true;
        }
     
    }
}