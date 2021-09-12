using Keywords;
using System;

namespace Cards
{
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1 ,None = 0,};

    public enum BodyPartEnum { None =0 ,Empty = 1, Head =2, Elbow=3, Hand=4, Knee=5, Leg=6 ,Joker = 7};
    public class Card
    {
        #region Fields

        private CardSO _cardSO;

        private int _cardID = 0;
        private int _currentLevel = 1;

        public int StaminaCost { get; private set; }
        public BodyPartEnum BodyPartEnum { get; private set; }


        private KeywordData[] _cardKeyword;
        #endregion

        #region Properties

        public int CardID => _cardID;

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


                //needs to re - implement
                //if (_cardSO.GetCardsKeywords.Length == _cardKeyword.Length && _currentLevel >= _cardSO.GetCardLevelToUnlockKeywords)
                //{
                //    _cardKeyword = new KeywordData[_cardSO.GetCardsKeywords.Length + _cardSO.GetAdditionalKeywords.Length];
                //    Array.Copy(_cardSO.GetCardsKeywords, _cardKeyword, _cardSO.GetCardsKeywords.Length);
                //    Array.Copy(_cardSO.GetAdditionalKeywords, 0, _cardKeyword, _cardSO.GetCardsKeywords.Length, _cardSO.GetAdditionalKeywords.Length);
                //}

                return _cardKeyword;
            }
        }
           

        #endregion


        #region Functions
        public Card(int specificCardID, CardSO _card)
        {
            _cardID = specificCardID;
            this._cardSO = _card;
            InitCard(specificCardID, _card);
        }

        public void InitCard(int specificCardID, CardSO _card) {
            _cardID = specificCardID;
            CardSO = _card;
            StaminaCost = _card.StaminaCost;
            BodyPartEnum = _card.BodyPartEnum;
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
                    BodyPartEnum = (BodyPartEnum)levelUpgrade.Amount;
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
