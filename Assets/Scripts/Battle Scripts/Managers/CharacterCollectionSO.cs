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

        public CharacterSO GetCharacter(CharacterTypeEnum type)
        {
            int length = _charactersSO.Length;
            for (int i = 0; i < length; i++)
            {
                if (_charactersSO[i].CharacterType == type)
                    return _charactersSO[i];
            }

            throw new System.Exception($"Could not find the character type: {type}\nin the character collections");
        }

    }
}
