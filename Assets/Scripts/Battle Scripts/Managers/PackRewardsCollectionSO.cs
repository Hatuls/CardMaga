using Cards;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Pack Reward Collection", menuName = "ScriptableObjects/Collection/Pack Rewards Collection")]
    public class PackRewardsCollectionSO : ScriptableObject
    {

        [SerializeField]
        private PackRewardSO[] _packs;


        public void Init(PackRewardSO[] packRewardSOs)
        {
            _packs = packRewardSOs;
        }
        public PackRewardSO PackRewardSO(RarityEnum Rarity)
        {
            switch (Rarity)
            {
                case RarityEnum.Common:
                case RarityEnum.Uncommon:
                    for (int i = 0; i < _packs.Length; i++)
                    {
                        if (_packs[i].Rarity == RarityEnum.Uncommon || _packs[i].Rarity == RarityEnum.Common)
                            return _packs[i];
                    }
                    break;
                case RarityEnum.Rare:
                case RarityEnum.Epic:
                case RarityEnum.LegendREI:
                    for (int i = 0; i < _packs.Length; i++)
                    {
                        if (_packs[i].Rarity == RarityEnum.Rare || _packs[i].Rarity == RarityEnum.Epic || _packs[i].Rarity == RarityEnum.LegendREI)
                            return _packs[i];
                    }
                    break;
                case RarityEnum.None:
                default:

                    break;
            }
            throw new System.Exception($"PackRewardsCollectionSO: Rarity Pack Is not Valid!");
        }
    }

}
