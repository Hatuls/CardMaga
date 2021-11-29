using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountCharacters : ILoadFirstTime
    {
        #region Fields
        [SerializeField]
        CharacterData[] _characterDatas;

        public CharacterData[] CharacterDatas { get => _characterDatas; }

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
            Array.Resize(ref _characterDatas, _characterDatas.Length + 1);
            for (int i = 0; i < _characterDatas.Length; i++)
            {
                if (_characterDatas[i] == null)
                {
                    var deck = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(character).Deck;
                    var list = AccountManager.Instance.AccountCards.CardList.Where((x) => deck.Any((y) => (x.CardID == y.Card.ID && x.Level == y.Level)));
                    _characterDatas[i] = new CharacterData(list.ToArray(), character);

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



            int currentLevel = AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value;
            var characters = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharactersSO(Battles.CharacterTypeEnum.Player);
            int length = characters.Length;
            _characterDatas = new CharacterData[0];
            for (int i = 0; i < length; i++)
            {
                if (characters[i].UnlockAtLevel <= currentLevel)
                {
                    AddChatacterToDictionary(characters[i].CharacterEnum);
                }
            }
        }
        #endregion
    }
}
