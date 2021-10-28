using Battles;
using Cards;
using Collections;
using Combo;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rewards;
using Characters;
using Account.GeneralData;
using Map;

namespace Factory
{
    public class GameFactory
    {
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
                    EventPointCollectionSO eventPointCollection  = Resources.Load<EventPointCollectionSO>("Collection SO/EventPointCollection");


                    _instance = new GameFactory(cardCollections, recipeCollection, characterCollection, battleRewardsCollection, eventPointCollection);
                }
                return _instance;
            }
        }
        public ComboFactory ComboFactoryHandler { get; private set; }
        public CardFactory CardFactoryHandler { get; private set; }
        public CharacterFactory CharacterFactoryHandler { get; private set; }
        public RewardFactory RewardFactoryHandler { get; private set; }
        public EventPointFactory EventPointFactoryHandler { get; private set; }
        public GameFactory(CardsCollectionSO cards, ComboCollectionSO comboCollectionSO, CharacterCollectionSO characters, BattleRewardCollectionSO rewards , EventPointCollectionSO eventPoints)
        {
            if (cards == null || comboCollectionSO == null || characters == null || rewards == null|| eventPoints == null)
                throw new Exception("Collections is null!!");


            CardFactoryHandler = new CardFactory(cards);
            ComboFactoryHandler = new ComboFactory(comboCollectionSO);
            CharacterFactoryHandler = new CharacterFactory(characters);
            RewardFactoryHandler = new RewardFactory(rewards);
            EventPointFactoryHandler = new EventPointFactory(eventPoints);
            Debug.Log("Factory Created<a>!</a>");

            _instance = this;
        }



        public class EventPointFactory
        {
            public EventPointCollectionSO EventPointCollection { get; private set; }
            public EventPointFactory(EventPointCollectionSO eventPoints)
            {
                EventPointCollection = eventPoints;
            }

            public EventPointAbstSO GetEventPoint(EventPointType type)
           => EventPointCollection.GetEventPoint(type);
        }
        public class RewardFactory
        {
            public BattleRewardCollectionSO BattleRewardCollection { get; private set; }
            public RewardFactory(BattleRewardCollectionSO battleRewardCollectionSO)
            {
                BattleRewardCollection = battleRewardCollectionSO;
            }

            public BattleReward GetBattleRewards(CharacterTypeEnum characterTypeEnum)
       => BattleRewardCollection.GetReward(characterTypeEnum);


        }
        public class CharacterFactory 
        {
            public CharacterCollectionSO CharacterCollection { get; private set; }
            public CharacterFactory(CharacterCollectionSO characterCollection)
            {
                CharacterCollection = characterCollection;
            }

            public CharacterSO GetCharacterSO(CharacterTypeEnum character)
            => CharacterCollection.GetCharacterSO(character);
            internal CharacterSO GetCharacterSO(CharacterEnum characterEnum)
             => CharacterCollection.GetCharacterSO(characterEnum);

            public Character CreateCharacter(CharacterData data, AccountDeck _deck)
                => new Character( data,  _deck);
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

            public ComboFactory( ComboCollectionSO comboCollection)
            {
                ComboCollection = comboCollection;
                comboCollection.AssignDictionary();
            }
            public Combo.Combo[] CreateCombo(CharacterSO.RecipeInfo[] recipeInfos)
            {
                if (recipeInfos != null)
                {
                    Combo.Combo[] combos = new Combo.Combo[recipeInfos.Length];
                    for (int i = 0; i < recipeInfos.Length; i++)
                        combos[i] = CreateCombo(recipeInfos[i]);

                    return combos;
                }
                return null;
            }

             public Combo.Combo CreateCombo(CharacterSO.RecipeInfo recipe)
             =>new Combo.Combo(recipe);

            public Combo.Combo CreateCombo(ComboSO comboSO, byte level = 0)
               => new Combo.Combo(comboSO, level);

            internal Combo.Combo[] CreateCombo(CombosAccountInfo[] characterCombos)
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

            private Combo.Combo CreateCombo(CombosAccountInfo combosAccountInfo)
            {
                if (combosAccountInfo == null)
                    throw new Exception("Combo Factory: CombosAccountInfo is null!");

                return new Combo.Combo(ComboCollection.GetCombo(combosAccountInfo.ID), combosAccountInfo.Level);
            }
        }
        public class CardFactory
        {
            static List<ushort> _battleCardIdList;
            static ushort _battleID;
            public CardsCollectionSO CardCollection { get; private set; }
            public CardFactory(CardsCollectionSO cards)
            {
                CardCollection = cards;
                cards.AssignDictionary();
                Reset();
            }
            public static ushort GetInstanceID
            {
                get
                {
                    if (_battleCardIdList == null)
                        _battleCardIdList = new List<ushort>();

                    while (_battleCardIdList.Contains(_battleID))
                        _battleID++;

                    _battleCardIdList.Add(_battleID);

                    return _battleID;
                }
            }

            public Card[] CreateDeck(CharacterSO.CardInfo[] cardsInfo)
            {
                if (cardsInfo != null && cardsInfo.Length != 0)
                {
                    Card[] cards = new Card[cardsInfo.Length];

                    for (int i = 0; i < cards.Length; i++)
                    {
                        if (cardsInfo[i] != null)
                            cards[i] = CreateCard(cardsInfo[i].Card, cardsInfo[i].Level);
                    }
                    return cards;
                }
                return null;
            }

            public void Reset()
            {
                if (_battleCardIdList == null)
                    _battleCardIdList = new List<ushort>();
                else
                    _battleCardIdList.Clear();


                _battleID = 1;
            }
            public Card CreateCard(ushort CardSOID, byte level = 0)
             => CreateCard(CardCollection.GetCard(CardSOID), level);
            public Card CreateCard(CardSO cardSO, byte level = 0)
            {

                if (cardSO != null && (level >= 0 && level <= cardSO.CardsMaxLevel))
                {
                    while (_battleCardIdList.Contains(_battleID))
                        _battleID++;

                    _battleCardIdList.Add(_battleID);

                    return new Card(_battleID, cardSO, level);
                }
                throw new Exception($" card was not created!\nCardSO is :{cardSO}");

            }
            public Card CreateCard(CardAccountInfo card)
            {
                if (card == null)
                    throw new Exception("Card Factory: Card Account Info Is null!");
                card.InstanceID = _battleID;
                return CreateCard(card.CardID, card.Level);
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

                    if (cardContainer == null )
                    throw new Exception($"CardFactory: card was not created!\nCard at index: {i}  is null!");

                    cards[i] = CreateCard(cardContainer);

                    if (cards[i] == null)
                        throw new Exception($"Card Factory: Card Was Not Created From:\n ID: {cardContainer.CardID}\nLevel: {cardContainer.Level}");
                 
                }

                return cards;
            }
        }
    }
     
}