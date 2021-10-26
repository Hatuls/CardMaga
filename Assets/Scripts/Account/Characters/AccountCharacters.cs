using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

namespace Account.GeneralData
{
    [Serializable]
    public class AccountCharacters
    {
        #region Fields
        [ShowInInspector]
        Dictionary<CharacterEnum, CharacterData> _characters;
        #endregion
        #region PrivateMethods
        public void  Init()
        {

            const byte characterAmount = 4;
            _characters = new Dictionary<CharacterEnum, CharacterData>(characterAmount);
            AddChatacterToDictionary(CharacterEnum.Chiara);
            AddChatacterToDictionary(CharacterEnum.TestSubject007);
        }
        #endregion
        #region Public Methods
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
