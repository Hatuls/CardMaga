using System.Collections.Generic;
using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class AccountCombos:ILoadFirstTime
    {
        #region Fields
        [SerializeField]
        List<CombosAccountInfo> _comboList;
        #endregion
        #region Properties
        public List<CombosAccountInfo> ComboList => _comboList;
        #endregion
        #region PublicMethods
        public void AddCombo(ushort id, byte level)
        {

        }
        public void ResetAccountCombo()
        {

        }
        public bool CheckDuplicate(ushort id)
        {
            throw new NotImplementedException();
        }
        public bool CheckComboByID(uint id)
        {
            throw new NotImplementedException();
        }
        public CombosAccountInfo GetComboByID()
        {
            throw new NotImplementedException();
        }

        public void NewLoad()
        {
        }
        #endregion
    }
}
