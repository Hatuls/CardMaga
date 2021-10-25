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
            _characters = new Dictionary<CharacterEnum, CharacterData>();
            AddChatacterToDictionary(CharacterEnum.Chiara);
            AddChatacterToDictionary(CharacterEnum.TestSubject007);
        }
        #endregion
        #region PublicMethods
        public AccountCharacters()
        {
            Init();
        }
        public CharacterData GetCharacterData(CharacterEnum character)
        {
            if(_characters.TryGetValue(character, out CharacterData value))
            {
                return value;
            }
            throw new Exception("AccountCharacters characterNotFound!");
        }
        public void AddChatacterToDictionary(CharacterEnum character)
        {
            if(character == CharacterEnum.Enemy)
            {
                throw new Exception("AccountCharacters enemy can not be added to dictionary");
            }
            _characters.Add(character,new CharacterData(character));
        }
        public void CreateCharacterFromServer(CharacterData characterData)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
