using UnityEngine;
using System;
namespace Combo
{
    [Serializable]
    public class Combo
    {

        [SerializeField]
        private ComboSO _comboSO;
        public ComboSO ComboSO { get=> _comboSO; private set { _comboSO = value; } }
        [SerializeField]
        private byte _level;
        public byte Level { get=> _level; private set=> _level =value; }
        public Combo(Battles.CharacterSO.RecipeInfo recipeInfo) : this(recipeInfo?.ComboRecipe, recipeInfo.Level) { }

        public Combo(ComboSO comboSO, byte level = 0)
        {
            ComboSO = (comboSO != null) ? comboSO : throw new Exception("Combo SO is null!");
            Level = level;
        }

        public bool LevelUp()
        {
            Level++;

            if (Level >= ComboSO.CraftedCard.CardsMaxLevel)
            {
                Level = ComboSO.CraftedCard.CardsMaxLevel;
                return false;
            }

            return true;
        }
    }
}