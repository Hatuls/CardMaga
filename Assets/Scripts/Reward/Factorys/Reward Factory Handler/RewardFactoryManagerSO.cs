using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
#endif
using UnityEngine;
namespace CardMaga.Rewards.Factory.Handlers
{
    [CreateAssetMenu(fileName = "new Reward Factory Manager", menuName = "ScriptableObjects/Rewards/Manager/New Reward Manager")]
    public class RewardFactoryManagerSO : ScriptableObject
    {

        [SerializeField]
        private RewardFactoryHandlerSO[] _factories;

        public BaseRewardFactorySO GetRewardFactory(int rewardTypeID, int id) 
            => GetRewardFactoryHandler(rewardTypeID)?.GetRewardFactory(id);

        public RewardFactoryHandlerSO GetRewardFactoryHandler(int rewardTypeID)
        {
            for (int i = 0; i < _factories.Length; i++)
            {
                if (_factories[i].ID == rewardTypeID)
                    return _factories[i];
            }
            throw new System.Exception($"RewardFactoryManagerSO - Could not find request CoreID = {rewardTypeID}\n");
        }

#if UNITY_EDITOR
        public void Add(RewardFactoryHandlerSO rewardFactoryHandlerSOs)
        {
            if (_factories == null)
                _factories = new RewardFactoryHandlerSO[0];
            List<RewardFactoryHandlerSO> list = _factories.ToList();
            list.Add(rewardFactoryHandlerSOs);
            _factories = list.ToArray();
        }
#endif
    }
}