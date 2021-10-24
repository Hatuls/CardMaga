using System;
using UnityEngine;


namespace Battles
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollectionSO :ScriptableObject
    {
        [SerializeField]
        private CharacterSO[] _charactersSO;
        public CharacterSO[] CharactersSO { get => _charactersSO; }


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

        internal CharacterSO GetCharacterSO(CharacterEnum characterEnum)
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
    }
}
