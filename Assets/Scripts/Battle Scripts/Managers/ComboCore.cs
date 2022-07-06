using Battle.Combo;
using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class ComboCore
    {
        [SerializeField]
        private int _id;
        // maybe need to add also instance id?
        [SerializeField]
        private int _level;
        public int Level { get => _level; private set => _level = value; }
        public int ID { get => _id; private set => _id = value; }


        public ComboCore(ComboSO comboSO, int level = 0) : this(comboSO?.ID ?? -1, level) { }

        public ComboCore(int id, int level = 0)
        {
            if (id == -1)
                throw new Exception("Combo is not registered Error Code -1");
            ID = id;
            Level = level;
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
        public static ComboSO ComboSO(this ComboCore c)
    => Factory.GameFactory.Instance.ComboFactoryHandler.GetComboSO(c.ID);
    }
}

namespace Battle.Combo
{
    [Serializable]
    public class Combo
    {
        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private Account.GeneralData.ComboCore _comboCore;
        [SerializeField]
        private ComboSO _comboSO;


        public Combo(ComboSO comboSO, int level)
        {
            ComboSO = comboSO;
            _comboCore = new Account.GeneralData.ComboCore(comboSO, level);

        }

        public int ID => _comboCore.ID;
        public int Level => _comboCore.Level;
        public Cards.CardTypeData[] ComboSequence => _comboSO.ComboSequence;
        public ComboSO ComboSO { get => _comboSO; private set => _comboSO = value; }
        public Cards.CardSO CraftedCard => _comboSO.CraftedCard;
        public Battle.Deck.DeckEnum GoToDeckAfterCrafting => _comboSO.GoToDeckAfterCrafting;
        public Account.GeneralData.ComboCore ComboCore => _comboCore;
    }
}