using CardMaga.Card;
using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class CardInstanceID : IEquatable<CardInstanceID>, IDisposable
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
        public int ID => _coreData.CardID;
        public CardSO CardSO => _coreData.CardSO;
        public int Level { get => _coreData.Level; }
        public int InstanceID { get => _instanceID; }

        public CardInstanceID(CardCore card)
        {
            _coreData = card;
            _instanceID = UniqueID;
        }

        public bool Equals(CardInstanceID other)
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
        public CardInstanceID()
        {

        }
#endif
    }

    [Serializable]
    public class CardCore : IDisposable
    {
        [SerializeField,Sirenix.OdinInspector.InlineProperty] private CoreID _coreID;

        [SerializeField] private CardSO _cardSO;
        [SerializeField] private int _level;


        public int CardID => _coreID.ID;
        public CardSO CardSO => _cardSO;
        public int Level => _level;


        public CardCore(CardInstanceID cardInstanceID) : this(cardInstanceID.ID) { }
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
    
        public CardCore(CoreID coreID,CardSO cardSO)
        {
            _coreID = coreID;
            _cardSO = cardSO;
            int baseCardLevel = _cardSO.ID;
            int differences = _coreID.ID - baseCardLevel;
            _level = differences;
        }

        //[Sirenix.OdinInspector.Button]
        //private void Refresh()
        //{
            
        //}
#endif
    }

    [Serializable]
    public class CoreID
    {
        [SerializeField]
        private int _id;
        public int ID => _id;
        public CoreID(int id)
        {
            _id = id;
        }
#if UNITY_EDITOR
        public CoreID() { }
     
#endif
    }

    public static class CardHelper
    {
        public static int GetLevel(int cardID)
        {
            int baseCardLevel = CardSO(cardID).ID;
            int differences = cardID - baseCardLevel;
            return differences;
        }


      
        public static CardSO CardSO(this CardInstanceID card)
    => CardSO(card.ID);
        public static CardSO CardSO(this CardCore card)
            => CardSO(card.CardID);
            public static CardSO CardSO(int id)
          => Factory.GameFactory.Instance.CardFactoryHandler.GetCard(id);
    }
}
