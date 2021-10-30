using Characters.Stats;
using System;
using UI.Meta.PlayScreen;
using UnityEngine;
namespace Account.GeneralData
{
    
    public class CharacterData
    {

    #region Field

        CharacterEnum _characterEnum;

        CharacterStats _stats;

        AccountDeck[] _decks;

        CombosAccountInfo[] _characterCombos;

        byte _unlockAtLevel;
        internal DeckUI[] _avilableDecks;
        #endregion

        #region Properties
        public CharacterEnum CharacterEnum => _characterEnum;
        public CharacterStats Stats => _stats;
        public AccountDeck[] Decks => _decks;
        public CombosAccountInfo[] CharacterCombos => _characterCombos;
        public byte UnlockAtLevel => _unlockAtLevel;
        #endregion

        #region PrivateMethods
        void AssignDeck(Battles.CharacterSO characterSO,byte deckAmount)
        {

            _decks = new AccountDeck[deckAmount];
            CardAccountInfo[] tempCards = new CardAccountInfo[characterSO.Deck.Length];
            for (int i = 0; i < characterSO.Deck.Length; i++)
            {
                tempCards[i] = new CardAccountInfo(characterSO.Deck[i].Card.ID, Factory.GameFactory.CardFactory.GetInstanceID, characterSO.Deck[i].Level);
            }


            for (int i = 0; i < deckAmount; i++)
            {
                Decks[i] = new AccountDeck(tempCards);
                Decks[i].DeckName = $"Basic Deck {i}";
            }
        }
        void AssignCombos(Battles.CharacterSO characterSO)
        {
            CombosAccountInfo[] tempCombos = new CombosAccountInfo[characterSO.Combos.Length];
            for (int i = 0; i < characterSO.Combos.Length; i++)
            {
                tempCombos[i] = new CombosAccountInfo(characterSO.Combos[i].ComboRecipe.ID, characterSO.Combos[i].Level);
            }

            _characterCombos = tempCombos;
        }
        #endregion

        #region PublicMethods
        public CharacterData(CharacterEnum characterEnum,byte deckAmount = 4)
        {
            if(characterEnum == CharacterEnum.Enemy)
            {
                throw new Exception("CharacterData inserted an enemy instead of a player character");
            }
            var characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(characterEnum);
            _characterEnum = characterEnum;
            _stats = characterSO.CharacterStats;
            _unlockAtLevel = characterSO.UnlockAtLevel;
            AssignDeck(characterSO, deckAmount);
            AssignCombos(characterSO);
        }
        public AccountDeck GetDeckAt(int index)
        {
            throw new NotImplementedException();
        }
        public void CharacterAccount(CharacterEnum character, CharacterStats stats, AccountDeck[] decks,
            CombosAccountInfo[] combos, byte unlocksAtLevel)
        {

        }
        #endregion
    }
}
