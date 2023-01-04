using CardMaga.Card;
using Sirenix.OdinInspector;
using System;
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
        public int CoreID => _coreData.CardID;
        public CardSO CardSO => _coreData.CardSO;
        public int Level { get => _coreData.Level; }
        public int InstanceID { get => _instanceID; }
        public bool IsMaxLevel => _coreData.CardsAtMaxLevel;
        [ReadOnly,ShowInInspector]
        public int CardsMaxLevel => CardSO.CardsMaxLevel;
        public CardInstance(CardCore card)
        {
            _coreData = card;
            _instanceID = UniqueID;
        }

        public bool Equals(CardInstance other)
        {
            return other.InstanceID == _instanceID;
        }


        public CardCore GetCardCore()
          => _coreData;

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
        [Sirenix.OdinInspector.OnValueChanged("InitInstanceEditor")]
        [SerializeField] private CardSO _cardSO;
        [Sirenix.OdinInspector.OnValueChanged("InitInstanceEditor")]
        [SerializeField] private int _level;


        public int CardID => _coreID.ID;
        public CardSO CardSO => _cardSO;
        public int Level => _level;
        public bool CardsAtMaxLevel => CardSO.CardsMaxLevel - 1 == Level; 
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

        public bool LevelUp()
        {
            if (CardsAtMaxLevel)
                return false;

            _coreID.ID++;
            _level++;

            return true;
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

        private void InitInstanceEditor()
        {
            if(_cardSO!=null)
            _coreID = new CoreID(_cardSO.ID + Level);
        }
#endif
    }

    [Serializable]
    public class CoreID
    {
        // [SerializeField]
        public int ID;
        //      [JsonProperty(PropertyName = "_id")]
        // public int CoreID => _id;
        public CoreID(int id)
        {
            ID = id;
        }
        public CoreID() { }

    }

    public static class CardHelper
    {
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
