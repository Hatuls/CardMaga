using Battle;
using System;

using UnityEngine;
using Account.GeneralData;
using Characters.Stats;
using CardMaga.Card;

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
        private CardData[] _characterDeck;
        public CardData[] CharacterDeck { get => _characterDeck; internal set => _characterDeck = value; }

        [SerializeField]
        private Battle.Combo.ComboData[] _comboRecipe;
   
      //  private Account.GeneralData.Character _data;

        public Battle.Combo.ComboData[] ComboRecipe { get => _comboRecipe; internal set => _comboRecipe = value; }

        public CharacterSO CharacterSO { get => _characterSO; internal set => _characterSO = value; }
 
        public CharacterBattleData() { }
        
        public CharacterBattleData(Account.GeneralData.Character data)
        {
            var factory = Factory.GameFactory.Instance;
            var selectedDeck = data.Deck[data.MainDeck];

            CharacterSO = factory.CharacterFactoryHandler.GetCharacterSO(data.Id);
            CharacterDeck = factory.CardFactoryHandler.CreateDeck(selectedDeck.Cards);
            ComboRecipe = factory.ComboFactoryHandler.CreateCombos(selectedDeck.Combos);
            _characterStats = CharacterSO.CharacterStats;
        }

        #region Editor
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        /// <summary>
        /// This is for editor purpose only!
        /// </summary>
        private void TryAssignCharacter()
        {
            try
            {
                CharacterDeck = CreateDeck(CharacterSO);
                ComboRecipe = CreateCombos(CharacterSO);
                _characterStats = CharacterSO.CharacterStats;
            }
            catch (Exception e)
            {
                throw e;
            }

            CardData[] CreateDeck(CharacterSO characterSO)
            
            {
                var deck = characterSO.Deck;
                CardData[] cards = new CardData[deck.Length];
                for (int i = 0; i < deck.Length; i++)
                {
                    cards[i] = new CardData(new CardInstanceID(new CardCore(deck[i].ID)));
                }
                return cards;
            }
             Combo.ComboData[] CreateCombos(CharacterSO characterSO)
            {
                var characterCombos = characterSO.Combos;
                Combo.ComboData[] combos = new Combo.ComboData[characterCombos.Length];
                for (int i = 0; i < characterCombos.Length; i++)
                {
                    combos[i] = new Combo.ComboData(characterCombos[i].ComboSO(), 0);
                }
                return combos;
            }
        }
#endif
        #endregion
    }
}
