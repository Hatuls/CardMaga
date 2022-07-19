using Account.GeneralData;
using Battle;
using CardMaga.Card;
using System;
using System.Linq;
using UnityEngine;


namespace Battle.Characters
{
    [Serializable]
    public class Character
    {
        [SerializeField]
        private CharacterBattleData _characterData;

        [SerializeField] 
        private string _displayName;

        public CharacterBattleData CharacterData { get => _characterData; private set => _characterData = value; }
        public string DisplayName { get => _displayName; }

        public Character() { }

        public Character(string displayName,  Account.GeneralData.Character data)
        {
            if (data == null)
                throw new Exception("Characters: Data Is Null");
            _displayName = displayName;
            _characterData = new CharacterBattleData(data);
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

            CardData card = deckList.Find((x) => x.CardInstanceID == InstanceID);
           
            bool check = deckList.Remove(card);
            if (check)
                _characterData.CharacterDeck = deckList.ToArray();
            return check;
        }
        public bool AddCardToDeck(CardCore card) => AddCardToDeck(card.CardSO(), card.Level);
        public bool AddCardToDeck(CardSO card, int level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add card to deck the card you tried to add is null!");

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);

            return AddCardToDeck(cardCreated);
        }
        public bool AddCardToDeck(CardData card)
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


        public bool AddComboRecipe(Battle.Combo.Combo combo)
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
