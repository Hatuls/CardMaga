using UnityEngine;
using Keywords;
namespace Cards
{
    [System.Serializable]
    public class PerLevelUpgrade
    {
        public PerLevelUpgrade(Upgrade[] upgrades)
            => _upgradesPerLevel = upgrades;

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
            public Upgrade(LevelUpgradeEnum levelUpgradeEnum,int amount)
            {
                _upgradeType = levelUpgradeEnum;
                _amount = amount;
            }


            [SerializeField] private LevelUpgradeEnum _upgradeType;
            [SerializeField] private KeywordData _keywordUpgrade;
            [SerializeField] private int _amount;
            public LevelUpgradeEnum UpgradeType => _upgradeType;
            public KeywordData KeywordUpgrade => _keywordUpgrade;
            public int Amount => _amount;
        }
    }
}