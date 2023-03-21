
using Battle.Characters;
using CardMaga.BattleConfigSO;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Battle.Data
{
    [DefaultExecutionOrder(-9999)]
    [Serializable]
    public class BattleData : MonoBehaviour
    {
        private static BattleData _instance;
    [TitleGroup("BattleData")]
        public static BattleData Instance => _instance;
        [SerializeField]
        [TabGroup("BattleData", "Player")]
        private BattleCharacter _player = null;
        [SerializeField]
        [TabGroup("BattleData", "Opponent")]
        private BattleCharacter _opponent = null;
        [SerializeField]//, Sirenix.OdinInspector.ReadOnly]
        private bool _isPlayerWon = false;
        [TabGroup("BattleData", "Config")]
        [SerializeField] 
        private BattleConfigSO _battleConfigSo;
        public BattleConfigSO BattleConfigSO { get => _battleConfigSo; }

        public BattleCharacter Left { get => _player; set => _player = value; }
        public BattleCharacter Right { get => _opponent; set => _opponent = value; }
        public bool IsPlayerWon { get => _isPlayerWon; set => _isPlayerWon = value; }
        
        public void ResetData()
        {
            _player = null;
            _opponent = null;
            _isPlayerWon = false;
        }

        public void AssginBattleTutorialData(TutorialConfigSO tutorialConfigSo)
        {
            _player = tutorialConfigSo.LeftCharacter;
            _opponent = tutorialConfigSo.RightCharacter;

            _battleConfigSo = tutorialConfigSo.BattleConfig;
        }
        public void AssignUserCharacter(string displayName, Account.GeneralData.Character data)
        {
            _player = new BattleCharacter(displayName,data);
        }

        public void AssignOpponent(string displayName, Account.GeneralData.Character data)
            => _opponent = new BattleCharacter(displayName, data, _player);
        public void AssignOpponent(BattleCharacter opponent)
        { _opponent = opponent; }
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