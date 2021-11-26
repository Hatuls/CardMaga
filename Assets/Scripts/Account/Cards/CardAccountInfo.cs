using System;
using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class CardAccountInfo : IEquatable<CardAccountInfo>
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
        public byte Level => _level;

        public CardAccountInfo()
        {
                
        }
        public CardAccountInfo(ushort cardID,ushort cardInstanceID ,byte level)
        {
            _cardID = cardID;
            _instanceID = cardInstanceID;
            _level = level;
        }

        public bool Equals(CardAccountInfo other)
        {
            return other.InstanceID == _instanceID;
        }
        #endregion
    }
}
