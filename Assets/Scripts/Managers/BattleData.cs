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



        [SerializeField]
        private bool _useSO;
        public bool UseSO => _useSO;


        private Character _player;
        private Character _opponent;

        public Character PlayerCharacterData => _player;
        public Character OpponentCharacterData => _opponent;


    }
}