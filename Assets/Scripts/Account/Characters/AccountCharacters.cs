using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class AccountCharacters : ILoadFirstTime
    {
        #region Fields
        [SerializeField] 
        CharacterData[] _characterDatas;
        //  Dictionary<CharacterEnum, CharacterData> _characters;
        #endregion
        #region PrivateMethods

        #endregion
        #region Public Methods

        public CharacterData GetCharacterData(CharacterEnum character)
        {
            //if(_characters.TryGetValue(character, out CharacterData value))
            //    return value;

            for (int i = 0; i < _characterDatas.Length; i++)
            {
                if (_characterDatas[i].CharacterEnum == character)
                    return _characterDatas[i];
            }

            throw new Exception("AccountCharacters characterNotFound!");
        }
        public void AddChatacterToDictionary(CharacterEnum character)
        {
            if (character == CharacterEnum.Enemy)
            {
                throw new Exception("AccountCharacters enemy can not be added to dictionary");
            }
            // _characters.Add(character,new CharacterData(character));
            for (int i = 0; i < _characterDatas.Length; i++)
            {
                if (_characterDatas[i] == null)
                {
                    _characterDatas[i] = new CharacterData(character);
                    return;
                }
            }

            throw new Exception("All Character Datas Are Full!");
        }
        public void CreateCharacterFromServer(CharacterData characterData)
        {
            throw new NotImplementedException();
        }

        public void NewLoad()
        {
            const byte characterAmount = 4;
            //    _characters = new Dictionary<CharacterEnum, CharacterData>(characterAmount);
            _characterDatas = new CharacterData[characterAmount];
            AddChatacterToDictionary(CharacterEnum.Chiara);
            AddChatacterToDictionary(CharacterEnum.TestSubject007);
        }
        #endregion
    }
}
