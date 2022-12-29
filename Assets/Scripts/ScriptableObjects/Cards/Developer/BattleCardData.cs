using Account.GeneralData;
using CardMaga.Battle;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.Card
{
    [Serializable]
    public enum CardTypeEnum { Utility = 3, Defend = 2, Attack = 1, None = 0, };

    public enum BodyPartEnum { None = 0, Empty = 1, Head = 2, Elbow = 3, Hand = 4, Knee = 5, Leg = 6, Joker = 7 };

    [Serializable]
    public class BattleCardData : IEquatable<BattleCardData>
    {
        #region Fields

        [SerializeField]
        private CardInstance _cardInstance;
        [SerializeField]
        private bool _toExhaust = false;


        [SerializeField]
        private CardTypeData _cardTypeData;

        [SerializeField]
        private KeywordData[] _cardKeyword;

        [SerializeField]
        private int _staminaCost;


        private CardCommandsHolder _cardCommandsHolder;
        #endregion

        #region Properties

        public CardTypeData CardTypeData => _cardTypeData;
        public CardInstance CardInstance => _cardInstance;
        public bool IsExhausted { get => _toExhaust; }
        public BodyPartEnum BodyPartEnum { get => _cardTypeData.BodyPart; }
        public int CardLevel => _cardInstance.Level;
        public CardSO CardSO => _cardInstance.CardSO;
        public bool CardsAtMaxLevel { get => CardSO.CardsMaxLevel - 1 == CardLevel; }
        public int StaminaCost { get => _staminaCost; private set => _staminaCost = value; }
        public CardCommandsHolder CardCommands
        {
            get => _cardCommandsHolder;

            private set => _cardCommandsHolder = value;
        }

        public KeywordData[] CardKeywords
        {
            get
            {
                if (_cardKeyword == null || _cardKeyword.Length == 0)
                    _cardKeyword = CardSO.CardSOKeywords;
           
                return _cardKeyword;
            }
        }

        #endregion

        #region Functions

        public BattleCardData(CardInstance cardAccountInfo)
        {
            _cardInstance = cardAccountInfo ?? throw new Exception($"BattleCard: BattleCard Info is null!");
            InitCard();
        }

        public void InitCard()
        {
            _cardKeyword = CreateKeywords(CardSO, _cardInstance.Level).ToArray();
            _cardCommandsHolder = new CardCommandsHolder(this);
        }

        internal void InitCommands(bool isLeft, IPlayersManager playersManager, KeywordManager keywordManager)
        {
            if (CardCommands == null)
                InitCard();

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

        public int GetKeywordAmount(KeywordType keyword)
        {
            int amount = 0;
            if (CardSO != null && CardSO.CardSOKeywords.Length > 0)
            {
                for (int i = 0; i < CardKeywords.Length; i++)
                {
                    if (CardKeywords[i].KeywordSO.GetKeywordType == keyword)
                        amount += CardKeywords[i].GetAmountToApply;
                }
            }
            return amount;
        }

        public bool Equals(BattleCardData other) => _cardInstance.Equals(other._cardInstance);

        public bool TryGetKeyword(KeywordType keyword, out int amount)
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


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void RefreshFromCoreID()
        {
            _cardInstance = new CardInstance(new CardCore(new CoreID(_cardInstance.CoreID)));
            
             _cardTypeData = CardSO.CardType;
        }
        [Sirenix.OdinInspector.Button]
        private void RefreshFromSOAndLevel()
        {
            _cardInstance = new CardInstance(new CardCore(CardSO.ID+ _cardInstance.Level) );
            _cardTypeData = CardSO.CardType;
        }
        public BattleCardData()
        {

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

    public CardCommandsHolder(BattleCardData battleCardData)
    {
        StaminaCostCommand = new StaminaCostCommand(battleCardData);
        CardTypeCommand = new CardTypeCommand(battleCardData);


        KeywordData[] keywords = battleCardData.CardKeywords;
        keywords.Sort(OrderKeywords);

        int highestAnimationIndex = keywords[keywords.Length - 1].AnimationIndex + 1;
        CardsKeywords = new CardsKeywordsCommands[highestAnimationIndex];
        for (int i = 0; i < highestAnimationIndex; i++)
        {
            IEnumerable<KeywordData> animationIndexList = keywords.Where(x => x.AnimationIndex == i);
            CardsKeywords[i] = new CardsKeywordsCommands(animationIndexList, battleCardData.CardSO.AnimationBundle.AttackAnimation.Length == 0 ? CommandType.WithPrevious : CommandType.AfterPrevious);
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
