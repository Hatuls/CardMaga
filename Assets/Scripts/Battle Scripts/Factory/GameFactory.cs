using Account.GeneralData;
using Battles;
using Cards;
using Characters;
using Collections;
using Combo;
using CardMaga;
using Rewards;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CardMaga.ActDifficultySO;
using System.Linq;

namespace Factory
{
    public class GameFactory
    {
        public static Action OnFactoryFinishedLoading;
        private static GameFactory _instance;
        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    CardsCollectionSO cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
                    ComboCollectionSO recipeCollection = Resources.Load<ComboCollectionSO>("Collection SO/RecipeCollection");
                    CharacterCollectionSO characterCollection = Resources.Load<CharacterCollectionSO>("Collection SO/CharacterCollection");
                    BattleRewardCollectionSO battleRewardsCollection = Resources.Load<BattleRewardCollectionSO>("Collection SO/BattleRewardsCollection");
                    EventPointCollectionSO eventPointCollection = Resources.Load<EventPointCollectionSO>("Collection SO/EventPointCollection");
                    Art.ArtSO _art = Resources.Load<Art.ArtSO>("Art/AllPalette/ART BLACKBOARD");
                    Keywords.KeywordsCollectionSO keywordsCollection = Resources.Load<Keywords.KeywordsCollectionSO>("Collection SO/KeywordSOCollection");
                    _instance = new GameFactory(_art, cardCollections, recipeCollection, characterCollection, battleRewardsCollection, eventPointCollection, keywordsCollection);
                }
                return _instance;
            }
        }

        
        public ComboFactory ComboFactoryHandler { get; private set; }
        public CardFactory CardFactoryHandler { get; private set; }
        public CharacterFactory CharacterFactoryHandler { get; private set; }
        public RewardFactory RewardFactoryHandler { get; private set; }
        public EventPointFactory EventPointFactoryHandler { get; private set; }
        public KeywordFactory KeywordSOHandler { get; private set; }
        public Art.ArtSO ArtBlackBoard { get; private set; }
       

        public GameFactory(Art.ArtSO art, CardsCollectionSO cards, ComboCollectionSO comboCollectionSO, CharacterCollectionSO characters, BattleRewardCollectionSO rewards, EventPointCollectionSO eventPoints, Keywords.KeywordsCollectionSO keywords)
        {
            if (cards == null || comboCollectionSO == null || characters == null || rewards == null || eventPoints == null)
                throw new Exception("Collections is null!!");

            ArtBlackBoard = art;
          
            CardFactoryHandler = new CardFactory(cards);
            ComboFactoryHandler = new ComboFactory(comboCollectionSO);
            CharacterFactoryHandler = new CharacterFactory(characters);
            RewardFactoryHandler = new RewardFactory(rewards);
            EventPointFactoryHandler = new EventPointFactory(eventPoints);
            KeywordSOHandler = new KeywordFactory(keywords);
            Debug.Log("Factory Created<a>!</a>");

            _instance = this;

            OnFactoryFinishedLoading?.Invoke();
        }



        public class EventPointFactory
        {
            public EventPointCollectionSO EventPointCollection { get; private set; }
            public EventPointFactory(EventPointCollectionSO eventPoints)
            {
                EventPointCollection = eventPoints;
            }

            public NodePointAbstSO GetEventPoint(NodeType type)
           => EventPointCollection.GetEventPoint(type);
        }
        public class RewardFactory
        {
            public BattleRewardCollectionSO BattleRewardCollection { get; private set; }
            public RewardFactory(BattleRewardCollectionSO battleRewardCollectionSO)
            {
                BattleRewardCollection = battleRewardCollectionSO;
            }

            public BattleReward GetBattleRewards(CharacterTypeEnum characterTypeEnum,ActsEnum act, IEnumerable<Combo.Combo> workOnCombo)
           => BattleRewardCollection.GetReward(characterTypeEnum, act, workOnCombo);

            public RunReward GetRunRewards(CharacterTypeEnum characterTypeEnum, ActsEnum act)
                => BattleRewardCollection.GetRunReward(characterTypeEnum, act);
        }
        public class CharacterFactory
        {
            public CharacterCollectionSO CharacterCollection { get; private set; }
            private Dictionary<int, CharacterSO> _charactersDictionary;
            public IReadOnlyDictionary<int, CharacterSO> CharactersDictionary => _charactersDictionary;
            public CharacterFactory(CharacterCollectionSO characterCollection)
            {
                CharacterCollection = characterCollection;
                var collection = CharacterCollection.CharactersSO;
                int length = collection.Length;

                _charactersDictionary = new Dictionary<int, CharacterSO>(length);

                for (int i = 0; i < length; i++)
                    _charactersDictionary.Add(collection[i].ID, collection[i]);
            }
            public CharacterSO GetCharacterSO(CharacterTypeEnum type)
            {
                var _charactersSO = CharacterCollection.CharactersSO;
                int length = _charactersSO.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_charactersSO[i].CharacterType == type)
                        return _charactersSO[i];
                }

                throw new Exception($"Could not find the character type: {type}\nin the character collections");
            }

            public CharacterSO[] GetCharactersSO(CharacterTypeEnum type, NodeLevel NodeLevelsRange)
            {
                int rightIndex = type == CharacterTypeEnum.Elite_Enemy ? 1 : 0;
                var range = NodeLevelsRange.MinMaxCharacters[rightIndex];

                return GetCharactersSO(type).Where(
                    diffuclty =>
                    (diffuclty.CharacterDiffciulty >= range.MinDiffculty &&
                    diffuclty.CharacterDiffciulty <= range.MaxDiffculty)
                    ).ToArray();
            }
            public CharacterSO[] GetCharactersSO(CharacterTypeEnum type) => CharacterCollection.CharactersSO.Where(character => (character.CharacterType == type)).ToArray();

            public CharacterSO GetCharacterSO(CharacterEnum characterEnum)
            {
                if (characterEnum != CharacterEnum.Enemy)
                {
                    var _charactersSO = CharacterCollection.CharactersSO;
                    var length = _charactersSO.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (_charactersSO[i].CharacterEnum == characterEnum)
                            return _charactersSO[i];
                    }
                }
                throw new Exception($"Character Collection: tried to get CharacterSO from character collection through the parameter CharacterEnum: <a>{characterEnum}</a>\n check if such characterSO exist in resource folder or in the collection!");

            }
      
         

            public CharacterSO GetRandomCharacterSO(CharacterTypeEnum character, NodeLevel NodeLevelsRange)
            {
                var collection = GetCharactersSO(character, NodeLevelsRange);
                int collecitonLength = collection.Length;

                if (collecitonLength == 0)
                    throw new Exception($"CharacterFactory: Couldnt find any characterSO from the character collection based on the CharacterTypeEnum : {character}");

                return collection[UnityEngine.Random.Range(0, collecitonLength)];
            }

            public Character CreateCharacter(CharacterSO characterSO) => new Character(characterSO);

  

            public Character CreateCharacter(CharacterData data, AccountDeck _deck)
                => new Character(data, _deck);
            internal Character CreateCharacter(CharacterTypeEnum character)
            {
                var characterSO = CharacterCollection.CharactersSO;
                for (int i = 0; i < characterSO.Length; i++)
                {
                    if (characterSO[i].CharacterType == character)
                        return new Character(characterSO[i]);
                }
                throw new Exception($"Could not create Character class because {characterSO} was not found in the SO !");
            }
        }
        public class ComboFactory
        {
            public ComboCollectionSO ComboCollection { get; private set; }
            private Dictionary<int, ComboSO> _comboDictionary;
            public ComboFactory(ComboCollectionSO comboCollection)
            {
                ComboCollection = comboCollection;

                var combos =  ComboCollection.AllCombos;
                int combosLength = combos.Length;

                _comboDictionary = new Dictionary<int, ComboSO>(combosLength);

                for (int i = 0; i < combosLength; i++)
                    _comboDictionary.Add(combos[i].ID, combos[i]);

            }
            public Combo.Combo[] CreateCombos(ComboSO[] combosSO)
            {
                if (combosSO != null)
                {
                    Combo.Combo[] combos = new Combo.Combo[combosSO.Length];
                    for (int i = 0; i < combosSO.Length; i++)
                        combos[i] = CreateCombo(combosSO[i]);

                    return combos;
                }
                return null;
            }

            public Combo.Combo CreateCombo(ComboSO comboSO, int level = 0)
               => new Combo.Combo(comboSO, level);

            public Combo.Combo[] CreateCombo(CombosAccountInfo[] characterCombos)
            {
                if (characterCombos == null)
                    throw new Exception("Combo Factory: characterCombos is null!");

                int length = characterCombos.Length;
                Combo.Combo[] combos = new Combo.Combo[length];
                for (int i = 0; i < length; i++)
                {
                    combos[i] = CreateCombo(characterCombos[i]);
                }
                return combos;
            }
            public ComboSO[] GetComboSOFromIDs(IEnumerable<int> ids)
            {
                return ids.Select(x => GetComboSO(x)).ToArray();
            }
            public ComboSO GetComboSO(int id)
            {
                if (_comboDictionary.TryGetValue(id, out var combo))
                    return combo;
                throw new Exception("ComboFactory: Combo ID Is not valid in the dictionary!");
            }
            private Combo.Combo CreateCombo(CombosAccountInfo combosAccountInfo)
            {
                if (combosAccountInfo == null)
                    throw new Exception("Combo Factory: CombosAccountInfo is null!");

                return new Combo.Combo(GetComboSO(combosAccountInfo.ID), combosAccountInfo.Level);
            }
        }
        public class CardFactory
        {
            static List<int> _battleCardIdList;
            static int _battleID;
            public CardsCollectionSO CardCollection { get; private set; }
            private Dictionary<int, CardSO> _cardCollectionDictionary;
       

            public CardFactory(CardsCollectionSO cards)
            {
                CardCollection = cards;
                var allCards = CardCollection.GetAllCards;
                int length = allCards.Length;
                _cardCollectionDictionary = new Dictionary<int, CardSO>(length);

                for (int i = 0; i < length; i++)
                    _cardCollectionDictionary.Add(allCards[i].ID, allCards[i]);
                
                Reset();
            }

            ~CardFactory()
            {
                _cardCollectionDictionary.Clear();
                _battleCardIdList?.Clear();
                _battleID = 1;
            }

            public CardSO GetCard(int ID)
            {

                if (_cardCollectionDictionary.TryGetValue(ID, out CardSO card))
                    return card;

                throw new System.Exception($"Card SO Could not been found from ID \nID is {ID}\nCheck Collection For card SO");
            }
            public static int GenerateCardInstanceID()
            {
         
                    if (_battleCardIdList == null)
                        _battleCardIdList = new List<int>();

                    while (_battleCardIdList.Contains(_battleID))
                        ++_battleID;

                    _battleCardIdList.Add(_battleID);

                    return _battleID;
                
            }

            public Card[] CreateDeck(CardInstanceID.CardCore[] cardsInfo)
            {
                if (cardsInfo != null && cardsInfo.Length != 0)
                {
                    Card[] cards = new Card[cardsInfo.Length];

                    for (int i = 0; i < cards.Length; i++)
                    {
                        if (cardsInfo[i] != null)
                            cards[i] = CreateCard(cardsInfo[i].CardSO(), cardsInfo[i].Level);
                    }
                    return cards;
                }
                return null;
            }

            public void Reset()
            {
                if (_battleCardIdList == null)
                    _battleCardIdList = new List<int>();

                    _battleCardIdList.Clear();
            }
            public void RegisterAccountLoadedCardsInstanceID(List<CardInstanceID> accountsCards)
            {
                for (int i = 0; i < accountsCards.Count; i++)
                    _battleCardIdList.Add(accountsCards[i].InstanceID);
            }
            public CardInstanceID CreateCardCoreInfo(CardSO cardSO, int level = 0, int exp = 0)
                => CreateCardCoreInfo(cardSO.ID, level, exp);
            public CardInstanceID CreateCardCoreInfo(int cardSOID, int level = 0,int exp = 0)
            => new CardInstanceID(cardSOID, GenerateCardInstanceID(), level,exp);
            public Card CreateCard(CardInstanceID _data)
            {
                if (_data == null)
                    throw new Exception($"CardFactory: CardCoreInfo is null!");
                return new Card(_data);
            }
            public Card CreateCard(int CardSOID, int level = 0)
             => CreateCard(GetCard(CardSOID), level);

            public Card CreateCard(CardSO cardSO, int level = 0)
            {

                if (cardSO != null && (level >= 0 && level <= cardSO.CardsMaxLevel))
                {
                    return CreateCard(CreateCardCoreInfo(cardSO,level));
                }
                throw new Exception($" card was not created!\nCardSO is :{cardSO} Level: {level} MaxLevel {cardSO.CardsMaxLevel}");

            }
            public Card[] CreateDeck(CardInstanceID[] cards)
            {
                Card[] c = new Card[cards.Length];
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = CreateCard(cards[i]);
                    cards[i].InstanceID = c[i].CardInstanceID;
                }
                return c;
            }
         
            internal Card[] CreateDeck(AccountDeck deck)
            {
                if (deck == null)
                    throw new Exception("Card Factory: AccountDeck is null!");

                int length = deck.Cards.Length;
                var cardsDataHolder = deck.Cards;
                Card[] cards = new Card[length];

                for (int i = 0; i < length; i++)
                {
                    var cardContainer = cardsDataHolder[i];

                    if (cardContainer == null)
                        throw new Exception($"CardFactory: card was not created!\nCard at index: {i}  is null!");

                    cards[i] = CreateCard(cardContainer);

                    if (cards[i] == null)
                        throw new Exception($"Card Factory: Card Was Not Created From:\n ID: {cardContainer.ID}\nLevel: {cardContainer.Level}");

                }

                return cards;
            }
        }


        public class KeywordFactory
        {
            Keywords.KeywordsCollectionSO _keywordCollection;
            public KeywordFactory(Keywords.KeywordsCollectionSO keyword)
            {
                _keywordCollection = keyword;
            }
            public Keywords.KeywordSO GetKeywordSO(Keywords.KeywordTypeEnum type)
                => _keywordCollection.GetKeywordSO(type);
        }
    }

}