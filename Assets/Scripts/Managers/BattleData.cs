using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace Battles
{
    [CreateAssetMenu(fileName = "Battle Data", menuName = "ScriptableObjects/Battle Data")]
    public class BattleData : ScriptableObject
    {
        [OdinSerialize]
        [ShowInInspector]
        public CharacterSO OpponentOne { get; set; }
        [OdinSerialize]
        [ShowInInspector]
        public CharacterSO OpponentTwo { get; set; }
    }
}