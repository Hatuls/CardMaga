using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaCharactersHandler
    {
        #region Fields

        private Dictionary<int, MetaCharacterData> _characterDatas;

        private MetaCharacterData _mainMetaCharacterData;
        private int _maxCharacter;

        #endregion

        #region Prop

        public MetaCharacterData[] CharacterDatas => _characterDatas.Values.ToArray();//need to chaeck rei!@
        
        public MetaCharacterData CharacterData => _mainMetaCharacterData;

        #endregion

        public MetaCharactersHandler(IReadOnlyList<Character> characters,int mainCharacterIndex)
        {
            _characterDatas = new Dictionary<int, MetaCharacterData>();
            
            foreach (var character in characters)
            {
                MetaCharacterData data = new MetaCharacterData(character);
                _characterDatas.Add(character.ID,data);
            }

            if (TrySetCharacter(mainCharacterIndex))
            {
                
            }//need to re done
        }

        public bool TrySetCharacter(int characterIndex)
        {
            if (characterIndex > _characterDatas.Count)
            {
                Debug.LogError("Invalid Character Index");
                return false;
            }

            if (!_characterDatas.TryGetValue(characterIndex, out MetaCharacterData metaCharacterData))
            {
                Debug.LogError("Failed to get Character from Dictionary");
                return false;
            }

            _mainMetaCharacterData = metaCharacterData;
            return true;
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