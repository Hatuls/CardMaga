using Account.GeneralData;
using Cards;
using Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Pack Reward Data", menuName = "ScriptableObjects/Data/Pack Rewards")]
    public class PackRewardSO : ScriptableObject
    {

        [TitleGroup("Packs")]
        [SerializeField]
      public  string PackName;
        public byte CardAmount { get => _cardAmount; }
        public byte[] CardChances { get => _cardChances; }
        public byte[] RecieveChipChances { get => _recieveChipChance; }
        public byte[] ChipChances { get => _chipChances; }
        public byte[] ChipAmount { get => _chipAmount; }

        public ResourceStock[] PurchaseCosts => _purchaseCosts;

        [TabGroup("Packs/Category", "Chip Amount Chances")]
        [SerializeField]
        byte[] _chipChances;
        [TabGroup("Packs/Category", "Chip Amount")]
        [SerializeField]
        byte[] _chipAmount;
        [TabGroup("Packs/Category", "Chances To Recieve Chip")]
        [SerializeField]
        byte[] _recieveChipChance;



        [TabGroup("Packs/Category", "Card Rarity Chance")]
        [SerializeField]
        byte[] _cardChances;
        [SerializeField]
        byte _cardAmount = 1;

        [TabGroup("Packs/Category", "Cost")]
        [SerializeField]
        ResourceStock[] _purchaseCosts;
        [SerializeField]

        [TabGroup("Packs/Category", "Card Rarity Chance")]
        CardsCollectionSO.RarityCards[] _dropChances;
        public PackReward CreatePackReward()
        {
        int cardRarity = (ChanceHelper.GetRandomIndexByChances(_cardChances));
            int recieveChipChanceResult = ChanceHelper.GetRandomIndexByChances(_recieveChipChance);

            ResourceStock resourceStock = null;
            if (recieveChipChanceResult == 1)
            {
                int chipAmountChance = ChanceHelper.GetRandomIndexByChances(_chipChances);
                resourceStock = new ResourceStock(ResourceEnum.Credits, _chipAmount[chipAmountChance]);
            }
            var handler = Factory.GameFactory.Instance.CardFactoryHandler;
            ushort cardSO = _dropChances[cardRarity].CardsID[Random.Range(0, _dropChances[cardRarity].CardsID.Length)];

            return new PackReward(handler.CreateCardCoreInfo(cardSO), resourceStock);
        }

        #region CSV
#if UNITY_EDITOR
        public void LoadRewardCards(CardsCollectionSO cards)
        {
            var allCards = cards.GetAllCards;
            int length = allCards.Length;
            List<ushort> _commonList = new List<ushort>();
            List<ushort> _unCommonList = new List<ushort>();
            List<ushort> _rareList = new List<ushort>();
            List<ushort> _epicList = new List<ushort>();
            List<ushort> _legendaryList = new List<ushort>();

            for (int i = 0; i < length; i++)
            {
                var currentCard = allCards[i];

                if (currentCard.IsPackReward)
                {
                    switch (currentCard.Rarity)
                    {

                        case RarityEnum.Common:
                            _commonList.Add(currentCard.ID);
                            break;
                        case RarityEnum.Uncommon:
                            _unCommonList.Add(currentCard.ID);
                            break;
                        case RarityEnum.Rare:
                            _rareList.Add(currentCard.ID);
                            break;
                        case RarityEnum.Epic:
                            _epicList.Add(currentCard.ID);
                            break;
                        case RarityEnum.LegendREI:
                            _legendaryList.Add(currentCard.ID);
                            break;
                        case RarityEnum.None:
                        default:
                            throw new System.Exception($"PackRewardSO: Rarity - ({currentCard.Rarity}) Is Not Valid\nCard ID : {currentCard.ID}");

                    }


                }

            }

            _dropChances = new CardsCollectionSO.RarityCards[5];
            _dropChances[0] = new CardsCollectionSO.RarityCards(_commonList.ToArray(), RarityEnum.Common);
            _dropChances[1] = new CardsCollectionSO.RarityCards(_unCommonList.ToArray(), RarityEnum.Uncommon);
            _dropChances[2] = new CardsCollectionSO.RarityCards(_rareList.ToArray(), RarityEnum.Rare);
            _dropChances[3] = new CardsCollectionSO.RarityCards(_epicList.ToArray(), RarityEnum.Epic);
            _dropChances[4] = new CardsCollectionSO.RarityCards(_legendaryList.ToArray(), RarityEnum.LegendREI);
        }
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

            PackName = csv[PackTypeIndex];
            if (PackName == "-")
                return false;

            // purchase cost
            string[] ResourceCost = csv[ResourceTypeIndex].Split('^');
            List<ResourceStock> purchaseCosts = new List<ResourceStock>();
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                string[] type = ResourceCost[i].Split('&');
                byte cost = 0;

                if (byte.TryParse(type[1], out byte _cost))
                    cost = _cost;
                else
                    throw new System.Exception($"PackRewardSO: Cost is not Valid! \nCoulmne:{ResourceTypeIndex}\nValue: {type[1]}");

                if (byte.TryParse(type[0], out byte _resource))
                    purchaseCosts.Add(new ResourceStock((ResourceEnum)_resource, cost));
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
                for (int j = 0; j < chances.Length; j++)
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
#endif
        #endregion
    }
    public class PackReward
    {
        public ResourceStock Reward { get; private set; }
        public CardCoreInfo RewardCard { get; private set; }

        public PackReward(CardCoreInfo rewardCard, ResourceStock reward = null)
        {
            Reward = reward;
            RewardCard = rewardCard;
        }
    }




    public static class ChanceHelper
    {
        public static int GetRandomIndexByChances(params byte[] chances)
        {
            const int OneHundred = 100;
            int random = (byte)Random.Range(0, OneHundred);
            int addition = 0;

            // find  BattleDropChance.RarityDropChance from the random number
            for (int j = 0; j < chances.Length; j++)
            {
                if (j > 0)
                    addition += chances[j - 1];

                if (random < (addition + chances[j]))
                {
                    return j;
                }
            }

            throw new System.Exception($"Could Not Retrieve Index From Chances {chances}");
        }
    }
}
