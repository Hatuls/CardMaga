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

        #region Prop

        public MetaCharacterData CharacterData => _mainMetaCharacterData;

        #endregion

        public MetaCharactersHandler(IReadOnlyList<Character> characters)
        {
            _characterDatas = new Dictionary<int, MetaCharacterData>();
            
            foreach (var character in characters)
            {
                MetaCharacterData data = new MetaCharacterData(character);
                _characterDatas.Add(character.Id,data);
            }

            if (_characterDatas.TryGetValue(1, out MetaCharacterData metaCharacterData))//need to re done
                _mainMetaCharacterData = metaCharacterData;
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