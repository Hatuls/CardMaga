namespace CardMaga.UI.Visuals
{
    public class AccountBarVisualData
    {
        #region Fields
        string _accountNickname;
        int _accountImageID;
        int _currentExpAmount;
        int _maxExpAmount;
        int _accountLevel;
        #endregion

        #region Properties
        public string AccountNickname => _accountNickname;
        public int AccountImageID => _accountImageID;
        public int CurrentExpAmount => _currentExpAmount;
        public int MaxExpAmount => _maxExpAmount;
        public int AccountLevel => _accountLevel;
        #endregion
        //Currency Types
    }
}