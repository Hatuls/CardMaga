using Account.GeneralData;
using Battles;
using System;
using System.Linq;
using UnityEngine;


namespace Characters
{
    [Serializable]
    public class Character
    {
        [SerializeField]
        CharacterBattleData _characterData;
        public CharacterBattleData CharacterData { get => _characterData; private set => _characterData = value; }
        public Character(CharacterData data, AccountDeck _deck)
        {
            if (data == null)
                throw new Exception($"Character: CharacterData is null!");
            else if (_deck == null || _deck.Cards.Length == 0)
                throw new Exception("AccountDeck Is null or empty!");

            CharacterData = new CharacterBattleData(data, _deck);
        }
        public Character(CharacterSO characterSO)
        {
            if (characterSO == null)
                throw new Exception($"Character: CharactersO is null!");

            CharacterData = new CharacterBattleData(characterSO);
        }
        public Character(CharacterBattleData data)
        {
            if (data == null)
                throw new Exception($"Character: CharacterData is null!");

            _characterData = data;
        }

        public bool RemoveCardFromDeck(ushort InstanceID)
        {
            var deckList = _characterData.CharacterDeck.ToList();
        
            Cards.Card card = deckList.Find((x) => x.CardID == InstanceID);
           
            bool check = deckList.Remove(card);
            if (check)
                _characterData.CharacterDeck = deckList.ToArray();
            return check;
        }
        public bool AddCardToDeck(CharacterSO.CardInfo card) => AddCardToDeck(card.Card, card.Level);
        public bool AddCardToDeck(Cards.CardSO card, byte level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add card to deck the card you tried to add is null!");

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);

            return AddCardToDeck(cardCreated);
        }
        public bool AddCardToDeck(Cards.Card card)
        {
            if (card == null)
                throw new Exception("Character Class : Card is null");

            var deck = _characterData.CharacterDeck;
            int length = _characterData.CharacterDeck.Length;
            Array.Resize(ref deck, length + 1);
            deck[length] = card;
            _characterData.CharacterDeck = deck;

            return card != null;
        }


        public bool AddComboRecipe(Combo.Combo combo)
        {
            bool hasThisCombo = false;
            var comboRecipe = _characterData.ComboRecipe;
            for (int i = 0; i < comboRecipe.Length; i++)
            {
                hasThisCombo = comboRecipe[i].ComboSO.ID == combo.ComboSO.ID;

                if (hasThisCombo)
                {
                    if (combo.Level > comboRecipe[i].Level)
                        comboRecipe[i] = combo;
                    else
                        return false;

                    break;
                }

            }

            if (hasThisCombo == false)
            {
                Array.Resize(ref comboRecipe, comboRecipe.Length + 1);
                comboRecipe[comboRecipe.Length - 1] = combo;

                hasThisCombo = true;
            }
            _characterData.ComboRecipe = comboRecipe;

            return hasThisCombo;
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
