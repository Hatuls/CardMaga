using Characters.Stats;
using System;
namespace Account.GeneralData
{
    public class CharacterData
    {
    #region Field
        CharacterEnum _character;
        CharacterStats _stats;
        AccountDeck[] _decks;
        CombosAccountInfo[] _characterCombos;
        ushort _unlockAtLevel;

    #endregion
    #region Properties
        public CharacterEnum Character => _character;
        public CharacterStats Stats => _stats;
        public AccountDeck[] Decks => _decks;
        public CombosAccountInfo[] CharacterCombos => _characterCombos;
        public ushort UnlockAtLevel => _unlockAtLevel;
        #endregion
        #region PublicMethods
        public CharacterData(CharacterEnum characterEnum,byte deckAmount = 4)
        {
            var characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(characterEnum);
            _stats = characterSO.CharacterStats;
            _decks = new AccountDeck[deckAmount];
            CardAccountInfo[] tempCard = new CardAccountInfo[characterSO.Deck.Length];
            for (int i = 0; i < characterSO.Deck.Length; i++)
            {
                tempCard[i] = new CardAccountInfo(characterSO.Deck[i].Card.ID, 0, characterSO.Deck[i].Level);
            }
            AccountDeck tempDeck = new AccountDeck(tempCard);
            for (int i = 0; i < deckAmount; i++)
            {
                Decks[i] = tempDeck;
            }
            //create a new cardAccountInfo[]
            //fill it with cards acording to the deck of the player
            //for every cardAccountInfo[i] add the ID of the card, card InstanceID and the Level of the card
            //use the cardAccountInfo to fill all the deck slots
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
