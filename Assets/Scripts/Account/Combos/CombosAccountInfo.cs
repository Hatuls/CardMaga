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

        public void LevelUp() => _level++;
    }
}
