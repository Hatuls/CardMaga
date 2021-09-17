using UnityEngine;


namespace Battles
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollection :ScriptableObject
    {
        [SerializeField]
        private CharacterSO[] _charactersSO;
        public CharacterSO[] CharactersSO { get => _charactersSO; }


        public void Init(CharacterSO[] characterSOs)
            => _charactersSO = characterSOs;

    }
}
