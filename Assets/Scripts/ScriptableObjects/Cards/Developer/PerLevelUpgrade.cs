using CardMaga.Card;
using CardMaga.Keywords;
using Keywords;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Cards
{
    [System.Serializable]
    public class PerLevelUpgrade
    {
        public PerLevelUpgrade(Upgrade[] upgrades, List<string[]> info, ushort costOfUpgrade, int upgradeValue)
        {
            _upgradesPerLevel = upgrades;
            _upgradesPerLevel.OrderBy(x => x.UpgradeType).Select(x => x.KeywordUpgrade).OrderBy(x => x.KeywordSO.GetKeywordType);
            _cardValue = upgradeValue;
               _purchaseCost = costOfUpgrade;

            _description = new DescriptionInfo[info.Count];

            for (int i = 0; i < info.Count; i++)
                _description[i]= new DescriptionInfo { Description = info[i] };
        }


        [SerializeField]
        private DescriptionInfo[] _description;
        public DescriptionInfo[] Description => _description ;
        [SerializeField]
        private ushort _purchaseCost;
        public ushort PurchaseCost { get => _purchaseCost; set => _purchaseCost = value; }
        [SerializeField]
        private Upgrade[] _upgradesPerLevel;
        public Upgrade[] UpgradesPerLevel => _upgradesPerLevel;

        [SerializeField]
        private int _cardValue;
        public int CardValue => _cardValue;
        [System.Serializable]
        public class Upgrade
        {
            public Upgrade(KeywordData keyword)
            {
                _upgradeType = LevelUpgradeEnum.KeywordAddition;
                _keywordUpgrade = keyword;
            }

            public Upgrade(CardTypeData cardTypeData)
            {
                _upgradeType = LevelUpgradeEnum.BodyPart;
                _cardTypeData = cardTypeData;
            }
            public Upgrade(LevelUpgradeEnum levelUpgradeEnum, int amount)
            {
                _upgradeType = levelUpgradeEnum;
                _amount = amount;
            }


            [SerializeField] private LevelUpgradeEnum _upgradeType;
            [SerializeField] private KeywordData _keywordUpgrade;
            [SerializeField] private CardTypeData _cardTypeData;
            [SerializeField] private int _amount;
            public LevelUpgradeEnum UpgradeType => _upgradeType;
            public KeywordData KeywordUpgrade => _keywordUpgrade;
            public CardTypeData CardTypeData => _cardTypeData;
            public int Amount => _amount;



            
        }

   
    }

    [System.Serializable]
    public class DescriptionInfo
    {
        public string[] Description;
    }
}