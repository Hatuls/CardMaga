using CardMaga.Battle.Players;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.Rewards
{
    public abstract class BaseRewardFactorySO : ScriptableObject, ITaggable
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private string _name;
        [SerializeField]
        protected RewardTagSO[] _tags;
        public int ID => _id;
        public string Name => _name;

        public virtual IEnumerable<TagSO> Tags => _tags;

        public abstract IRewardable GenerateReward();


#if UNITY_EDITOR
        public void AssignValues(int id, string name)
        {
            _id = id;
            _name = name;
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
