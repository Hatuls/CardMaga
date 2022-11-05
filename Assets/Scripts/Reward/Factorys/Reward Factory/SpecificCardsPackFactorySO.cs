using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Specific Pack Reward Factory", menuName = "ScriptableObjects/Rewards/Packs/New Specific Pack Reward")]
    public class SpecificCardsPackFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private PackReward _packReward;
        public override IRewardable GenerateReward()
        => _packReward;

#if UNITY_EDITOR
        public void Init(int[] cardIDs)
        {
            _packReward = new PackReward();
            _packReward.Init(Name, cardIDs);
        }
#endif
    }
}
