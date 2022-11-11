using Battle.Combo;
using CardMaga.Card;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class ComboCore
    {
     
        public int Level;
        public int ID;
        [JsonConstructor]
        public ComboCore() { }
 
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
    public class ComboData
    {
        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private Account.GeneralData.ComboCore _comboCore;
        [SerializeField]
        private ComboSO _comboSO;


        public ComboData(ComboSO comboSO, int level)
        {
            ComboSO = comboSO;
            _comboCore = new Account.GeneralData.ComboCore(comboSO, level);

        }

        public int ID => _comboCore.ID;
        public int Level => _comboCore.Level;
        public CardTypeData[] ComboSequence => _comboSO.ComboSequence;
        public ComboSO ComboSO { get => _comboSO; private set => _comboSO = value; }
        public CardSO CraftedCard => _comboSO.CraftedCard;
        public Battle.Deck.DeckEnum GoToDeckAfterCrafting => _comboSO.GoToDeckAfterCrafting;
        public Account.GeneralData.ComboCore ComboCore => _comboCore;
    }
}