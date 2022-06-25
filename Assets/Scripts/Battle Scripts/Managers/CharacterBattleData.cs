using Battles;
using System;

using UnityEngine;
using Account.GeneralData;

namespace Characters
{
    [Serializable]
    public class CharacterBattleData
    {
        [SerializeField]
        private Stats.CharacterStats _characterStats;
        public ref Stats.CharacterStats CharacterStats { get => ref _characterStats; }

        [SerializeField]
        private Cards.Card[] _characterDeck;
        public Cards.Card[] CharacterDeck { get => _characterDeck; internal set => _characterDeck = value; }

        [SerializeField]
        private Combo.Combo[] _comboRecipe;
        [SerializeField]
        private CharacterSO _characterSO;

        public Combo.Combo[] ComboRecipe { get => _comboRecipe; internal set => _comboRecipe = value; }

        public CharacterSO CharacterSO { get => _characterSO; private set => _characterSO = value; }
        // Need To be Re-Done
        public CharacterBattleData(CharacterSO characterSO)
        {
            //CharacterSO = characterSO;
            //var factory = Factory.GameFactory.Instance;
            //_characterDeck = factory.CardFactoryHandler.CreateDeck(CharacterSO.Deck);
            //_comboRecipe = factory.ComboFactoryHandler.CreateCombos(CharacterSO.Combos);

            //_characterStats = CharacterSO.CharacterStats;
        }
        // Need To be Re-Done
        public CharacterBattleData(CharacterData data, AccountDeck _deck)
        {
            //if (data == null)
            //    throw new Exception($"CharacterBattleData : Did not constructed because CharacterData is null!");
            //var factory = Factory.GameFactory.Instance;
            //_characterStats = data.Stats;
            //CharacterSO = factory.CharacterFactoryHandler.GetCharacterSO(data.CharacterEnum);
            //_characterDeck = factory.CardFactoryHandler.CreateDeck(_deck);
            //_comboRecipe = factory.ComboFactoryHandler.CreateCombo(data.CharacterCombos);
        }
        public CharacterBattleData()
        {

        }


    }
}
