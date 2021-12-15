
using Characters;
using UnityEngine;
namespace Battles
{
    [System.Serializable]
    public class BattleData
    {
        [SerializeField]
        private MapRewards _mapRewards = null;
        [SerializeField]
        private Character _player = null;
        [SerializeField]
        private Character _opponent = null;
        [SerializeField]
        private bool _isFinishedPlaying = false;
        [SerializeField]
        private bool _playerWon = false;
        [SerializeField]
        Map.Map _map;



        public Map.Map Map { get => _map; set => _map = value; }
        public MapRewards MapRewards { get => _mapRewards; set => _mapRewards = value; }
        public Character Player { get => _player; set => _player = value; }
        public Character Opponent { get => _opponent; set => _opponent = value; }
        public bool PlayerWon { get => _playerWon; set => _playerWon = value; }
        public bool IsFinishedPlaying { get => _isFinishedPlaying; set => _isFinishedPlaying = value; }

        public void ResetData()
        {
            _mapRewards = null;
            _player = null;
            _opponent = null;
            _isFinishedPlaying = false;
            _playerWon = false;
            _map = null;
        }
    }


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