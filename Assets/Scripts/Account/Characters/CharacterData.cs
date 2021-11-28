using Characters.Stats;
using System;
using UI.Meta.PlayScreen;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CharacterData
    {

        #region Field
        [SerializeField]
        CharacterEnum _characterEnum;
        [SerializeField]
        CharacterStats _stats;
        [SerializeField]
        AccountDeck[] _decks;
        [SerializeField]
        CombosAccountInfo[] _characterCombos;
        [SerializeField]
        byte _unlockAtLevel;
      
        #endregion

        #region Properties
        public CharacterEnum CharacterEnum => _characterEnum;
        public CharacterStats Stats => _stats;
        public AccountDeck[] Decks => _decks;
        public CombosAccountInfo[] CharacterCombos => _characterCombos;
        public byte UnlockAtLevel => _unlockAtLevel;
        #endregion

        #region PrivateMethods

        void AssignDeck(CardCoreInfo[] cardAccountInfos, byte deckAmount)
        {

            _decks = new AccountDeck[deckAmount];
            CardCoreInfo[] tempCards = new CardCoreInfo[cardAccountInfos.Length];
            for (int i = 0; i < cardAccountInfos.Length; i++)
            {
                tempCards[i] = cardAccountInfos[i];
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
        public CharacterData(CardCoreInfo[] startingDeck ,CharacterEnum characterEnum, byte deckAmount = 4)
        {
            if (characterEnum == CharacterEnum.Enemy)
            {
                throw new Exception("CharacterData inserted an enemy instead of a player character");
            }
            var characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(characterEnum);
            _characterEnum = characterEnum;
            _stats = characterSO.CharacterStats;
            _unlockAtLevel = characterSO.UnlockAtLevel;
            AssignDeck(startingDeck, deckAmount);
            AssignCombos(characterSO);

            AccountCards.OnUpgrade += CardUpgraded;
        }
     
        public CharacterData()
        {
            AccountCards.OnUpgrade += CardUpgraded;

        }
        ~CharacterData()
        {
            AccountCards.OnUpgrade -= CardUpgraded;
        }
        public AccountDeck GetDeckAt(int index)
      => _decks[index];
        public void CharacterAccount(CharacterEnum character, CharacterStats stats, AccountDeck[] decks,
            CombosAccountInfo[] combos, byte unlocksAtLevel)
        {

        }

        public void CardUpgraded(CardCoreInfo cardCoreInfo)
        {
            for (int i = 0; i < _decks.Length; i++)
            {
                var cards = _decks[i].Cards;
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].InstanceID == cardCoreInfo.InstanceID)
                    {
                        cards[j] = cardCoreInfo;
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
