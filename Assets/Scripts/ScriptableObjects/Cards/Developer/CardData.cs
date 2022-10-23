using Account.GeneralData;
using Battle;
using CardMaga.Card;
using CardMaga.Commands;
using Keywords;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Card
{
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1, None = 0, };

    public enum BodyPartEnum { None = 0, Empty = 1, Head = 2, Elbow = 3, Hand = 4, Knee = 5, Leg = 6, Joker = 7 };

    [Serializable]
    public class CardData : IEquatable<CardData>
    {
        #region Fields
        [SerializeField]
        private CardSO _cardSO;
        [SerializeField]
        private CardInstanceID _cardCoreInfo;
        [SerializeField]
        private bool _toExhaust = false;


        [SerializeField]
        CardTypeData _cardTypeData;

        [SerializeField]
        private KeywordData[] _cardKeyword;

        [SerializeField]
        private int _staminaCost;


        private CardCommandsHolder _cardCommandsHolder;
        #endregion

        #region Properties

        public CardTypeData CardTypeData
        {
            get => _cardTypeData;
        }
        public bool IsExhausted { get => _toExhaust; }
        public BodyPartEnum BodyPartEnum { get => _cardTypeData.BodyPart; }
        public int CardInstanceID => _cardCoreInfo.InstanceID;
        public int CardLevel => _cardCoreInfo.Level;
        public int CardEXP => _cardCoreInfo.Exp;
        public bool CardsAtMaxLevel { get => _cardSO.CardsMaxLevel - 1 == CardLevel; }
        public int StaminaCost { get => _staminaCost; private set => _staminaCost = value; }
        public CardCommandsHolder CardCommands
        {
            get => _cardCommandsHolder;

            private set => _cardCommandsHolder = value;
        }


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

        public CardInstanceID CardCoreInfo => _cardCoreInfo;


        #endregion

        #region Functions
        public CardData()
        {

        }
        public CardData(CardInstanceID cardAccountInfo)
        {
            _cardCoreInfo = cardAccountInfo ?? throw new Exception($"Card: Card Info is null!");

            InitCard(Factory.GameFactory.Instance.CardFactoryHandler.GetCard(_cardCoreInfo.ID), _cardCoreInfo.Level);
        }
        public void InitCard(CardSO _card, int cardsLevel)
        {
            _cardSO = _card;
            _cardKeyword = CreateKeywords(_cardSO, cardsLevel).ToArray();

            _cardCommandsHolder = new CardCommandsHolder(this);
        }
        internal void InitCommands(bool isLeft, IPlayersManager playersManager, KeywordManager _keywordManager)
        {
            if (CardCommands == null)
                InitCard(CardSO, CardLevel);

            var player = playersManager.GetCharacter(isLeft);
            CardCommands.CardsKeywords.Init(isLeft, playersManager, _keywordManager);
            CardCommands.CardTypeCommand.Init(player.CraftingHandler);
            CardCommands.AnimationCommand.Init(player.VisualCharacter.AnimatorController);
        }
        private List<KeywordData> CreateKeywords(CardSO _card, int cardsLevel)
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
                        _toExhaust = upgrade.Amount == 1 ? true : false;
                        break;
                    case LevelUpgradeEnum.BodyPart:
                        _cardTypeData = upgrade.CardTypeData;
                        break;
                    case LevelUpgradeEnum.None:
                    default:
                        break;
                }
            }

            return keywordsList;
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

        public bool Equals(CardData other) => _cardCoreInfo.Equals(other._cardCoreInfo);
        public bool TryGetKeyword(KeywordTypeEnum keyword, out int amount)
        {
            amount = 0;
            var keywords = CardKeywords;
            KeywordData current;
            bool found = false;
            for (int i = 0; i < keywords.Length; i++)
            {
                current = keywords[i];
                bool result = current.KeywordSO.GetKeywordType == keyword;
                if (result)
                {
                    amount += current.GetAmountToApply;
                    found |= result;
                }
            }
            return found;
        }



        public CardData Clone()
       => new CardData(new CardInstanceID(_cardCoreInfo.GetCardCore()));




#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void Refresh()
        {
            var newCore = new CardCore(_cardSO.ID, _cardCoreInfo?.Level ?? 0, _cardCoreInfo?.Exp ?? 0);
            _cardCoreInfo = new CardInstanceID(newCore);

            _cardTypeData = _cardSO.CardType;
        }

#endif
    }
    #endregion
}

public class CardCommandsHolder
{
    private CardTypeCommand _cardTypeCommand;
    private CardsKeywords _cardsKeywords;
    private AnimationVisualCommand _animationCommand;
    public CardsKeywords CardsKeywords { get => _cardsKeywords; private set => _cardsKeywords = value; }
    public AnimationVisualCommand AnimationCommand { get => _animationCommand; private set => _animationCommand = value; }
    public CardTypeCommand CardTypeCommand { get => _cardTypeCommand; private set => _cardTypeCommand = value; }

    public CardCommandsHolder(CardData cardData)
    {
        CardTypeCommand = new CardTypeCommand(cardData);
        CardsKeywords = new CardsKeywords(cardData);
        AnimationCommand = new AnimationVisualCommand(cardData.CardSO, CommandType.AfterPrevious);
    }
}
public static class CardHelper
{
    public static CardData[] CloneCards(this CardData[] cards)
    {
        CardData[] newArray = new CardData[cards.Length];
        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = cards[i].Clone();

        return newArray;
    }
}
