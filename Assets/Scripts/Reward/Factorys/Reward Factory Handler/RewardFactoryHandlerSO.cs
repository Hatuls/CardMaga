using UnityEngine;
namespace CardMaga.Rewards.Factory.Handlers
{
    [CreateAssetMenu(fileName = "new Reward Factory Handler", menuName = "ScriptableObjects/Rewards/Handler/New Reward Handler")]
    public class RewardFactoryHandlerSO : ScriptableObject
    {
        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private RewardType _rewardType;
        [SerializeField]
        private BaseRewardFactorySO[] _factories;
        public int ID => (int)_rewardType;
        public RewardType RewardType => _rewardType;
        public BaseRewardFactorySO GetRewardFactory(int id)
        {
            for (int i = 0; i < _factories.Length; i++)
            {
                if (_factories[i].ID == id)
                                   return _factories[i];
            }
            throw new System.Exception($"RewardFactoryHandlerSO - Could not find request CoreID = {id}\nFactoryID = {ID}");
        }

#if UNITY_EDITOR
        public void Init(BaseRewardFactorySO[] factorySOs)
            => _factories = factorySOs;
        public void SetID(RewardType id) => _rewardType = id;
#endif
    }
}