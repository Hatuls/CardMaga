using Cards;
using System;
using System.Collections.Generic;
using Unity.Events;

namespace Battles.Deck
{
    public class PlayerDeck : DeckAbst
    {
        BuffIcon _deckIcon;
        SoundsEvent _soundEvent;
        public PlayerDeck(bool isPlayer, Card[] deckCards, BuffIcon DeckIcon, SoundsEvent soundsEvent) : base(isPlayer, deckCards)
        {
            _deckIcon = DeckIcon;
        }
        public override bool AddCard(Card card)
        {
         bool added =   base.AddCard(card);

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);
            return added;
        }


        public override Card GetFirstCard()
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
            if (isPlayer)
                SetDeck = Managers.PlayerManager.Instance.Deck;
            else
                SetDeck = EnemyManager.Instance.Deck;

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);


            Shuffle();
            OrderDeck();

            CountCards();
        }
        public override bool DiscardCard(in Card card)
        {
            bool succeed = base.DiscardCard(card);

            if (isPlayer)
                _deckIcon?.SetAmount(GetAmountOfFilledSlots);
            return succeed;
        }
        public void Shuffle()
        {

            OrderDeck();
            var list = new List<Card>(GetAmountOfFilledSlots);
            for (int i = 0; i < GetAmountOfFilledSlots; i++)
            {
                list.Add(GetDeck[i]);
                GetDeck[i] = null;
            }

            Random ran = new Random();

            for (int i = 0; i < GetAmountOfFilledSlots;i++)
            {
                int index = ran.Next(0, list.Count);
                GetDeck[i] = list[index];
                list.RemoveAt(index);
            }
            if (isPlayer)
                _soundEvent?.Raise(SoundsNameEnum.ShuffleDeck);

            //if(GetDeck == null)
            //    return;

                //int deckLength = GetDeck.Length;

                //if (deckLength == 0)
                //    return;

                //Card[] tempArray = new Card[deckLength];
                //List<Card> tempList = new List<Card>(deckLength);
                //for (int i = 0; i < deckLength; i++)
                //{
                //    tempList.Add(GetDeck[i]);
                //}
                //Random random = new Random();
                //for (int i = 0; i < deckLength; i++)
                //{
                //    int indexNum = random.Next(0, tempList.Count);
                //    tempArray[i] = tempList[indexNum];
                //    tempList.RemoveAt(indexNum);
                //}
                //for (int i = 0; i < deckLength; i++)
                //{
                //    GetDeck[i] = tempArray[i];
                //}
        }
    }
}