using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CombosAccountInfo
    {
        #region Fields
        [SerializeField]
        int _id;
        [SerializeField]
        int _level;
        #endregion
        #region Properties
        public int ID => _id;
        public int Level => _level;
        #endregion
        #region Public Methods
        public CombosAccountInfo(int id, int level)
        {
            _id = id;
            _level = level;
        }
        public void LevelUp() => _level++;
        #endregion
    }
}
