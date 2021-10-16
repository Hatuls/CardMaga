using Battles;
using Cards;
using Collections.RelicsSO;
using Combo;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rewards;
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
                    CharacterCollection characterCollection = Resources.Load<CharacterCollection>("Collection SO/CharacterCollection");
                    BattleRewardCollectionSO battleRewardsCollection = Resources.Load<BattleRewardCollectionSO>("Collection SO/BattleRewardsCollection");

                    _instance = new GameFactory(cardCollections, recipeCollection, characterCollection, battleRewardsCollection);
                }
                return _instance;
            }
        }
        public ComboFactory ComboFactoryHandler { get; private set; }
        public CardFactory CardFactoryHandler { get; private set; }
        public CharacterFactory CharacterFactoryHandler { get; private set; }
        public RewardFactory RewardFactoryHandler { get; private set; }
        public GameFactory(CardsCollectionSO cards, ComboCollectionSO comboCollectionSO, CharacterCollection characters, BattleRewardCollectionSO rewards )
        {
            if (cards == null || comboCollectionSO == null || characters == null || rewards == null)
                throw new System.Exception("Collections is null!!");


            CardFactoryHandler = new CardFactory(cards);
            ComboFactoryHandler = new ComboFactory(comboCollectionSO);
            CharacterFactoryHandler = new CharacterFactory(characters);
            RewardFactoryHandler = new RewardFactory(rewards);
            Debug.Log("Factory Created<a>!</a>");

            _instance = this;
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
            public CharacterCollection Characters { get; private set; }
            public CharacterFactory(CharacterCollection characterCollection)
            {
                Characters = characterCollection;
            }

        
            internal Character CreateCharacter(CharacterTypeEnum character)
            {
                var characterSO = Characters.CharactersSO;
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

        }
        public class CardFactory 
        { 
            static List<int> _battleCardIdList;
            static ushort _battleID;
            public CardsCollectionSO CardCollection { get; set; }
            public CardFactory(CardsCollectionSO cards)
            {
                CardCollection = cards;
                Reset();
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
                    _battleCardIdList = new List<int>();
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
        }
    }
     
}