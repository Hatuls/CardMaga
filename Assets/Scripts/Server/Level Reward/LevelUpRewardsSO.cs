using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.MetaData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Levels/Rewards")]
public class LevelUpRewardsSO : ScriptableObject
    {
        //[SerializeField]
        //private int[] _currentLevel;
        [SerializeField,Tooltip("The Max EXP to Level Up")]
        private int[] _maxLevel;
        [SerializeField,Tooltip("The reward received when leveling to this level")]
        private int[] _rewardID;



        public LevelData GetLevelData(int currentLevel)
        {
            int index = GetIndex(currentLevel);
            return new LevelData(_maxLevel[index], _rewardID[index]);
        }


        private int GetIndex(int level)
        {
            //for (int i = 0; i < _currentLevel.Length; i++)
            //{
            //    if (_currentLevel[i] == level)
            //        return i;
            //}
            return level - 1;
            throw new Exception("Level Was Not Found - " + level);
        }

        public struct LevelData
        {
            public readonly int MaxEXP;
            public readonly int RewardID;
            public LevelData(int maxEXP, int rewardID)
            {
                MaxEXP = maxEXP;
                RewardID = rewardID;
            }
        }

        #region Editor

        public void Init(int[] maxEXP, int[] giftRewardID)
        {
            _maxLevel = maxEXP;
            _rewardID = giftRewardID;
        }
        #endregion

    }

}