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


        [SerializeField]
        private Character _player;
        [SerializeField]
        private Character _opponent;

        public Character PlayerCharacterData => _player;
        public Character OpponentCharacterData => _opponent;
        public void UpdatePlayerCharacter(Character data)
        => _player =  data;
        
        public void Initbattle(CharacterTypeEnum opponent,Character data = null)
        {
            if (UseSO == true)
                return;

                var characterFacory = Factory.GameFactory.Instance.CharacterFactoryHandler;

            UpdatePlayerCharacter(data);

              _opponent = characterFacory.CreateCharacter(opponent);
        }
        [Sirenix.OdinInspector.Button]
        public void ResetPlayerStats() => _player = null;
    }
}