using CardMaga.Card;
using System;
using System.Collections.Generic;
using Unity.Events;

namespace Battle.Deck
{
    public class PlayerBaseDeck : BaseDeck
    {
        public event Action OnShuffleDeck;

        private bool _toShuffleDeck = true;
   
        public PlayerBaseDeck(BattleCardData[] deckCards,bool toShuffleDeck) : base( deckCards)
        {
            _toShuffleDeck = toShuffleDeck;
        }
        public override bool AddCard(BattleCardData battleCard)
        {
         bool added =   base.AddCard(battleCard);

            //if (isPlayer)
            //    _deckIcon?.SetAmount(AmountOfFilledSlots);
        //    OnAmountOfFilledSlotsChange?.Invoke(AmountOfEmptySlots);
            return added;
        }


        public override BattleCardData GetFirstCard()
        {
            var card = base.GetFirstCard();
            if (card ==  null)
            {
                OrderDeck();
                card = base.GetFirstCard();
            }
            return card;
        }
        public override void ResetDeck()
        {
            //if (isPlayer)
            //    SetDeck = Managers.PlayerManager.Instance.StartingCards;
            //else
            //    SetDeck = EnemyManager.Instance.StartingCards;


            Shuffle();
            OrderDeck();

            CountCards();
        }
        public override bool DiscardCard(in BattleCardData battleCard)
        {
            bool succeed = base.DiscardCard(battleCard);
    //        OnAmountOfFilledSlotsChange?.Invoke(AmountOfEmptySlots);
            //if (isPlayer)
            //    _deckIcon?.SetAmount(AmountOfFilledSlots);
            return succeed;
        }
        public void Shuffle()
        {
            if (!_toShuffleDeck)
                return;
            
            OrderDeck();
            var list = new List<BattleCardData>(AmountOfFilledSlots);
            for (int i = 0; i < AmountOfFilledSlots; i++)
            {
                list.Add(GetDeck[i]);
                GetDeck[i] = null;
            }

            Random ran = new Random();

            for (int i = 0; i < AmountOfFilledSlots;i++)
            {
                int index = ran.Next(0, list.Count);
                GetDeck[i] = list[index];
                list.RemoveAt(index);
            }
            
            OnShuffleDeck?.Invoke();
        }
    }
}