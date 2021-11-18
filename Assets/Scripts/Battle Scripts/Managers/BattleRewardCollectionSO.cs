using Battles;
using Cards;
using System.Collections.Generic;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Battle Reward Collection", menuName = "ScriptableObjects/Collections/Battle Rewards")]
    public class BattleRewardCollectionSO : ScriptableObject
    {
        [SerializeField]
        BattleRewardSO[] _reward;
        public void Init(BattleRewardSO[] rewardSOs)
            => _reward = rewardSOs;
        public BattleReward GetReward(CharacterTypeEnum characterTypeEnum, ActsEnum act)
        {
            for (int i = 0; i < _reward.Length; i++)
            {
                if (_reward[i].CharacterDifficultyEnum == characterTypeEnum)
                    return _reward[i].CreateReward(act);
            }
            throw new System.Exception($"Could not make reward because it was not found in the Battle reward collection\nthe character was {characterTypeEnum.ToString()}\nthe collection has in it {_reward.Length} rewards");
        }
    }


    [CreateAssetMenu(fileName = "Pack Reward Data", menuName = "ScriptableObjects/Data/Pack Rewards")]
    public class PackRewardSO : ScriptableObject
    {
        [SerializeField]
        RarityEnum _rarePack;
        public RarityEnum Rarity => _rarePack;

        public byte[] ChipChances { get => _chipChances; }
        public byte[] CardChances { get => _cardChances; }
        public byte[] RecieveChipChances { get => _recieveChipChances; }
        public byte CardAmount { get => _cardAmount; }
        public byte[] ChipAmount { get => _chipAmount; }

        [SerializeField]
        byte[] _chipChances;

        [SerializeField]
        byte[] _recieveChipChance;

        [SerializeField]
        byte[] _cardChances;



        [SerializeField]
        byte _cardAmount = 1;
        [SerializeField]
        byte[] _chipAmount;


        [SerializeField]
        PurchaseCost[] _purchaseCosts;

        public PackRewardSO Init(string[] csv)
        {
            const int PackTypeIndex = 0;
            const int ResourceTypeIndex = 1;
            const int CardAmountIndex = 2;
            const int CardChancesIndex = 3;
            const int ChipChanceIndex = 4;
            const int ChipAmountChanceIndex = 5;
            const int ChipAmountIndex = 6;

            // rarity
            if (int.TryParse(csv[PackTypeIndex], out int packType))
                _rarePack = packType == 0 ? RarityEnum.Common : RarityEnum.Rare;
                       else
                return null;

            // purchase cost
            string[] ResourceCost = csv[ResourceTypeIndex].Split('^');
            List<PurchaseCost> purchaseCosts = new List<PurchaseCost>();
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                string[] type = ResourceCost[i].Split('&');
                byte cost = 0;

                if (byte.TryParse(type[1], out byte _cost))
                    cost = _cost;
                else
                    throw new System.Exception($"PackRewardSO: Cost is not Valid! \nCoulmne:{ResourceTypeIndex}\nValue: {type[1]}");

                if (byte.TryParse(type[0], out byte _resource))
                    purchaseCosts.Add(new PurchaseCost((ResourceEnum)_resource, cost));
                else
                    throw new System.Exception($"PackRewardSO: Resource is not a valid number ");
            }
            _purchaseCosts = purchaseCosts.ToArray();

            // Card Amount
            if (byte.TryParse(csv[CardAmountIndex], out byte x))
                _cardAmount = x;
            else
                throw new System.Exception($"PackRewardSO: Card Amount is Not a Valid Number {csv[CardAmountIndex]}");

            // Card Chances
            string[] chancesString = csv[CardChancesIndex].Split('&');

            _cardChances = new byte[chancesString.Length];
            for (int i = 0; i < chancesString.Length; i++)
            {
                if (byte.TryParse(chancesString[i], out byte chance))
                    _cardChances[i] = chance;
                else
                    throw new System.Exception($"PackRewardSO: Chance is not a number! - {chancesString[i]}");
            }


            // ChipChances

            string[] Chances = csv[ChipChanceIndex].Split('^');
            _chipChances = new byte[2];
            for (int i = 0; i < Chances.Length; i++)
            {
                string[] chance = Chances[i].Split('&');
                for (int j = 0; j < chance.Length; j++)
                {
                    if (byte.TryParse(chance[j], out byte amount))
                        _chipAmount[j] = amount;
                    else
                        throw new System.Exception($"PackRewardSO: {csv[ChipChanceIndex]} IS NOT VALID!");                 
                }
            }

            _recieveChipChances = recieveChipChances;
            _chipChances = chipChances;

            _chipAmount = chipAmount;
        }
    }
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
[System.Serializable]
public class PurchaseCost
{
    [SerializeField]
    Rewards.ResourceEnum _resourceEnum;
    [SerializeField]
    ushort _price;

    public PurchaseCost(Rewards.ResourceEnum resourceEnum, ushort price)
    {
        _resourceEnum = resourceEnum;
        _price = price;
    }

    public Rewards.ResourceEnum ResourceEnum { get => _resourceEnum; }
    public ushort Price { get => _price; }
}