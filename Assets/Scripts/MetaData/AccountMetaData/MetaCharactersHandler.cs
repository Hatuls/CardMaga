using System.Collections.Generic;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCharactersHandler
    {
        #region Fields

        private Dictionary<int, MetaCharacterData> _characterDatas;

        private MetaCharacterData _mainMetaCharacterData;
        private int _maxCharacter;

        #endregion

        public MetaCharactersHandler(IReadOnlyList<Character> characters)
        {
            foreach (var character in characters)
            {
                MetaCharacterData data = new MetaCharacterData(character);
                _characterDatas.Add(character.Id,data);
            }
        }

        public bool TryAddCharacter()//need to work on
        {
            if (_characterDatas.Count >= _maxCharacter)
            {
                Debug.LogWarning("Max amount of character");
                return false;
            }
            
            return true;
        }
    }
}