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

        public void NewLoad()
        {

        }
        #endregion
    }
}
