using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;
namespace CardMaga.Rewards.Factory.Handlers
{
    [CreateAssetMenu(fileName = "new Reward Factory Manager", menuName = "ScriptableObjects/Rewards/Manager/New Reward Manager")]
    public class RewardFactoryManagerSO : ScriptableObject
    {

        [SerializeField]
        private RewardFactoryHandlerSO[] _factories;

        public BaseRewardFactorySO GetRewardFactory(RewardType rewardType, int id) 
            => GetRewardFactoryHandler(rewardType)?.GetRewardFactory(id);

        public RewardFactoryHandlerSO GetRewardFactoryHandler(RewardType rewardType)
        {
   
            for (int i = 0; i < _factories.Length; i++)
            {
                if (_factories[i].RewardType.Contain(rewardType))
                    return _factories[i];
            }
            throw new System.Exception($"RewardFactoryManagerSO - Could not find request ID = {rewardType}\n");
        }

#if UNITY_EDITOR
        public void Add(RewardFactoryHandlerSO rewardFactoryHandlerSOs)
        {
            if (_factories == null)
                _factories = new RewardFactoryHandlerSO[0];
            List<RewardFactoryHandlerSO> list = _factories.ToList();
            list.Add(rewardFactoryHandlerSOs);
            _factories = list.ToArray();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}