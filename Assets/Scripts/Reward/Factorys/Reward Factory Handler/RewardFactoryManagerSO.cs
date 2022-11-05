using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Rewards.Factory.Handlers
{
    [CreateAssetMenu(fileName = "new Reward Factory Manager", menuName = "ScriptableObjects/Rewards/Manager/New Reward Manager")]
    public class RewardFactoryManagerSO : ScriptableObject
    {

        [SerializeField]
        private RewardFactoryHandlerSO[] _factories;
        public BaseRewardFactorySO[] GetRewardFactorySOs(params RewardTypeAndID[] rewardTypeAndIDs)
        {
            List<BaseRewardFactorySO> factorys = new List<BaseRewardFactorySO>();
            for (int i = 0; i < rewardTypeAndIDs.Length; i++)
                factorys.Add(GetRewardFactory(rewardTypeAndIDs[i]));

            return factorys.ToArray();
        }
        public BaseRewardFactorySO GetRewardFactory(RewardTypeAndID rewardTypeAndID)
            => GetRewardFactory(rewardTypeAndID.RewardTypeID, rewardTypeAndID.RewardID);
        public BaseRewardFactorySO GetRewardFactory(int rewardTypeID, int id) 
            => GetRewardFactoryHandler(rewardTypeID)?.GetRewardFactory(id);

        public RewardFactoryHandlerSO GetRewardFactoryHandler(int rewardTypeID)
        {
            for (int i = 0; i < _factories.Length; i++)
            {
                if (_factories[i].ID == rewardTypeID)
                    return _factories[i];
            }
            throw new System.Exception($"RewardFactoryManagerSO - Could not find request ID = {rewardTypeID}\n");
        }
    }
}