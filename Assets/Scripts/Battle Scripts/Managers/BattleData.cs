
using Characters;
using Rewards;
using System;
using UnityEngine;
namespace Battles
{
    [System.Serializable]
    public class BattleData
    {
        [SerializeField]
        private MapRewards[] _mapRewards = null;
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

        [SerializeField]
        ActsEnum _currentAct;


        public MapRewards this[CharacterTypeEnum characterTypeEnum]
        {
            get
            {

                if (_mapRewards == null)
                    ResetMapRewards();
                

                const int offsetToZero = 2;
                int index = (int)characterTypeEnum - offsetToZero;
                return _mapRewards[index];
            }
        }

        public void ResetMapRewards()
        {
            const int characters = 4;
            _mapRewards = new MapRewards[characters];
        }

        public Map.Map Map { get => _map; set => _map = value; }
        public Character Player { get => _player; set => _player = value; }
        public Character Opponent { get => _opponent; set => _opponent = value; }
        public bool PlayerWon { get => _playerWon; set => _playerWon = value; }
        public bool IsFinishedPlaying { get => _isFinishedPlaying; set => _isFinishedPlaying = value; }
        public ActsEnum CurrentAct { get => _currentAct; set => _currentAct = value; }


        public ushort GetAllDiamonds()
        => GetAllFromMapRewards((x) => x.Diamonds);
        public ushort GetAllExp()
            => GetAllFromMapRewards(x => x.EXP);
        public ushort GetAllGold()
        => GetAllFromMapRewards(x => x.Gold);
        public ushort GetAllCredits()
            => GetAllFromMapRewards(x => x.Credits);
        private ushort GetAllFromMapRewards(Func<MapRewards, ushort> operation)
        {
            ushort value = 0;
            for (int i = 0; i < _mapRewards.Length; i++)
                value += operation.Invoke(_mapRewards[i]);

            return value;
        }
        public void ResetData()
        {
            _mapRewards = null;
            _player = null;
            _opponent = null;
            _isFinishedPlaying = false;
            _playerWon = false;
            _map = null;
            _currentAct = ActsEnum.ActOne;
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