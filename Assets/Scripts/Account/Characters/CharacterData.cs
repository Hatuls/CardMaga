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
