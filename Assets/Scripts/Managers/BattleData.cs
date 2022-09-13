
using Battle.Characters;
using CardMaga.BattleConfigSO;
using System;
using UnityEngine;

namespace Battle.Data
{
    [DefaultExecutionOrder(-9999)]
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
        private bool _isPlayerWon = false;
        [SerializeField] private BattleConfigSO _battleConfigSo;
        public BattleConfigSO BattleConfigSO { get => _battleConfigSo; }

        public Character Left { get => _player; set => _player = value; }
        public Character Right { get => _opponent; set => _opponent = value; }
        public bool PlayerWon { get => _isPlayerWon; set => _isPlayerWon = value; }


        public void ResetData()
        {
            _player = null;
            _opponent = null;
            _isPlayerWon = false;
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

  
}