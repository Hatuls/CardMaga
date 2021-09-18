using Keywords;
using System;
using System.Collections.Generic;

namespace Cards
{
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1 ,None = 0,};

    public enum BodyPartEnum { None =0 ,Empty = 1, Head =2, Elbow=3, Hand=4, Knee=5, Leg=6 ,Joker = 7};
    public class Card
    {
        #region Fields

        private CardSO _cardSO;

        private int _cardBattleID = 0;
        private int _currentLevel =0;
        private bool toExhaust = false;
        public int StaminaCost { get; private set; }
        CardTypeData _cardTypeData;
        public BodyPartEnum BodyPartEnum { get => _cardTypeData.BodyPart; }


        private KeywordData[] _cardKeyword;
        #endregion

        #region Properties

        public int CardID => _cardBattleID;

        public int CardLevel => _currentLevel;

        public  CardSO CardSO
        {    private set => _cardSO = value;
             get => _cardSO;
        }

        public KeywordData[] CardKeywords
        {
            get {
                if (_cardKeyword == null || _cardKeyword.Length == 0)
                    _cardKeyword = _cardSO.CardSOKeywords;

                return _cardKeyword;
            }
        }
           

        #endregion


        #region Functions
        public Card(int battleCardID, CardSO _card, int cardsLevel)
        {   
            _cardBattleID = battleCardID;
            InitCard(_card, cardsLevel);
        }

        public void InitCard( CardSO _card, int cardsLevel) 
        {
            toExhaust = _card.ToExhaust;

            _currentLevel = cardsLevel;
            var levelUpgrade = _card.GetLevelUpgrade(_currentLevel);
            List<KeywordData> keywordsList = new List<KeywordData>(1);
            for (int i = 0; i < levelUpgrade.UpgradesPerLevel.Length; i++)
            {
                var upgrade = levelUpgrade.UpgradesPerLevel[i];
         
                switch (upgrade.UpgradeType)
                {
              
                    case LevelUpgradeEnum.Stamina:
                        StaminaCost = upgrade.Amount;
                        break;
                    case LevelUpgradeEnum.KeywordAddition:
                        keywordsList.Add(upgrade.KeywordUpgrade);
                        break;
                    case LevelUpgradeEnum.ConditionReduction:
                        break;
                    case LevelUpgradeEnum.ToRemoveExhaust:
                        toExhaust = upgrade.Amount == 1 ? true : false;
                        break;
                    case LevelUpgradeEnum.BodyPart:
                        _cardTypeData = upgrade.CardTypeData;
                        break;
                    case LevelUpgradeEnum.None:
                    default:
                        break;
                }
            }
            _cardKeyword = keywordsList.ToArray();
            this._cardSO = _card;
        }

        public int GetKeywordAmount(KeywordTypeEnum keyword)
        {
            int amount = 0;
            if (_cardSO != null && _cardSO.CardSOKeywords.Length > 0)
            {
                for (int i = 0; i < CardKeywords.Length; i++)
                {
                    if (CardKeywords[i].KeywordSO.GetKeywordType == keyword)
                        amount += CardKeywords[i].GetAmountToApply;
                }
            }
            return amount;
        }


        public bool LevelUpCard()
        {
            if (_currentLevel >= CardSO.CardsMaxLevel)
                return false;

            _currentLevel++;

            SpreadUpgrades();
            return true;
        }

        private void SpreadUpgrades()
        {
          var levelUpgrade = _cardSO.PerLevelUpgrade[_currentLevel - 1];
           
            if (levelUpgrade.UpgradesPerLevel.Length == 0)
                    return;
          
            for (int i = 0; i < levelUpgrade.UpgradesPerLevel.Length; i++)
            {
                if (levelUpgrade.UpgradesPerLevel[i] == null)
                    continue;
                UpgradeHandler(levelUpgrade.UpgradesPerLevel[i]);
            }

        }
        private void UpgradeHandler(PerLevelUpgrade.Upgrade levelUpgrade)
        {

            switch (levelUpgrade.UpgradeType)
            {
                case LevelUpgradeEnum.Stamina:
                    StaminaCost = levelUpgrade.Amount;
                    break;



                case LevelUpgradeEnum.BodyPart:
            
                    _cardTypeData = levelUpgrade.CardTypeData;
                    break;


                     case LevelUpgradeEnum.KeywordAddition:

                    Array.Resize<KeywordData>(ref _cardKeyword, _cardKeyword.Length + 1);

                    _cardKeyword[_cardKeyword.Length - 1] = levelUpgrade.KeywordUpgrade;

                    break;
                case LevelUpgradeEnum.ConditionReduction:
                    break;

                case LevelUpgradeEnum.None:
                default:
                    return;

            }


        }
        #endregion




    }


 

}
