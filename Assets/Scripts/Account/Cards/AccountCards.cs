using System;
using System.Collections.Generic;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountCards : ILoadFirstTime
    {
        #region Fields
        [SerializeField]
        List<CardAccountInfo> _cardList = new List<CardAccountInfo>();
        #endregion
        #region Properties
        public List<CardAccountInfo> CardList => _cardList;
        #endregion
        #region PublicMethods
        public void AddCard(Cards.Card card)
        {
            _cardList.Add(new CardAccountInfo(card.CardSO.ID, card.CardInstanceID, card.CardLevel));
        }
        public bool RemoveCard(uint instanceId)
        {
            int length = CardList.Count;
            for (int i = 0; i <length; i++)
            {
                if (_cardList[i].InstanceID == instanceId)
                {
                    _cardList.Remove(_cardList[i]);
                    return true;
                }
            }
            return false;
        }
        public void ResetAccountCards()
        {

        }
        public bool CheckCardByID(uint id)
        {
            throw new NotImplementedException();
        }
        public CardAccountInfo FindCardByID(uint id)
        {
            throw new NotImplementedException();
        }
        public bool CheckCarDyInstance(uint instanceID)
        {
            throw new NotImplementedException();
        }
        public CardAccountInfo FindCardByInstance(uint instanceID)
        {
            throw new NotImplementedException();
        }

        public void NewLoad()
        {
            var factory = Factory.GameFactory.Instance;
         var staterPack = factory.CharacterFactoryHandler.GetCharacterSO(CharacterEnum.Chiara).Deck;
         var cards =   factory.CardFactoryHandler.CreateDeck(staterPack);
            for (int i = 0; i < cards.Length; i++)
            {
                AddCard(cards[i]);
            }
        }
        #endregion
    }
}
