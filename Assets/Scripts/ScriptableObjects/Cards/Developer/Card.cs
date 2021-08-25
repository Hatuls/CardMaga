using Keywords;
using System;

namespace Cards
{
    public enum CardTypeEnum { Utility = 2, Defend = 1, Attack = 0 };
   public enum Location {Hand=0,Discard =1,Exhaust = 2, Drawpile =3 , Crafting =4,MiddleScreenPosition =5 }
    public enum BodyPartEnum { None =0 , Head =1, Elbow=2, Hand=3, Knee=4, Leg=5 };
    public class Card
    {
        #region Fields

        private CardSO _cardSO;

        private int _cardID = 0;
        private int _currentLevel = 1;

        private KeywordData[] _cardKeyword;
        #endregion

        #region Properties

        public int GetCardID => _cardID;

        public int GetCardLevel => _currentLevel;

        public  CardSO GetSetCard
        {    private set => _cardSO = value;
             get => _cardSO;
        }

        public KeywordData[] GetCardKeywords
        {
            get {
                if (_cardKeyword == null || _cardKeyword.Length == 0)
                    _cardKeyword = _cardSO.GetCardsKeywords;

                if (_cardSO.GetCardsKeywords.Length == _cardKeyword.Length && _currentLevel >= _cardSO.GetCardLevelToUnlockKeywords)
                {
                    _cardKeyword = new KeywordData[_cardSO.GetCardsKeywords.Length + _cardSO.GetAdditionalKeywords.Length];
                    Array.Copy(_cardSO.GetCardsKeywords, _cardKeyword, _cardSO.GetCardsKeywords.Length);
                    Array.Copy(_cardSO.GetAdditionalKeywords, 0, _cardKeyword, _cardSO.GetCardsKeywords.Length, _cardSO.GetAdditionalKeywords.Length);
                }

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
            GetSetCard = _card;
        }

        public int GetKeywordAmount(KeywordTypeEnum keyword)
        {
            if (_cardSO != null && _cardSO.GetCardsKeywords.Length > 0)
            {
                for (int i = 0; i < _cardSO.GetCardsKeywords.Length; i++)
                {
                    if (_cardSO.GetCardsKeywords[i].GetKeywordSO.GetKeywordType == keyword)
                        return _cardSO.GetCardsKeywords[i].GetAmountToApply + (_currentLevel - 1) * _cardSO.GetCardsKeywords[i].GetUpgradedAmount;
                }
            }
            return 0;
        }
        #endregion


 

    }


 

}
