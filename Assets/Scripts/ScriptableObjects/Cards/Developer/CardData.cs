using Account.GeneralData;
using Battle;
using CardMaga.Card;
using CardMaga.Commands;
using Keywords;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        internal void InitCommands(bool isLeft, IPlayersManager playersManager, KeywordManager keywordManager)
        {
            if (CardCommands == null)
                InitCard(CardSO, CardLevel);

            var player = playersManager.GetCharacter(isLeft);
            CardCommands.InitCardKeywords(isLeft, playersManager, keywordManager);
            CardCommands.CardTypeCommand.Init(player.CraftingHandler);
            CardCommands.StaminaCostCommand.Init(player.StaminaHandler);
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

public class CardCommandsHolder : ICommand
{
    private StaminaCostCommand _staminaCostCommand;
    private CardTypeCommand _cardTypeCommand;
    private CardsKeywordsCommands[] _cardsKeywords;
    public StaminaCostCommand StaminaCostCommand { get => _staminaCostCommand; private set => _staminaCostCommand = value; }
    public CardsKeywordsCommands[] CardsKeywords { get => _cardsKeywords; private set => _cardsKeywords = value; }
    public CardTypeCommand CardTypeCommand { get => _cardTypeCommand; private set => _cardTypeCommand = value; }

    public CardCommandsHolder(CardData cardData)
    {
        StaminaCostCommand = new StaminaCostCommand(cardData);
        CardTypeCommand = new CardTypeCommand(cardData);


        KeywordData[] keywords = cardData.CardKeywords;
        keywords.Sort(OrderKeywords);

        int highestAnimationIndex = keywords[keywords.Length - 1].AnimationIndex + 1;
        CardsKeywords = new CardsKeywordsCommands[highestAnimationIndex];
        for (int i = 0; i < highestAnimationIndex; i++)
        {
            IEnumerable<KeywordData> animationIndexList = keywords.Where(x => x.AnimationIndex == i);
            CardsKeywords[i] = new CardsKeywordsCommands(animationIndexList , cardData.CardSO.AnimationBundle.AttackAnimation.Length ==0 ? CommandType.WithPrevious : CommandType.AfterPrevious);
        }
    }

    public void InitCardKeywords(bool isLeft, IPlayersManager playersManager, KeywordManager keywordManager)
    {
        for (int i = 0; i < CardsKeywords.Length; i++)
            CardsKeywords[i].Init(isLeft, playersManager, keywordManager);
    }


    private int OrderKeywords(KeywordData myself, KeywordData other)
    {
        if (myself.AnimationIndex == other.AnimationIndex)
            return 0;
        else if (myself.AnimationIndex < other.AnimationIndex)
            return -1;
        else return 1;
    }


    public void Execute()
    {
        for (int i = 0; i < CardsKeywords.Length; i++)
            CardsKeywords[i].Execute();


        CardTypeCommand.Execute();
    }

    public void Undo()
    {
        for (int i = CardsKeywords.Length - 1; i >= 0; i--)
            CardsKeywords[i].Undo();
        CardTypeCommand.Undo();
    }
}
