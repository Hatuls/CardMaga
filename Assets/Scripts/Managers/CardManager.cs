
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Battles;

namespace Managers
{

    public class CardManager : MonoSingleton<CardManager>
    {
        #region Fields
        List<int> _battleCardIdList;
        static int _battleID;

        #endregion
        public override void Init()
        {
            if (_battleCardIdList == null)
                _battleCardIdList = new List<int>(20);

            _battleID = 0;
        }

        public Card[] CreateDeck(CharacterSO.CardInfo[] cardsInfo)
        {
            if (cardsInfo != null && cardsInfo.Length != 0)
            {
                Card[] cards = new Card[cardsInfo.Length];

                for (int i = 0; i < cards.Length; i++)
                {
                    if (cardsInfo[i] != null)
                    cards[i] = CreateCard(cardsInfo[i].Card, cardsInfo[i].Level);
                }
                return cards;
            }
            return null;
        }


        public Card CreateCard(CardSO cardSO, int level = 0)
        {
            if (cardSO != null && (level >= 0 && level < cardSO.CardsMaxLevel))
            {
                _battleCardIdList.Add(_battleID);
                return new Card(_battleID++, cardSO, level);
            }
            return null;

        }
    }
}
