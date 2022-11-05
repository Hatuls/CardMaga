using UnityEngine;
namespace CardMaga.Rewards.Factory.Handlers
{
    [CreateAssetMenu(fileName = "new Reward Factory Handler", menuName = "ScriptableObjects/Rewards/Handler/New Reward Handler")]
    public class RewardFactoryHandlerSO : ScriptableObject
    {
        private int _iD;
        [SerializeField]
        private BaseRewardFactorySO[] _factories;
        public int ID => _iD;
        public BaseRewardFactorySO GetRewardFactory(int id)
        {
            for (int i = 0; i < _factories.Length; i++)
            {
                if (_factories[i].ID == id)
                    return _factories[i];
            }
            throw new System.Exception($"RewardFactoryHandlerSO - Could not find request ID = {id}\nFactoryID = {ID}");
        }
    }
}