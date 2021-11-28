using System;
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
            const byte characterAmount = 2;
            //    _characters = new Dictionary<CharacterEnum, CharacterData>(characterAmount);
            _characterDatas = new CharacterData[characterAmount];
            AddChatacterToDictionary(CharacterEnum.Chiara);
            AddChatacterToDictionary(CharacterEnum.TestSubject007);
        }
        #endregion
    }
}
