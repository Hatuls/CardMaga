using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Special Pack Reward Factory", menuName = "ScriptableObjects/Rewards/Packs/New Special Pack Reward")]
    public class SpecialCardsPackRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private int _amountOfCards;
        [SerializeField]
        private RarityChanceCardContainer[] _packRewards;
        public override IRewardable GenerateReward()
        {
            var _packReward = new PackReward();
            //_packReward.Init(Name, cardIDs);
            return _packReward;
        }
#if UNITY_EDITOR
        public void Init(int[] cardIDs)
        {

        }
#endif
    }
}
