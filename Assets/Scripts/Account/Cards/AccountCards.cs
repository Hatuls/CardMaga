using System;
using System.Collections.Generic;

namespace Account.GeneralData
{
    public class AccountCards
    {
        #region Fields
        List<CardAccountInfo> _cardList;
        #endregion
        #region Properties
        public List<CardAccountInfo> CardList => _cardList;
        #endregion
        #region PublicMethods
        public void AddCard(uint id, uint level)
        {

        }
        public void RemoveCard(uint instanceId)
        {

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
        #endregion
    }
}
