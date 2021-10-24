namespace UI.Meta.Laboratory
{
    public class CharacterDeckSelection : IOpenCloseUIHandler
    {
        #region Fields
        CharacterEnum _currentCharacter;
        #endregion

        #region Properties
        public CharacterEnum CurrentCharacter => _currentCharacter;
        #endregion

        #region Public Methods
        public void SelectCharacter(int index)
        {

        }
        #endregion

        #region Interface
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
