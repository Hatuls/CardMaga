namespace Account.GeneralData
{
    public class CombosAccountInfo
    {
        #region Fields
        ushort _id;
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
