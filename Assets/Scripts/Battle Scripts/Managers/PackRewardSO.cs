using Cards;
using System.Collections.Generic;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Pack Reward Data", menuName = "ScriptableObjects/Data/Pack Rewards")]
    public class PackRewardSO : ScriptableObject
    {
        [SerializeField]
        RarityEnum _rarePack;
        public RarityEnum Rarity => _rarePack;

        public byte CardAmount { get => _cardAmount; }
        public byte[] CardChances { get => _cardChances; }
        public byte[] RecieveChipChances { get => _recieveChipChance; }
        public byte[] ChipChances { get => _chipChances; }
        public byte[] ChipAmount { get => _chipAmount; }

        public PurchaseCost[] PurchaseCosts => _purchaseCosts;

        [SerializeField]
        byte[] _chipChances;
        [SerializeField]
        byte[] _chipAmount;
        [SerializeField]
        byte[] _recieveChipChance;

    

        [SerializeField]
        byte[] _cardChances;
        [SerializeField]
        byte _cardAmount = 1;


        [SerializeField]
        PurchaseCost[] _purchaseCosts;

        public bool Init(string[] csv)
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
                return false;

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
            _recieveChipChance = new byte[2];
            for (int i = 0; i < Chances.Length; i++)
            {
                string[] chance = Chances[i].Split('&');
                for (int j = 0; j < chance.Length; j++)
                {
                    if (byte.TryParse(chance[j], out byte amount))
                        _recieveChipChance[j] = amount;
                    else
                        throw new System.Exception($"PackRewardSO: {csv[ChipChanceIndex]} IS NOT VALID!");                 
                }
            }


            // chip Amount chances
            string[] chipAmount = csv[ChipAmountChanceIndex].Split('^');
            for (int i = 0; i < chipAmount.Length; i++)
            {
                string[] chances = chipAmount[i].Split('&');

            _chipChances = new byte[chances.Length];
                for (int j = 0; j <chances.Length; j++)
                {
                    if (byte.TryParse(chances[j], out byte value))
                        _chipChances[j] = value;
                    else
                        throw new System.Exception("PackRewardSO: Chip Chance Is not a valid Number " + chances[j]);
                }
            }

            // chip amount
            string[] s = csv[ChipAmountIndex].Split('&');
            _chipAmount = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                if (byte.TryParse(s[i], out byte value))
                    _chipAmount[i] = value;
                else
                    throw new System.Exception($"Pack Amount: Value is not a valid number {s[i]}");
            }
            return true;
        }
    }

}
