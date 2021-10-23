namespace Account.GeneralData
{
    public class CardAccountInfo
    {
        #region Fields
        ushort _cardID;
        ushort _InstanceID;
        byte _level;
        #endregion

        #region Properties
        public ushort CardID => _cardID;
        public ushort InstanceID => _InstanceID;
        public byte Level => _level;
        #endregion
    }
}
