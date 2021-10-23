using System.Collections.Generic;
using System;

namespace Account.GeneralData
{
    public class AccountCharacters
    {
        #region Fields
        Dictionary<CharacterEnum, CharacterData> _characters;
        #endregion
        #region PrivateMethods
        public void  Init()
        {

        }
        #endregion
        #region PublicMethods
        public CharacterData GetCharacterData(CharacterEnum character)
        {
            throw new NotImplementedException();
        }
        public void AddChatacterToDictionary(CharacterData characterData)
        {

        }
        public void CreateCharacterFromServer(CharacterData characterData)
        {

        }
        public void CreateCharacterFromCSV(CharacterData characterData)
        {

        }
        #endregion
    }
}
