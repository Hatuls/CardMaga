using Battles;
using System;

using UnityEngine;


namespace Characters
{

    [Serializable]
    public class CharacterData
    {
        [SerializeField]
        private Stats.CharacterStats _characterStats;
        public ref Stats.CharacterStats CharacterStats { get => ref _characterStats;  }

        [SerializeField]
        private Cards.Card[] _characterDeck;
        public Cards.Card[] CharacterDeck { get => _characterDeck; internal set => _characterDeck = value; }

        [SerializeField]
        private Combo.Combo[] _comboRecipe;
        public Combo.Combo[] ComboRecipe { get => _comboRecipe; internal set => _comboRecipe = value; }

        [Sirenix.OdinInspector.ShowInInspector]
        public CharacterSO Info { get; private set; }

        public CharacterData(CharacterSO characterSO)
        {
            Info = characterSO;
            var factory = Factory.GameFactory.Instance;
            _characterDeck = factory.CardFactoryHandler.CreateDeck(Info.Deck);
            _comboRecipe = factory.ComboFactoryHandler.CreateCombo(Info.Combos);

            _characterStats = Info.CharacterStats;
        }

    }
    [Serializable]
    public class Character
    {
        [SerializeField]
        CharacterData _characterData;
        public CharacterData CharacterData { get => _characterData; private set => _characterData = value; }
       public Character(CharacterSO characterSO)  
        {
            if (characterSO == null)
                throw new Exception($"Character: CharactersO is null!");

            CharacterData = new CharacterData(characterSO);
        }
        public Character(CharacterData data)
        {
            if (data == null)
                throw new Exception($"Character: CharacterData is null!");

            _characterData = data;
        }

        public bool AddCardToDeck(CharacterSO.CardInfo card) => AddCardToDeck(card.Card, card.Level);
        public bool AddCardToDeck(Cards.CardSO card, byte level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add card to deck the card you tried to add is null!");

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);
      
            return    AddCardToDeck(cardCreated); 
        }
        public bool AddCardToDeck(Cards.Card card)
        {
            if (card == null)
                throw new Exception("Character Class : Card is null");

            var deck = _characterData.CharacterDeck;
            int length = _characterData.CharacterDeck.Length;
            Array.Resize(ref deck, length + 1);
            deck[length ] = card;
            _characterData.CharacterDeck = deck;

            return card != null;
        }

        public bool AddComboRecipe(CharacterSO.RecipeInfo recipeInfo)
        {
            bool hasThisCombo = false;
            var comboRecipe = _characterData.ComboRecipe;
            for (int i = 0; i < comboRecipe.Length; i++)
            {
                hasThisCombo = comboRecipe[i].ComboSO.ID == recipeInfo.ComboRecipe.ID;

                if (hasThisCombo)
                {
                    if (recipeInfo.Level > comboRecipe[i].Level)
                        comboRecipe[i] = Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(recipeInfo);

                    break;
                }

            }

            if (hasThisCombo == false)
            {
                Array.Resize(ref comboRecipe, comboRecipe.Length + 1);
                comboRecipe[comboRecipe.Length - 1] = Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(recipeInfo);

                hasThisCombo = true;
            }
            _characterData.ComboRecipe = comboRecipe;

            return hasThisCombo;
        }



    }
}
