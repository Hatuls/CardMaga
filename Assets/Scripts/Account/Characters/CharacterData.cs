using Characters.Stats;
using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CharacterData
    {
    #region Field
        [SerializeField]
        CharacterEnum _character;
        [SerializeField]
        CharacterStats _stats;
        [SerializeField]
        AccountDeck[] _decks;
        [SerializeField]
        CombosAccountInfo[] _characterCombos;
        [SerializeField]
        ushort _unlockAtLevel;

    #endregion
    #region Properties
        public CharacterEnum Character => _character;
        public CharacterStats Stats => _stats;
        public AccountDeck[] Decks => _decks;
        public CombosAccountInfo[] CharacterCombos => _characterCombos;
        public ushort UnlockAtLevel => _unlockAtLevel;
        #endregion
        #region Public Methods
        public CharacterData(CharacterEnum characterEnum,byte deckAmount = 4)
        {
            if(characterEnum == CharacterEnum.Enemy)
            {
                throw new Exception("CharacterData inserted an enemy instead of a player character");
            }

            _character = characterEnum;

            var characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(characterEnum);
            _stats = characterSO.CharacterStats;
            _unlockAtLevel = characterSO.UnlockAtLevel;
            
            _decks = new AccountDeck[deckAmount];
            CardAccountInfo[] tempCard = new CardAccountInfo[characterSO.Deck.Length];
            for (int i = 0; i < characterSO.Deck.Length; i++)
            {
                tempCard[i] = new CardAccountInfo(characterSO.Deck[i].Card.ID, Factory.GameFactory.CardFactory.GetInstanceID, characterSO.Deck[i].Level);
            }


            AccountDeck tempDeck = new AccountDeck(tempCard);
            for (int i = 0; i < deckAmount; i++)
            {
                Decks[i] = tempDeck;
                Decks[i].DeckName = $"Basic Deck {i}";
            }

            CombosAccountInfo[] tempCombos = new CombosAccountInfo[characterSO.Combos.Length];
            for (int i = 0; i < characterSO.Combos.Length; i++)
            {
                tempCombos[i] = new CombosAccountInfo(characterSO.Combos[i].ComboRecipe.ID, characterSO.Combos[i].Level);
            }

            _characterCombos = tempCombos;

        }
        public AccountDeck GetDeckAt(int index)
        {
            throw new NotImplementedException();
        }
        public void CharacterAccount(CharacterEnum character, CharacterStats stats, AccountDeck[] decks,
            CombosAccountInfo[] combos, ushort unlocksAtLevel)
        {

        }
        #endregion
    }
}
