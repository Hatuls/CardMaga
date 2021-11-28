using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CardCoreInfo : IEquatable<CardCoreInfo>
    {
        #region Fields
        [SerializeField]
        ushort _cardID;
        [SerializeField]
        ushort _instanceID;
        [SerializeField]
        byte _level;
        #endregion

        #region Properties
        public ushort CardID => _cardID;
        public ushort InstanceID { get => _instanceID; set => _instanceID = value; }
        public byte Level { get => _level; set => _level = value; }

        public CardCoreInfo()
        {
                
        }
        public CardCoreInfo(ushort cardID,ushort cardInstanceID ,byte level)
        {
            _cardID = cardID;
            _instanceID = cardInstanceID;
            _level = level;
        }

        public bool Equals(CardCoreInfo other)
        {
            return other.InstanceID == _instanceID;
        }
        #endregion
    }
}
