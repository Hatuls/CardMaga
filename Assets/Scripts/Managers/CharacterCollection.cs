using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Battles
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollection :ScriptableObject
    {
        [OdinSerialize]
        [ShowInInspector]
        public CharacterSO[] CharactersSO { get; private set; }


        public void Init(CharacterSO[] characterSOs)
            => CharactersSO = characterSOs;

    }
}
