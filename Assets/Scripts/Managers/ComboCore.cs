using Battle.Combo;
using CardMaga.Card;
using Newtonsoft.Json;
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class ComboCore
    {
        public int Level;
        public int CoreID;
        [JsonConstructor]
        public ComboCore() { }
 
        public ComboCore(ComboSO comboSO, int level = 0) : this(comboSO?.ID ?? -1, level) { }

        public ComboCore(int coreID, int level = 0)
        {
            if (coreID == -1)
                throw new Exception("Combo is not registered Error Code -1");
            CoreID = coreID;
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
    [Serializable]
    public class ComboInstance : IEquatable<ComboInstance>,IEquatable<ComboCore>,IEquatable<int>
    {
        private static int _uniqueID = 0;
        private static int UniqueID => _uniqueID++;

        [SerializeField,ReadOnly] 
        private ComboCore _comboCore;
        [SerializeField,ReadOnly] 
        private int _instanceID;
        
        public int CoreID => _comboCore.CoreID;
        public ComboSO ComboSo => _comboCore.ComboSO();

        public ComboCore ComboCore => _comboCore;

        public int Level { get => _comboCore.Level; }
        public int InstanceID { get => _instanceID; }

        public ComboInstance(ComboCore comboCore)
        {
            _comboCore = comboCore;
            _instanceID = _uniqueID;
        }

        public bool Equals(ComboInstance other)
        {
            if (other == null) return false;
            return other.InstanceID == InstanceID;
        }

        public bool Equals(ComboCore other)
        {
            if (other == null) return false;
            return other.CoreID == CoreID;
        }

        public bool Equals(int comboCoreId)
        {
            return comboCoreId == CoreID;
        }
    }
    
    public static class ComboHelper
    {
        public static ComboSO ComboSO(this ComboCore c)
    => Factory.GameFactory.Instance.ComboFactoryHandler.GetComboSO(c.CoreID);
    }
}

namespace Battle.Combo
{
    [Serializable]
    public class BattleComboData
    {
        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private Account.GeneralData.ComboCore _comboCore;
        [SerializeField]
        private ComboSO _comboSO;


        public BattleComboData(ComboSO comboSO, int level)
        {
            ComboSO = comboSO;
            _comboCore = new Account.GeneralData.ComboCore(comboSO, level);

        }

        public int CoreID => _comboCore.CoreID;
        public int Level => _comboCore.Level;
        public CardTypeData[] ComboSequence => _comboSO.ComboSequence;
        public ComboSO ComboSO { get => _comboSO; private set => _comboSO = value; }
        public CardSO CraftedCard => _comboSO.CraftedCard;
        public Battle.Deck.DeckEnum GoToDeckAfterCrafting => _comboSO.GoToDeckAfterCrafting;
        public Account.GeneralData.ComboCore ComboCore => _comboCore;
    }
}