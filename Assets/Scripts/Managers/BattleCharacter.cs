using Account.GeneralData;
using CardMaga.Battle.Players;
using CardMaga.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.Characters
{
    [Serializable]
    public class BattleCharacter
    {
        [SerializeField]
        private CharacterBattleData _characterData;

        [SerializeField] 
        private string _displayName;
        [SerializeField]
        private PlayerTagSO[] _playerTagSOs;
        public CharacterBattleData CharacterData { get => _characterData; private set => _characterData = value; }
        public string DisplayName { get => _displayName; }

        private int _model = 0;
        public int Model { get => _model; }
        public IReadOnlyList<PlayerTagSO> PlayerTags => _playerTagSOs;

        public BattleCharacter() { }

        public BattleCharacter(string displayName,  Account.GeneralData.Character data)
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

            BattleCardData battleCard = deckList.Find((x) => x.CardInstance.InstanceID == InstanceID);
           
            bool check = deckList.Remove(battleCard);
            if (check)
                _characterData.CharacterDeck = deckList.ToArray();
            return check;
        }
        
        public bool AddCardToDeck(CardCore card) => AddCardToDeck(card.CardSO(), card.Level);
        
        public bool AddCardToDeck(CardSO card, int level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add battleCard to deck the battleCard you tried to add is null!");

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);

            return AddCardToDeck(cardCreated);
        }
        
        public bool AddCardToDeck(BattleCardData battleCard)
        {
            if (battleCard == null)
                throw new Exception("Character Class : BattleCard is null");

            var deck = _characterData.CharacterDeck;
            int length = _characterData.CharacterDeck.Length;
            Array.Resize(ref deck, length + 1);
            deck[length] = battleCard;
            _characterData.CharacterDeck = deck;

            return battleCard != null;
        }
        
        public bool AddComboRecipe(Battle.Combo.BattleComboData battleComboData)
        {
            bool hasThisCombo = false;
            var comboRecipe = _characterData.ComboRecipe;
            for (int i = 0; i < comboRecipe.Length; i++)
            {
                hasThisCombo = comboRecipe[i].ID == battleComboData.ID;

                if (hasThisCombo)
                {
                    if (battleComboData.Level > comboRecipe[i].Level)
                        comboRecipe[i] = battleComboData;
                    else
                        return false;

                    break;
                }

            }

            if (hasThisCombo == false)
            {
                Array.Resize(ref comboRecipe, comboRecipe.Length + 1);
                comboRecipe[comboRecipe.Length - 1] = battleComboData;

                hasThisCombo = true;
            }
            _characterData.ComboRecipe = comboRecipe;

            return hasThisCombo;
        }
    }
}
