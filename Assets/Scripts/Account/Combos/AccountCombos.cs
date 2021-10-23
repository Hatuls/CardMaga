using System.Collections.Generic;
using System;

namespace Account.GeneralData
{
    public class AccountCombos
    {
        #region Fields
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
        #endregion
    }
}
