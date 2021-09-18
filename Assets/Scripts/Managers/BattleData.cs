using UnityEngine;

namespace Battles
{
    [CreateAssetMenu(fileName = "Battle Data", menuName = "ScriptableObjects/Battle Data")]
    public class BattleData : ScriptableObject
    {
        [SerializeField]
        private CharacterSO _opponentOne;
        [SerializeField]
        private CharacterSO _opponentTwo;
        public CharacterSO OpponentOne { get => _opponentOne; set => _opponentOne = value; }
     
        public CharacterSO OpponentTwo { get=> _opponentTwo; set => _opponentTwo= value; }
    }
}