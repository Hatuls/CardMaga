using UnityEngine;
using System;
namespace Combo
{
    [Serializable]
    public class Combo
    {
        [SerializeField]
        private int _id;
        // maybe need to add also instance id?
        [SerializeField]
        private int _level;
        public int Level { get=> _level; private set=> _level =value; }
        public int ID { get => _id; private set => _id = value; }


        public Combo(ComboSO comboSO, int level = 0) :this( comboSO?.ID ?? -1, level) { }
     
        public Combo (int id , int level =0)
        {
            if (id == -1)
                throw new Exception("Combo is not registered Error Code -1");
            ID = id;
            Level= level;
        }
        public bool LevelUp()
        {
            Level++;
            var ComboSO = this.ComboSO();
            if (Level >= ComboSO.CraftedCard.CardsMaxLevel)
            {
                Level = ComboSO.CraftedCard.CardsMaxLevel;
                return false;
            }
            return true;
        }


    }
    public static class ComboHelper
    {
        public static ComboSO ComboSO(this Combo c)
    => Factory.GameFactory.Instance.ComboFactoryHandler.GetComboSO(c.ID);
    }
}