using System;
using UnityEngine;


namespace CardMaga.Rewards
{
    public abstract class BaseRewardFactorySO : ScriptableObject
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private string _name;
        public int ID => _id;
        public string Name => _name;
        public abstract IRewardable GenerateReward();

#if UNITY_EDITOR
        public void AssignValues(int id, string name)
        {
            _id = id;
            _name = name;
        }
#endif
    }



    [Serializable]
    public class RewardTypeAndID
    {
        [SerializeField]
        private int rewardTypeID;
        [SerializeField] private int rewardID;

        public int RewardID { get => rewardID; }
        public int RewardTypeID { get => rewardTypeID; }
    }
}
