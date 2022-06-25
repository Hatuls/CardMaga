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
        public Character(Account.GeneralData.Character data, AccountDeck _deck)
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
        public bool RemoveCombo(int comboID)
        {
            var combo = _characterData.ComboRecipe.ToList();
            for (int i = 0; i < combo.Count; i++)
            {
                if (combo[i].ID == comboID)
                {
                    combo.RemoveAt(i);
                    _characterData.ComboRecipe = combo.ToArray();
                    return true;
                }
            }
       
            return false;
        }
        public bool RemoveCardFromDeck(int InstanceID)
        {
            var deckList = _characterData.CharacterDeck.ToList();
        
            Cards.Card card = deckList.Find((x) => x.CardInstanceID == InstanceID);
           
            bool check = deckList.Remove(card);
            if (check)
                _characterData.CharacterDeck = deckList.ToArray();
            return check;
        }
        public bool AddCardToDeck(CardInstanceID.CardCore card) => AddCardToDeck(card.CardSO(), card.Level);
        public bool AddCardToDeck(Cards.CardSO card, int level = 0)
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
                hasThisCombo = comboRecipe[i].ID == combo.ID;

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

    }
}
