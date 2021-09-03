using Keywords;
using System;

namespace Cards
{
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1 ,None = 0,};

    public enum BodyPartEnum { None =0 , Head =1, Elbow=2, Hand=3, Knee=4, Leg=5 ,Joker = 6};
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
                    _cardKeyword = _cardSO.GetCardsKeywords;


                // needs to re-implement
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
        public Card()
        {
            UnityEngine.Debug.Log("A Card was created with no ID or ScriptableObject in it");

        }
        public void InitCard(int specificCardID, CardSO _card) {
            _cardID = specificCardID;
            CardSO = _card;
            StaminaCost = _card.GetStaminaCost;
            BodyPartEnum = _card.GetBodyPartEnum;
        }

        public int GetKeywordAmount(KeywordTypeEnum keyword)
        {
            if (_cardSO != null && _cardSO.GetCardsKeywords.Length > 0)
            {
                for (int i = 0; i < _cardSO.GetCardsKeywords.Length; i++)
                {
                    if (_cardSO.GetCardsKeywords[i].GetKeywordSO.GetKeywordType == keyword)
                        return _cardSO.GetCardsKeywords[i].GetAmountToApply;
                }
            }
            return 0;
        }


        public bool LevelUpCard()
        {
            if (_currentLevel >= CardSO.CardsMaxLevel)
                return false;

            _currentLevel++;

            UpgradeCard();
            return true;
        }

        private void UpgradeCard()
        {
          var levelUpgrade = _cardSO.PerLevelUpgrade[_currentLevel - 1];
          
            for (int i = 0; i < levelUpgrade.UpgradesPerLevel.Length; i++)
            {
                switch (levelUpgrade.UpgradesPerLevel[i].UpgradeType)
                {
                 
                    case LevelUpgradeEnum.Stamina:
                        StaminaCost = levelUpgrade.UpgradesPerLevel[i].Amount;
                        break;



                    case LevelUpgradeEnum.BodyPart:
                        BodyPartEnum = (BodyPartEnum)levelUpgrade.UpgradesPerLevel[i].Amount;
                        break;


                    case LevelUpgradeEnum.UpgradeKeywords:

                        for (int j = 0; j < CardKeywords.Length; j++)
                        {
                            if (CardKeywords[j].GetKeywordSO.GetKeywordType == levelUpgrade.UpgradesPerLevel[i].KeywordRefernce)
                                CardKeywords[j].GetAmountToApply = levelUpgrade.UpgradesPerLevel[i].Amount;
                        }


                        break;
                    case LevelUpgradeEnum.ConditionReduction:
                        break;


                    case LevelUpgradeEnum.None:
                    default:
                        return;
                        
                }


            }

        }

        #endregion


 

    }


 

}
