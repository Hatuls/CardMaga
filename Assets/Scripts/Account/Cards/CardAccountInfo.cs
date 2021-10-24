namespace Account.GeneralData
{
    public class CardAccountInfo
    {
        #region Fields
        ushort _cardID;
        ushort _instanceID;
        byte _level;
        #endregion

        #region Properties
        public ushort CardID => _cardID;
        public ushort InstanceID { get => _instanceID; set => _instanceID = value; }
        public byte Level => _level;

        public CardAccountInfo(ushort cardID,ushort cardInstanceID ,byte level)
        {
            _cardID = cardID;
            _instanceID = cardInstanceID;
            _level = level;
        }
        #endregion
    }
}
