using CardMaga.Battle.Players;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.Rewards
{
    public abstract class BaseRewardFactorySO : ScriptableObject
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private string _name;
        [SerializeField,ReadOnly,EnumToggleButtons]
        protected RewardType _tags;
        public int ID => _id;
        public string Name => _name;
        public abstract IRewardable GenerateReward();

        public virtual RewardType RewardTypes=> _tags;
#if UNITY_EDITOR
        public void AssignValues(int id, string name, RewardType type)
        {
            _id = id;
            _name = name;
            _tags = type;
        }
#endif
    }



    //[Serializable]
    //public class RewardTypeAndID
    //{
    //    [SerializeField]
    //    private int rewardTypeID;
    //    [SerializeField] private int rewardID;

    //    public int RewardID { get => rewardID; }
    //    public int RewardTypeID { get => rewardTypeID; }
    //}
}
