using Account.GeneralData;
using Battle;
using Collections;
using Battle.Combo;
using CardMaga.Card;
using CardMaga.Keywords;
using Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

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
                    LoadFromResources();
                }
                return _instance;
            }
        }

        private static void LoadFromResources()
        {
            //throw new Exception("Factory is null!");
            CardsCollectionSO cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
            ComboCollectionSO recipeCollection = Resources.Load<ComboCollectionSO>("Collection SO/RecipeCollection");
            CharacterCollectionSO characterCollection = Resources.Load<CharacterCollectionSO>("Collection SO/CharacterCollection");

            Keywords.KeywordsCollectionSO keywordsCollection = Resources.Load<Keywords.KeywordsCollectionSO>("Collection SO/KeywordSOCollection");
            _instance = new GameFactory(cardCollections, recipeCollection, characterCollection, keywordsCollection);
        }

        public ComboFactory ComboFactoryHandler { get; private set; }
        public CardFactory CardFactoryHandler { get; private set; }
        public CharacterFactory CharacterFactoryHandler { get; private set; }
        public RewardFactory RewardFactoryHandler { get; private set; }
        public KeywordFactory KeywordFactoryHandler { get; private set; }


        public GameFactory(CardsCollectionSO cards, ComboCollectionSO comboCollectionSO, CharacterCollectionSO characters, KeywordsCollectionSO keywords)
        {
            if (cards == null || comboCollectionSO == null || characters == null || keywords == null)
                throw new Exception("Collections is null!!");

            CardFactoryHandler = new CardFactory(cards);
            ComboFactoryHandler = new ComboFactory(comboCollectionSO);
            CharacterFactoryHandler = new CharacterFactory(characters);
            //   RewardFactoryHandler = new RewardFactory(rewards);
            KeywordFactoryHandler = new KeywordFactory(keywords);
            Debug.Log("Factory Created<a>!</a>");

            _instance = this;

            OnFactoryFinishedLoading?.Invoke();
        }

        public class RewardFactory
        {
            // public BattleRewardCollectionSO BattleRewardCollection { get; private set; }
            // public RewardFactory(BattleRewardCollectionSO battleRewardCollectionSO)
            // {
            //     BattleRewardCollection = battleRewardCollectionSO;
            // }

            // public BattleReward GetBattleRewards(CharacterTypeEnum characterTypeEnum, ActsEnum act, IEnumerable<Battle.Combo.ComboData> workOnCombo)
            //=> BattleRewardCollection.GetReward(characterTypeEnum, act, workOnCombo);

            // public RunReward GetRunRewards(CharacterTypeEnum characterTypeEnum, ActsEnum act)
            //     => BattleRewardCollection.GetRunReward(characterTypeEnum, act);
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

            public CharacterSO GetCharacterSO(int id)
            {
                return CharacterCollection.CharactersSO.First(x => x.ID == id) ?? throw new Exception("CharacterFactory: CharacterID was not found\nID requested - " + id);
            }


            public Character CreateCharacter(CharacterSO characterSO) => new Character(characterSO);




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
            private Dictionary<int, Battle.Combo.ComboSO> _comboDictionary;
            public ComboFactory(ComboCollectionSO comboCollection)
            {
                ComboCollection = comboCollection;

                var combos = ComboCollection.AllCombos;
                int combosLength = combos.Length;

                _comboDictionary = new Dictionary<int, Battle.Combo.ComboSO>(combosLength);

                for (int i = 0; i < combosLength; i++)
                    _comboDictionary.Add(combos[i].ID, combos[i]);
            }

            public MetaComboData GetMetaComboData(ComboCore comboCore)
            {
                return new MetaComboData(comboCore);
            }

            public List<MetaComboData> GetMetaComboData(ComboCore[] comboCores)
            {
                List<MetaComboData> output = new List<MetaComboData>(comboCores.Length);

                foreach (var comboCore in comboCores)
                {
                    output.Add(new MetaComboData(comboCore));
                }

                return output;
            }

            public BattleComboData[] CreateCombos(ComboCore[] combosSO)
            {
                if (combosSO != null)
                {
                    List<BattleComboData> combos = new List<BattleComboData>();
                    
                    for (int i = 0; i < combosSO.Length; i++)
                    {
                        if(combosSO[i].ID != 0)
                         combos.Add(CreateCombo(combosSO[i].ComboSO()));
                    }

                    return combos.ToArray();
                }
                return null;
            }

            public BattleComboData CreateCombo(ComboSO comboSO, int level = 0)
               => new BattleComboData(comboSO, level);

            public ComboSO[] GetComboSOFromIDs(IEnumerable<int> ids)
            {
                return ids.Select(x => GetComboSO(x)).ToArray();
            }
            public ComboSO GetComboSO(int id)
            {
                if (_comboDictionary.TryGetValue(id, out var combo))
                    return combo;
                throw new Exception("ComboFactory: Combo CoreID Is not valid in the dictionary!\nID: " + id);
            }

        }
        public class CardFactory
        {
            static List<CardCore> _battleCardIdList;

            public CardsCollectionSO CardCollection { get; private set; }
            private Dictionary<int, CardSO> _cardCollectionDictionary;


            public CardFactory(CardsCollectionSO cards)
            {
                CardCollection = cards;
                var allCards = CardCollection.GetAllCardsSO;
                int length = allCards.Length;
                _cardCollectionDictionary = new Dictionary<int, CardSO>(length);

                foreach (var cardID in CardCollection.GetAllCardsSO)
                    foreach (var cardLevel in cardID.CardCore)
                        _cardCollectionDictionary.Add(cardLevel.CardCore.CardID, cardID);

                _battleCardIdList = new List<CardCore>();

                for (int i = 0; i < length; i++)


                    Reset();
            }

            ~CardFactory()
            {
                _cardCollectionDictionary.Clear();
                _battleCardIdList?.Clear();

            }

            public CardSO GetCard(int ID)
            {

                if (_cardCollectionDictionary.TryGetValue(ID, out CardSO card))
                    return card;

                throw new System.Exception($"BattleCard SO Could not been found from CoreID \nID is {ID}\nCheck Collection For battleCard SO");
            }

            public MetaCardData GetMetaCardData(CardCore cardCore)
            {
                CardSO cardSo = GetCard(cardCore.CardID);

                CardInstance instance = CreateCardInstance(cardCore);
                
                BattleCardData battleCardData = CreateCard(instance);

                return new MetaCardData(instance, cardSo, battleCardData);
            }

            public List<MetaCardData> GetMetaCardData(CoreID[] cardCores)
            {
                List<MetaCardData> output = new List<MetaCardData>(cardCores.Length);

                foreach (var cardCore in cardCores)
                {
                    CardSO cardSo = GetCard(cardCore.ID);

                    CardInstance instance = CreateCardInstance(cardCore.ID);
                
                    BattleCardData battleCardData = CreateCard(instance);
                    
                    output.Add(new MetaCardData(instance, cardSo, battleCardData));
                }
                
                return output;
            }
            
            public List<MetaCardData> GetMetaCardData(CardCore[] cardCores)
            {
                List<MetaCardData> output = new List<MetaCardData>(cardCores.Length);

                foreach (var cardCore in cardCores)
                {
                    CardSO cardSo = GetCard(cardCore.CardID);

                    CardInstance instance = CreateCardInstance(cardCore);
                
                    BattleCardData battleCardData = CreateCard(instance);
                    
                    output.Add(new MetaCardData(instance, cardSo, battleCardData));
                }
                
                return output;
            }
            
            public BattleCardData[] CreateDeck(CoreID[] coreIDs)
            {
                CardCore[] cards = new CardCore[coreIDs.Length];
                for (int i = 0; i < cards.Length; i++)
                    cards[i] = new CardCore(coreIDs[i]);

                return CreateDeck(cards);
            }

            public BattleCardData[] CreateDeck(CardCore[] cardsInfo)
            {
                if (cardsInfo != null && cardsInfo.Length != 0)
                {
                    BattleCardData[] cards = new BattleCardData[cardsInfo.Length];

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
                _battleCardIdList.Clear();
                _battleCardIdList.Clear();
            }

            public List<CardInstance> CreateCardInstances(List<CoreID> coreIds)
            {
                List<CardInstance> output = new List<CardInstance>(coreIds.Count);
                
                foreach (var coreId in coreIds)
                {
                    output.Add(CreateCardInstance(coreId));
                }

                return output;
            }

            public CardInstance CreateCardInstance(CardSO cardSO, int level = 0)
                => CreateCardInstance(cardSO.ID, level);

            public CardInstance CreateCardInstance(int cardSOID, int level = 0)
             => CreateCardInstance(cardSOID + level);
            public CardInstance CreateCardInstance(int cardID)
            => CreateCardInstance(new CardCore(cardID));

            public CardInstance CreateCardInstance(CoreID cardID)
 => CreateCardInstance(new CardCore(cardID));
            public CardInstance CreateCardInstance(CardCore core)
            => new CardInstance(core);
            public BattleCardData CreateCard(CardInstance _data)
            {
                if (_data == null)
                    throw new Exception($"CardFactory: InstanceID is null!");
                return new BattleCardData(_data);
            }
            public BattleCardData CreateCard(int CardSOID, int level = 0)
             => CreateCard(GetCard(CardSOID), level);

            public BattleCardData CreateCard(CardSO cardSO, int level = 0)
            {

                if (cardSO != null && (level >= 0 && level <= cardSO.CardsMaxLevel))
                {
                    return CreateCard(CreateCardInstance(cardSO, level));
                }
                throw new Exception($" battleCard was not created!\nCardSO is :{cardSO} Level: {level} MaxLevel {cardSO.CardsMaxLevel}");

            }
            public BattleCardData[] CreateDeck(CardInstance[] cards)
            {
                BattleCardData[] c = new BattleCardData[cards.Length];
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = CreateCard(cards[i]);

                   // cards[i].InstanceID = c[i].CardInstance.InstanceID;
                }
                return c;
            }

            internal BattleCardData[] CreateDeck(AccountDeck deck)
            {
                if (deck == null)
                    throw new Exception("BattleCard Factory: AccountDeck is null!");

                int length = deck.Cards.Length;
                var cardsDataHolder = deck.Cards;
                BattleCardData[] cards = new BattleCardData[length];

                for (int i = 0; i < length; i++)
                {
                    var cardContainer = cardsDataHolder[i];

                    if (cardContainer == null)
                        throw new Exception($"CardFactory: battleCard was not created!\nCard at index: {i}  is null!");

                    cards[i] = CreateCard(cardContainer);

                    if (cards[i] == null)
                        throw new Exception($"BattleCard Factory: BattleCard Was Not Created From:\n CoreID: {cardContainer.CoreID}\nLevel: {cardContainer.Level}");

                }

                return cards;
            }
            public CardCore[] CreateCardCores(CoreID[] coreIDs)
            {
                CardCore[] cardCores = new CardCore[coreIDs.Length];
                for (int i = 0; i < coreIDs.Length; i++)
                    cardCores[i] = new CardCore(coreIDs[i].ID);
                return cardCores;
            }
            public static void Register(CardCore card)
            {
                if (_battleCardIdList?.Contains(card) ?? false)
                    _battleCardIdList.Add(card);
            }
            public static void Remove(CardCore card)
                => _battleCardIdList.Remove(card);
        }


        public class KeywordFactory
        {
            Keywords.KeywordsCollectionSO _keywordCollection;
            public KeywordFactory(Keywords.KeywordsCollectionSO keyword)
            {
                _keywordCollection = keyword;
            }
            public KeywordSO GetKeywordSO(KeywordType type)
                => _keywordCollection.GetKeywordSO(type);
        }
    }

}