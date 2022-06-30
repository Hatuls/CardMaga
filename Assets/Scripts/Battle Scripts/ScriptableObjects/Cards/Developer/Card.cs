using Account.GeneralData;
using Keywords;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cards
{
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1, None = 0, };


    public enum BodyPartEnum { None = 0, Empty = 1, Head = 2, Elbow = 3, Hand = 4, Knee = 5, Leg = 6, Joker = 7 };
    [Serializable]
    public class Card : IEquatable<Card>
    {
        #region Fields
        [SerializeField]
        private CardSO _cardSO;
        [SerializeField]
        private CardInstanceID _cardCoreInfo;
        [SerializeField]
        private bool toExhaust = false;


        [SerializeField]
        CardTypeData _cardTypeData;

        [SerializeField]
        private KeywordData[] _cardKeyword;

        [SerializeField]
        private int staminaCost;
        #endregion

        #region Properties

        public bool IsExhausted { get => toExhaust; }
        public BodyPartEnum BodyPartEnum { get => _cardTypeData.BodyPart; }
        public int CardInstanceID => _cardCoreInfo.InstanceID;
        public int CardLevel => _cardCoreInfo.Level;
        public int CardEXP => _cardCoreInfo.Exp;
        public bool CardsAtMaxLevel { get => _cardSO.CardsMaxLevel-1 == CardLevel; }
        public int StaminaCost { get => staminaCost; private set => staminaCost = value; }

        public CardSO CardSO
        {
            private set => _cardSO = value;
            get => _cardSO;
        }

        public KeywordData[] CardKeywords
        {
            get
            {
                if (_cardKeyword == null || _cardKeyword.Length == 0)
                    _cardKeyword = _cardSO.CardSOKeywords;

                return _cardKeyword;
            }
        }

        public CardInstanceID CardCoreInfo { get => _cardCoreInfo; }


        #endregion


        #region Functions
        public Card()
        {

        }
        public Card(CardInstanceID cardAccountInfo)
        {
            if (cardAccountInfo == null)
                throw new Exception($"Card: Card Info is null!");
            _cardCoreInfo = cardAccountInfo;
            InitCard(Factory.GameFactory.Instance.CardFactoryHandler.GetCard(_cardCoreInfo.ID), _cardCoreInfo.Level);
        }
        public void InitCard(CardSO _card, int cardsLevel)
        {
    

            var levelUpgrade = _card.GetLevelUpgrade(cardsLevel);
            List<KeywordData> keywordsList = new List<KeywordData>(1);
            for (int i = 0; i < levelUpgrade.UpgradesPerLevel.Length; i++)
            {
                var upgrade = levelUpgrade.UpgradesPerLevel[i];

                switch (upgrade.UpgradeType)
                {

                    case LevelUpgradeEnum.Stamina:
                        StaminaCost = (byte)upgrade.Amount;
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

        public bool Equals(Card other)
        {
            return _cardCoreInfo == other._cardCoreInfo;
        }


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void Refresh()
        {
            var newCore = new CardCore(_cardSO.ID, _cardCoreInfo?.Level ?? 0, _cardCoreInfo?.Exp ?? 0);
            _cardCoreInfo = new CardInstanceID(newCore);
        }
#endif
    }
    #endregion




}


 


