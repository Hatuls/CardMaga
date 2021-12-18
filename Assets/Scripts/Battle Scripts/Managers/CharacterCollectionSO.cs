using Battles;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Map.ActDifficultySO;

namespace Collections
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollectionSO :ScriptableObject , IScriptableObjectCollection
    {
        [SerializeField]
        private CharacterSO[] _charactersSO;
        public CharacterSO[] CharactersSO { get => _charactersSO; }

        private Dictionary<CharacterEnum, CharacterSO> _characterDict;


        public void Init(CharacterSO[] characterSOs)
            => _charactersSO = characterSOs;

        public CharacterSO GetCharacterSO(CharacterTypeEnum type)
        {
            int length = _charactersSO.Length;
            for (int i = 0; i < length; i++)
            {
                if (_charactersSO[i].CharacterType == type)
                    return _charactersSO[i];
            }

            throw new Exception($"Could not find the character type: {type}\nin the character collections");
        }

        public CharacterSO[] GetCharactersSO(CharacterTypeEnum type, NodeLevel NodeLevelsRange)
        {
            return GetCharactersSO(type).Where(
                diffuclty =>
                (diffuclty.CharacterDiffciulty >= NodeLevelsRange.MinDiffculty &&
                diffuclty.CharacterDiffciulty <= NodeLevelsRange.MaxDiffculty)
                ).ToArray();
        }
        public CharacterSO[] GetCharactersSO(CharacterTypeEnum type) => _charactersSO.Where(character => (character.CharacterType == type)).ToArray();

        public CharacterSO GetCharacterSO(CharacterEnum characterEnum)
        {
            if (characterEnum != CharacterEnum.Enemy)
            {
                var length = _charactersSO.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_charactersSO[i].CharacterEnum == characterEnum)
                        return _charactersSO[i];
                }
            }
            throw new Exception($"Character Collection: tried to get CharacterSO from character collection through the parameter CharacterEnum: <a>{characterEnum}</a>\n check if such characterSO exist in resource folder or in the collection!");

        }

        public void AssignDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
