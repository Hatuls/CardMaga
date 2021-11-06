using UnityEngine;
using Keywords;
namespace Cards
{
    [System.Serializable]
    public class PerLevelUpgrade
    {
        public PerLevelUpgrade(Upgrade[] upgrades, string info)
        {
          _upgradesPerLevel = upgrades;
            _description = info;
        }
[SerializeField]
        private string _description;
        public string Description => _description;
        [SerializeField]
        private Upgrade[] _upgradesPerLevel;
        public Upgrade[] UpgradesPerLevel => _upgradesPerLevel;

        [System.Serializable]
        public class Upgrade
        {
            public Upgrade (KeywordData keyword)
            {
                _upgradeType = LevelUpgradeEnum.KeywordAddition;
                _keywordUpgrade = keyword;
            }

            public Upgrade(CardTypeData cardTypeData)
            {
                _upgradeType = LevelUpgradeEnum.BodyPart;
                _cardTypeData = cardTypeData;
            }
            public Upgrade(LevelUpgradeEnum levelUpgradeEnum,int amount)
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
}