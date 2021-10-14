using System;
namespace Battles
{
    [Serializable]
    public class Character
    {
        private Characters.Stats.CharacterStats _characterStats;
        public ref Characters.Stats.CharacterStats CharacterStats { get => ref _characterStats; }

        private Cards.Card[] _characterDeck;
        public Cards.Card[] CharacterDeck { get => _characterDeck; }

        private Combo.Combo[] _comboRecipe;
        public Combo.Combo[] ComboRecipe { get => _comboRecipe; }

        public CharacterSO Info { get;private set; }
        public Character(CharacterSO characterSO)
        {
            Info = characterSO;
            var factory = Factory.GameFactory.Instance;
            _characterDeck = factory.CardFactoryHandler.CreateDeck(Info.Deck);
            _comboRecipe = factory.ComboFactoryHandler.CreateCombo(Info.Combos);

            _characterStats = Info.CharacterStats;
        }



        public bool AddCardToDeck(CharacterSO.CardInfo card) => AddCardToDeck(card.Card, card.Level);
        public bool AddCardToDeck(Cards.CardSO card, byte level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add card to deck the card you tried to add is null!");

            Array.Resize(ref _characterDeck, CharacterDeck.Length + 1);

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);
            _characterDeck[CharacterDeck.Length - 1] = cardCreated;

            return cardCreated != null;
        }
        public bool AddComboRecipe(CharacterSO.RecipeInfo recipeInfo)
        {
            bool hasThisCombo = false;

            for (int i = 0; i < _comboRecipe.Length; i++)
            {
                hasThisCombo = _comboRecipe[i].ComboSO.ID == recipeInfo.ComboRecipe.ID;

                if (hasThisCombo)
                {
                    if (recipeInfo.Level > _comboRecipe[i].Level)
                        _comboRecipe[i] = Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(recipeInfo);
                    
                    break;
                }

            }

            if (hasThisCombo == false)
            {
                Array.Resize(ref _comboRecipe, _comboRecipe.Length + 1);
                _comboRecipe[_comboRecipe.Length - 1] = Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(recipeInfo);

                hasThisCombo = true;
            }

            return hasThisCombo;
        }



    }
}
