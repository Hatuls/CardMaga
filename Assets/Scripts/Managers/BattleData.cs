
using Battle.Characters;
using System;
using UnityEngine;

namespace Battle.Data
{
    [Serializable]
    public class BattleData : MonoBehaviour
    {
        private static BattleData _instance;
    
        public static BattleData Instance =>_instance;
        [SerializeField]
        private Character _player = null;
        [SerializeField]
        private Character _opponent = null;
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private bool _playerWon = false;

        public Character Left { get => _player; set => _player = value; }
        public Character Right { get => _opponent; set => _opponent = value; }
        public bool PlayerWon { get => _playerWon; set => _playerWon = value; }


        public void ResetData()
        {
            _player = null;
            _opponent = null;
            _playerWon = false;
        }

        public void AssginCharacter(in bool isPlayer, CharacterSO characterSO)
            => AssginCharacter(isPlayer, characterSO.CharacterName, new Account.GeneralData.Character(characterSO));
        public void AssginCharacter(in bool isPlayer,string displayName, Account.GeneralData.Character data)
        {
            if (isPlayer)
                _player = new Character(displayName,data);
            else
                _opponent = new Character(displayName, data);
        }


        public void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
        }

    }

    /// <summary>
    /// Need Remake
    /// </summary>
    [System.Serializable]
    public class MapRewards
    {
        [SerializeField] private ushort _diamonds;
        [SerializeField] private ushort _exp;
        [SerializeField] private ushort _credits;
        [SerializeField] private ushort _gold;



        public ushort Diamonds { get => _diamonds; set => _diamonds = value; }
        public ushort EXP { get => _exp; set => _exp = value; }
        public ushort Credits { get => _credits; set => _credits = value; }
        public ushort Gold { get => _gold; set => _gold = value; }
    }
}