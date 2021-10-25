using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CombosAccountInfo
    {
        #region Fields
        [SerializeField]
        ushort _id;
        [SerializeField]
        byte _level;
        #endregion
        #region Properties
        public ushort ID => _id;
        public byte Level => _level;
        #endregion
        #region Public Methods
        public CombosAccountInfo(ushort id, byte level)
        {
            _id = id;
            _level = level;
        }
        public void LevelUp() => _level++;
        #endregion
    }
}
