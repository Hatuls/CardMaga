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
        public ref Stats.CharacterStats CharacterStats { get => ref _characterStats;  }

        [SerializeField]
        private Cards.Card[] _characterDeck;
        public Cards.Card[] CharacterDeck { get => _characterDeck; internal set => _characterDeck = value; }

        [SerializeField]
        private Combo.Combo[] _comboRecipe;
        public Combo.Combo[] ComboRecipe { get => _comboRecipe; internal set => _comboRecipe = value; }

        [Sirenix.OdinInspector.ShowInInspector]
        public CharacterSO Info { get; private set; }

        public CharacterBattleData(CharacterSO characterSO)
        {
            Info = characterSO;
            var factory = Factory.GameFactory.Instance;
            _characterDeck = factory.CardFactoryHandler.CreateDeck(Info.Deck);
            _comboRecipe = factory.ComboFactoryHandler.CreateCombo(Info.Combos);

            _characterStats = Info.CharacterStats;
        }

        public CharacterBattleData(CharacterData data, AccountDeck _deck)
        {
            if (data == null)
                throw new Exception($"CharacterBattleData : Did not constructed because CharacterData is null!");
            var factory = Factory.GameFactory.Instance;
            _characterStats = data.Stats;
            Info = factory.CharacterFactoryHandler.GetCharacterSO(data.CharacterEnum);
            _characterDeck = factory.CardFactoryHandler.CreateDeck(_deck);
            _comboRecipe = factory.ComboFactoryHandler.CreateCombo(data.CharacterCombos);
        }



    }
}
