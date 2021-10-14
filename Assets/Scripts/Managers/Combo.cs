
using System;
namespace Combo
{
    public class Combo
    {
        public ComboSO ComboSO { get; private set; }
        public byte Level { get; private set; }

        public Combo(Battles.CharacterSO.RecipeInfo recipeInfo) : this(recipeInfo?.ComboRecipe, recipeInfo.Level) { }

        public Combo(ComboSO comboSO, byte level = 0)
        {
            if (comboSO == null)
                throw new Exception("Combo SO is null!");

            ComboSO = comboSO;
            Level = level;
        }

        public bool LevelUp()
        {
            Level++;

            if (Level > ComboSO.CraftedCard.CardsMaxLevel)
            {
                Level = ComboSO.CraftedCard.CardsMaxLevel;
                return false;
            }

            return true;
        }
    }
}