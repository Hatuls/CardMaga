using Battle;
using System;

using UnityEngine;
using Account.GeneralData;
using Characters.Stats;
using Cards;

namespace Battle.Characters
{
    [Serializable]
    public class CharacterBattleData
    {
        [SerializeField]
        private CharacterSO _characterSO;
        [SerializeField]
        private CharacterStats _characterStats;
        public ref CharacterStats CharacterStats { get => ref _characterStats; }

        [SerializeField]
        private Cards.Card[] _characterDeck;
        public Cards.Card[] CharacterDeck { get => _characterDeck; internal set => _characterDeck = value; }

        [SerializeField]
        private Battle.Combo.Combo[] _comboRecipe;
   
      //  private Account.GeneralData.Character _data;

        public Battle.Combo.Combo[] ComboRecipe { get => _comboRecipe; internal set => _comboRecipe = value; }

        public CharacterSO CharacterSO { get => _characterSO; private set => _characterSO = value; }
 
        public CharacterBattleData() { }
        public CharacterBattleData(Account.GeneralData.Character data)
        {
            var factory =       Factory.GameFactory.Instance;
            var selectedDeck =  data.Deck[data.MainDeck];

            CharacterSO =       factory.CharacterFactoryHandler.GetCharacterSO(data.Id);
            _characterDeck =    factory.CardFactoryHandler.CreateDeck(selectedDeck.Cards);
            _comboRecipe =      factory.ComboFactoryHandler.CreateCombos(selectedDeck.Combos);
            _characterStats =   CharacterSO.CharacterStats;
        }


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        /// <summary>
        /// This is for editor purpose only!
        /// </summary>
        private void TryAssignCharacter()
        {
            try
            {
                _characterDeck = CreateDeck(CharacterSO);
                _comboRecipe = CreateCombos(CharacterSO);
                _characterStats = CharacterSO.CharacterStats;
            }
            catch (Exception e)
            {
                throw e;
            }


             Card[] CreateDeck(CharacterSO characterSO)
            {
                var deck = characterSO.Deck;
                Card[] cards = new Card[deck.Length];
                for (int i = 0; i < deck.Length; i++)
                {
                    cards[i] = new Card(deck[i].CreateInstance());
                }
                return cards;
            }
             Combo.Combo[] CreateCombos(CharacterSO characterSO)
            {
                var characterCombos = characterSO.Combos;
                Combo.Combo[] combos = new Combo.Combo[characterCombos.Length];
                for (int i = 0; i < characterCombos.Length; i++)
                {
                    combos[i] = new Combo.Combo(characterCombos[i].ComboSO(), 0);
                }
                return combos;
            }
        }
#endif

    }
}
