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
        private int _salvageChipsAddition;
        public int SalvageChipsAddition => _salvageChipsAddition;


        [SerializeField]
        private Upgrade[] _upgradesPerLevel;
        public Upgrade[] UpgradesPerLevel => _upgradesPerLevel;

        [System.Serializable]
        public class Upgrade
        {
            [SerializeField] private LevelUpgradeEnum _upgradeType;
            [SerializeField] private KeywordTypeEnum _keywordReference;
            [SerializeField] private int _animationIndex;
            [SerializeField] private int _amount;
            public LevelUpgradeEnum UpgradeType => _upgradeType;
            public KeywordTypeEnum KeywordRefernce => _keywordReference;
            public int AnimationIndex => _animationIndex;
            public int Amount => _amount;
        }
    }
}