using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Account.GeneralData
{
    [Serializable]
    public class AccountCombos : ILoadFirstTime
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

        public bool IsCorrupted()
        {
            // not implemented yet
            // need to impelement later
            return false;
        }
        #endregion
    }
}
