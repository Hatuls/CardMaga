using UnityEngine;
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
        #region PrivateMethods
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
            return false;
        }
        public CardAccountInfo FindCardByID(uint id)
        {
            return null;
        }
        public bool CheckCarDyInstance(uint instanceID)
        {
            return false;
        }
        public CardAccountInfo FindCardByInstance(uint instanceID)
        {
            return null;
        }
        #endregion
    }
}
