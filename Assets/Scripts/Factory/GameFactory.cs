using Battles;
using Cards;
using Collections.RelicsSO;
using Combo;
using System.Collections.Generic;
using UnityEngine;

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

                    _instance = new GameFactory(cardCollections, recipeCollection, characterCollection);
                }
                return _instance;
            }
        }
        public ComboFactory ComboFactoryHandler { get; private set; }
        public CardFactory CardFactoryHandler { get; private set; }
        public CharacterFactory CharacterFactoryHandler { get; private set; }
        public GameFactory(CardsCollectionSO cards, ComboCollectionSO comboCollectionSO, CharacterCollection characters)
        {
            if (cards == null || comboCollectionSO == null || characters == null)
                throw new System.Exception("Collections is null!!");


            CardFactoryHandler = new CardFactory(cards);
            ComboFactoryHandler = new ComboFactory(comboCollectionSO);
            CharacterFactoryHandler = new CharacterFactory(characters);


            _instance = this;
        }




        public class CharacterFactory 
        {
            public CharacterCollection Characters { get; private set; }
            public CharacterFactory(CharacterCollection characterCollection)
            {
                Characters = characterCollection;
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

            public Combo.Combo CreateCombo(ComboSO comboSO, int level = 0)
               => new Combo.Combo(comboSO, level);

        }
        public class CardFactory 
        { 
            static List<int> _battleCardIdList;
            static int _battleID;
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

            public Card CreateCard(CardSO cardSO, int level = 0)
            {

                if (cardSO != null && (level >= 0 && level <= cardSO.CardsMaxLevel))
                {
                    while (_battleCardIdList.Contains(_battleID))
                        _battleID++;

                    _battleCardIdList.Add(_battleID);

                    return new Card(_battleID, cardSO, level);
                }
                return null;

            }
        }
    }
     
}