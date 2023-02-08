using System;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Basic Pack Reward Factory", menuName = "ScriptableObjects/Rewards/Packs/New Basic Pack Reward")]
    public class CardsPackRewardFactorySO : BaseRewardFactorySO 
    {
        [SerializeField]
        private int _amountOfCards;
        [SerializeField]
        private RarityChanceCardContainer[] _packRewards;
        public override IRewardable GenerateReward()
        {
            var _packReward = new PackReward(Name, GenerateCards());
            return _packReward;
        }

        private int[] GenerateCards()
        {
            int[] ids = new int[_amountOfCards];
            for (int i = 0; i < _amountOfCards; i++)
            {
                RarityChanceCardContainer cardContainer = RandomizeContainer();
                System.Collections.Generic.IReadOnlyList<int> allCards = cardContainer.PackCardsRewards.CardsID;
                ids[i] = allCards[UnityEngine.Random.Range(0, allCards.Count)];
            }
            return ids;
        }
        private RarityChanceCardContainer RandomizeContainer()
        {
            float previousValue = 0;
            float chance = UnityEngine.Random.Range(0f, 100f);
            for (int i = 0; i < _packRewards.Length; i++)
            {
                if (chance < previousValue+ _packRewards[i].Chance)
                   return _packRewards[i];
                previousValue += _packRewards[i].Chance;
            }
            throw new Exception($"{Name} could not acquire a RarityChance Container");
        }
#if UNITY_EDITOR
        public RarityChanceCardContainer[] RarirtyContainer => _packRewards;
        public void Init(int amountOfCards,params RarityChanceCardContainer[] _cardsPool)
        {
            _amountOfCards = amountOfCards;
            _packRewards = _cardsPool;
        }
#endif
    }
}
