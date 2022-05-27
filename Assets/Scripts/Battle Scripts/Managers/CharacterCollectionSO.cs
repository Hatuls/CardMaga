using Battles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardMaga.ActDifficultySO;

namespace Collections
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollectionSO : ScriptableObject, IScriptableObjectCollection
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
            int rightIndex = type == CharacterTypeEnum.Elite_Enemy ? 1 : 0;
            var range = NodeLevelsRange.MinMaxCharacters[rightIndex];

            return GetCharactersSO(type).Where(
                diffuclty =>
                (diffuclty.CharacterDiffciulty >= range.MinDiffculty &&
                diffuclty.CharacterDiffciulty <= range.MaxDiffculty)
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
