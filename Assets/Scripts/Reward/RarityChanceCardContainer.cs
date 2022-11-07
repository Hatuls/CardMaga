using Collections;
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class RarityChanceCardContainer
    {
        [SerializeField]
        private RarityCardsContainer _packCardsRewards;

        [SerializeField]
        private float _chance;

        public float Chance { get => _chance; }
        public RarityCardsContainer PackCardsRewards { get => _packCardsRewards; }

#if UNITY_EDITOR
        public void InitChance(float chance , RarityCardsContainer rarityCardsContainer)
        {
            _chance = chance;
            _packCardsRewards = rarityCardsContainer;
        }
#endif
    }


}