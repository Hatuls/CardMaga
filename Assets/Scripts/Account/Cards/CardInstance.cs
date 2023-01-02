using CardMaga.Card;
using System;
using System.Linq;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class CardInstance : IEquatable<CardInstance>, IDisposable
    {
        private static int _uniqueID = 0;
        private static int UniqueID => _uniqueID++;

        #region Fields
        [SerializeField]
        private CardCore _coreData;
        [SerializeField]
        private int _instanceID;

        #endregion

        #region Properties
        public CardCore CardCore => _coreData;
        public int CoreID => CardCore.CardID;
        public CardSO CardSO => CardCore.CardSO;
        public int Level => CardCore.Level; 
        public int InstanceID  => _instanceID; 

        
        public CardInstance(CardCore card)
        {
            _coreData = card;
            _instanceID = UniqueID;
        }

        public bool Equals(CardInstance other)
        {
            return other.InstanceID == _instanceID;
        }

        public CardCore GetCardCore() => _coreData;

        public void Dispose()
        {
            _coreData.Dispose();
        }

        #endregion

#if UNITY_EDITOR
        public CardInstance()
        {

        }
#endif
    }

    [Serializable]
    public class CardCore : IDisposable
    {
        [SerializeField, Sirenix.OdinInspector.InlineProperty]
        private CoreID _coreID;

        [SerializeField] private CardSO _cardSO;
        [SerializeField] private int _level;

        public int CardID => _coreID.ID;
        public CardSO CardSO => _cardSO;
        public int Level => _level;
        public bool IsAtMaxLevel => _level == _cardSO.CardsMaxLevel;
        public CardCore(int iD) : this(new CoreID(iD)) { }

        public CardCore(CoreID coreID)
        {
            _coreID = coreID;
            _cardSO = CardHelper.CardSO(CardID);
            _level = CardHelper.GetLevel(CardID);
            Factory.GameFactory.CardFactory.Register(this);
        }

        public void Dispose()
        {
            Factory.GameFactory.CardFactory.Remove(this);
        }


#if UNITY_EDITOR
        public CardCore() { }

        public CardCore(CoreID coreID, CardSO cardSO)
        {
            _coreID = coreID;
            _cardSO = cardSO;
            int baseCardLevel = _cardSO.ID;
            int differences = _coreID.ID - baseCardLevel;
            _level = differences;
        }
#endif
    }

    [Serializable]
    public class CoreID
    {
        public int ID;

        public CoreID() { }

        public CoreID(int id)
        {
            ID = id; 
        }
    }

    public static class CardHelper
    {
        public static bool IsCardExhausted(this CardCore cardCore)
            => cardCore.CardSO.GetLevelUpgrade(cardCore.Level).UpgradesPerLevel.First(x => x.UpgradeType == LevelUpgradeEnum.ToRemoveExhaust).Amount == 1;
        public static int StaminaCost(this CardCore cardCore)
        => cardCore.CardSO.GetLevelUpgrade(cardCore.Level).UpgradesPerLevel.First(x => x.UpgradeType == LevelUpgradeEnum.Stamina).Amount;
        public static CardTypeData CardTypeData(this CardCore cardCore)
            => cardCore.CardSO.CardTypeData;
        public static string CardName(this CardCore cardCore)
            => cardCore.CardSO.CardName;
        public static int GetCardValue(this CardCore cardCore)
            => cardCore.CardSO.GetCardValue(cardCore.Level);
        public static bool IsCombo(this CardCore cardCore)
        => cardCore.CardSO.IsCombo;
        public static bool IsFusedCard(this CardCore cardCore)
        => cardCore.CardSO.IsFusedCard;
        public static int[] CardsFusesFrom(this CardCore cardCore)
        => cardCore.CardSO.CardsFusesFrom;
        public static RarityEnum Rarity(this CardCore cardCore)
            => cardCore.CardSO.Rarity;

        public static int GetLevel(int cardID)
        {
            int baseCardLevel = CardSO(cardID).ID;
            int differences = cardID - baseCardLevel;
            return differences;
        }
        public static CardSO CardSO(this CardInstance card) => CardSO(card.CoreID);
        public static CardSO CardSO(this CardCore card) => CardSO(card.CardID);
        public static CardSO CardSO(int id) => Factory.GameFactory.Instance.CardFactoryHandler.GetCard(id);
    }
}
