using UnityEngine;
using Keywords;
namespace Cards
{
    [System.Serializable]
    public class PerLevelUpgrade
    {
        [SerializeField]
        private int _costForLevelUp;
        public int CostForLevelUp => _costForLevelUp;

        [SerializeField]
        private Upgrade[] _upgradesPerLevel;
        public Upgrade[] UpgradesPerLevel => _upgradesPerLevel;

        [System.Serializable]
        public class Upgrade
        {
            [SerializeField] private LevelUpgradeEnum _upgradeType;
            [SerializeField] private KeywordData _keywordUpgrade;
            [SerializeField] private int _amount;
            public LevelUpgradeEnum UpgradeType => _upgradeType;
            public KeywordData KeywordUpgrade => _keywordUpgrade;
            public int Amount => _amount;
        }
    }
}